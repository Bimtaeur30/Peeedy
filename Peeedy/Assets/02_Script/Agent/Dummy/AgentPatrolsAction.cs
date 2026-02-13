using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AgentPatrols", story: "[Agent] Patrols", category: "Action", id: "93efcd828403bc36ab83a5b8012c2d45")]
public partial class AgentPatrolsAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new BlackboardVariable<float>(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    [Tooltip("목적지 근처에서 감속을 시작할 거리")]
    [SerializeReference] public BlackboardVariable<float> SlowDownDistance = new BlackboardVariable<float>(1.0f);

    [Tooltip("랜덤 목적지를 뽑을 반경(Agent 주변)")]
    [SerializeReference] public BlackboardVariable<float> PickRadius = new BlackboardVariable<float>(8.0f);

    [Tooltip("NavMesh.SamplePosition 탐색 거리(클수록 랜덤 점이 NavMesh 밖이어도 붙여줌)")]
    [SerializeReference] public BlackboardVariable<float> SampleMaxDistance = new BlackboardVariable<float>(2.0f);

    [Tooltip("목적지 뽑기 재시도 횟수")]
    [SerializeReference] public BlackboardVariable<int> MaxPickTries = new BlackboardVariable<int>(12);

    [Tooltip("도착하면 Success로 끝낼지, 새 목적지 뽑아서 계속 랜덤 이동할지")]
    [SerializeReference] public BlackboardVariable<bool> LoopRandomMove = new BlackboardVariable<bool>(false);

    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;

    private Vector3 m_Destination;
    private float m_CurrentSpeed;

    [CreateProperty] private float m_OriginalStoppingDistance = -1f;
    [CreateProperty] private float m_OriginalSpeed = -1f;

    private float m_ColliderOffset;

    // 네 기존 구조에 있던 모듈들(있으면 쓰고, 없어도 동작하도록 null-safe)
    private Agent m_Assistant;
    private IRenderer renderer;

    protected override Status OnStart()
    {
        if (Agent.Value == null) return Status.Failure;

        m_Assistant = Agent.Value.GetComponent<Agent>();
        if (m_Assistant != null)
        {
            var particle = m_Assistant.GetModule<AgentRunParticler>();
            particle?.PlayParticle();

            renderer = m_Assistant.GetModule<IRenderer>();
        }

        return Initialize();
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null) return Status.Failure;

        if (m_NavMeshAgent == null) return Status.Failure; // NavMesh 기반 랜덤 이동은 에이전트 필수

        // Flip (선택)
        if (renderer != null)
            renderer.FlipController(m_NavMeshAgent.velocity.sqrMagnitude > 0.0001f ? m_NavMeshAgent.velocity.normalized.x : 0f);

        // 도착 판정: remainingDistance가 안정적이고 pathPending 아닐 때
        bool reached =
            !m_NavMeshAgent.pathPending &&
            m_NavMeshAgent.hasPath &&
            m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance + 0.01f;

        if (reached)
        {
            UpdateAnimatorSpeed(0f);

            if (LoopRandomMove.Value)
            {
                // 다음 목적지 뽑기
                if (!TryPickRandomDestination(out m_Destination))
                    return Status.Failure;

                m_NavMeshAgent.SetDestination(m_Destination);
                return Status.Running;
            }

            return Status.Success;
        }

        // 감속(선택): NavMeshAgent 자체 감속은 speed 조절로 흉내
        float remaining = m_NavMeshAgent.pathPending ? float.PositiveInfinity : m_NavMeshAgent.remainingDistance;

        float desiredSpeed = Speed.Value;
        if (remaining < SlowDownDistance.Value)
        {
            float t = Mathf.Clamp01(remaining / Mathf.Max(0.0001f, SlowDownDistance.Value));
            desiredSpeed *= t;
        }
        m_NavMeshAgent.speed = desiredSpeed;

        m_CurrentSpeed = m_NavMeshAgent.velocity.magnitude;
        UpdateAnimatorSpeed();

        return Status.Running;
    }

    protected override void OnEnd()
    {
        UpdateAnimatorSpeed(0f);

        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
                m_NavMeshAgent.ResetPath();

            if (m_OriginalSpeed >= 0f) m_NavMeshAgent.speed = m_OriginalSpeed;
            if (m_OriginalStoppingDistance >= 0f) m_NavMeshAgent.stoppingDistance = m_OriginalStoppingDistance;
        }

        if (m_Assistant != null)
        {
            var particle = m_Assistant.GetModule<AgentRunParticler>();
            particle?.StopParticle();
        }

        m_NavMeshAgent = null;
        m_Animator = null;
    }

    protected override void OnDeserialize()
    {
        if (Agent.Value == null) return;

        m_NavMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            if (m_OriginalSpeed >= 0f) m_NavMeshAgent.speed = m_OriginalSpeed;
            if (m_OriginalStoppingDistance >= 0f) m_NavMeshAgent.stoppingDistance = m_OriginalStoppingDistance;

            // 리로드 시 위치 싱크
            m_NavMeshAgent.Warp(Agent.Value.transform.position);
        }

        Initialize();
    }

    private Status Initialize()
    {
        // Collider offset (네 코드 유지)
        m_ColliderOffset = 0.0f;
        Collider agentCollider = Agent.Value.GetComponentInChildren<Collider>();
        if (agentCollider != null)
        {
            Vector3 ext = agentCollider.bounds.extents;
            m_ColliderOffset += Mathf.Max(ext.x, ext.z);
        }

        m_NavMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (m_NavMeshAgent == null) return Status.Failure;

        if (!m_NavMeshAgent.isOnNavMesh)
            return Status.Failure;

        if (m_NavMeshAgent.isOnNavMesh)
            m_NavMeshAgent.ResetPath();

        m_OriginalSpeed = m_NavMeshAgent.speed;
        m_OriginalStoppingDistance = m_NavMeshAgent.stoppingDistance;

        // stoppingDistance는 “도착 판정” 기준이라 기존처럼 넣어줌
        m_NavMeshAgent.stoppingDistance = DistanceThreshold.Value + m_ColliderOffset;

        // 첫 목적지 뽑기
        if (!TryPickRandomDestination(out m_Destination))
            return Status.Failure;

        m_NavMeshAgent.speed = Speed.Value;
        m_NavMeshAgent.SetDestination(m_Destination);

        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        return Status.Running;
    }

    private bool TryPickRandomDestination(out Vector3 destination)
    {
        Vector3 origin = Agent.Value.transform.position;

        for (int i = 0; i < MaxPickTries.Value; i++)
        {
            // 1) 원 안에서 랜덤 점(월드)
            Vector2 r = UnityEngine.Random.insideUnitCircle * PickRadius.Value;
            Vector3 randomPoint = origin + new Vector3(r.x, 0f, r.y);

            // 2) NavMesh 위로 스냅
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, SampleMaxDistance.Value, NavMesh.AllAreas))
            {
                destination = hit.position;

                // 3) “정말 갈 수 있나?” 간단 검증(경로 계산)
                if (m_NavMeshAgent != null)
                {
                    var path = new NavMeshPath();
                    if (m_NavMeshAgent.CalculatePath(destination, path) && path.status == NavMeshPathStatus.PathComplete)
                        return true;
                }
                else
                {
                    return true;
                }
            }
        }

        destination = default;
        return false;
    }

    private void UpdateAnimatorSpeed(float explicitSpeed = -1)
    {
        if (m_Animator == null || string.IsNullOrEmpty(AnimatorSpeedParam.Value)) return;

        float speedToSet = explicitSpeed >= 0 ? explicitSpeed : m_CurrentSpeed;
        m_Animator.SetFloat(AnimatorSpeedParam.Value, speedToSet);
    }
}

