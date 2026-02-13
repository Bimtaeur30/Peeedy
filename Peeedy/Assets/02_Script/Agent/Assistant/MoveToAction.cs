using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveTo", story: "[Agent] Moves to [Target]", category: "Action", id: "ed473cd8842e56aace359c14a7a78589")]
public partial class MoveToAction : Action
{
    public enum TargetPositionMode
    {
        ClosestPointOnAnyCollider,      // 모든 콜라이더 중 가장 가까운 점 사용
        ClosestPointOnTargetCollider,   // 타겟 자체의 콜라이더만 사용
        ExactTargetPosition             // 콜라이더 무시하고 정확한 위치 사용
    }

    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new BlackboardVariable<float>(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    [SerializeReference] public BlackboardVariable<float> SlowDownDistance = new BlackboardVariable<float>(1.0f);

    [Tooltip("내비게이션 타겟 위치 결정 방식")]
    [SerializeReference] public BlackboardVariable<TargetPositionMode> m_TargetPositionMode = new(TargetPositionMode.ClosestPointOnAnyCollider);

    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_ColliderAdjustedTargetPosition;
    [CreateProperty] private float m_OriginalStoppingDistance = -1f;
    [CreateProperty] private float m_OriginalSpeed = -1f;
    private float m_ColliderOffset;
    private float m_CurrentSpeed;
    private Agent m_Assistant;
    private IRenderer renderer;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }
        m_Assistant = Agent.Value.GetComponent<Agent>();
        m_Assistant.GetModule<AgentRunParticler>().PlayParticle();
        renderer = m_Assistant.GetModule<IRenderer>();
        return Initialize();
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        renderer.FlipController(m_NavMeshAgent.velocity.normalized.x);

        // 1. 타겟 위치 실시간 업데이트 체크
        bool boolUpdateTargetPosition = !Mathf.Approximately(m_LastTargetPosition.x, Target.Value.transform.position.x)
            || !Mathf.Approximately(m_LastTargetPosition.y, Target.Value.transform.position.y)
            || !Mathf.Approximately(m_LastTargetPosition.z, Target.Value.transform.position.z);

        if (boolUpdateTargetPosition)
        {
            m_LastTargetPosition = Target.Value.transform.position;
            m_ColliderAdjustedTargetPosition = GetPositionColliderAdjusted();
        }

        float distance = GetDistanceXZ();
        bool destinationReached = distance <= (DistanceThreshold + m_ColliderOffset);

        // 2. 목적지 도착 판정
        if (destinationReached && (m_NavMeshAgent == null || !m_NavMeshAgent.pathPending))
        {
            return Status.Success;
        }

        // 3. 이동 처리
        if (m_NavMeshAgent == null) // Transform 기반 직접 이동
        {
            Vector3 direction = (m_ColliderAdjustedTargetPosition - Agent.Value.transform.position).normalized;

            // 거리 기반 감속 처리
            float step = Speed.Value * Time.deltaTime;
            if (distance < SlowDownDistance.Value)
            {
                step *= (distance / SlowDownDistance.Value);
            }

            Agent.Value.transform.position += direction * step;

            if (direction != Vector3.zero)
            {
                Agent.Value.transform.forward = direction; // 진행 방향 보기
            }

            m_CurrentSpeed = step / Time.deltaTime;
        }
        else // NavMeshAgent 기반 이동
        {
            if (boolUpdateTargetPosition)
            {
                m_NavMeshAgent.SetDestination(m_ColliderAdjustedTargetPosition);
            }
            m_CurrentSpeed = m_NavMeshAgent.velocity.magnitude;
        }

        // 4. 애니메이션 업데이트
        UpdateAnimatorSpeed();

        return Status.Running;
    }

    protected override void OnEnd()
    {
        UpdateAnimatorSpeed(0f); // 정지 애니메이션

        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }
            m_NavMeshAgent.speed = m_OriginalSpeed;
            m_NavMeshAgent.stoppingDistance = m_OriginalStoppingDistance;
        }
        m_Assistant.GetModule<AgentRunParticler>().StopParticle();
        m_NavMeshAgent = null;
        m_Animator = null;
    }

    protected override void OnDeserialize()
    {
        m_NavMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            if (m_OriginalSpeed >= 0f)
                m_NavMeshAgent.speed = m_OriginalSpeed;
            if (m_OriginalStoppingDistance >= 0f)
                m_NavMeshAgent.stoppingDistance = m_OriginalStoppingDistance;

            m_NavMeshAgent.Warp(Agent.Value.transform.position);
        }

        Initialize();
    }

    private Status Initialize()
    {
        m_LastTargetPosition = Target.Value.transform.position;
        m_ColliderAdjustedTargetPosition = GetPositionColliderAdjusted();

        // 콜라이더 크기에 따른 오프셋 계산
        m_ColliderOffset = 0.0f;
        Collider agentCollider = Agent.Value.GetComponentInChildren<Collider>();
        if (agentCollider != null)
        {
            Vector3 colliderExtents = agentCollider.bounds.extents;
            m_ColliderOffset += Mathf.Max(colliderExtents.x, colliderExtents.z);
        }

        if (GetDistanceXZ() <= (DistanceThreshold + m_ColliderOffset))
        {
            return Status.Success;
        }

        m_NavMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }

            m_OriginalSpeed = m_NavMeshAgent.speed;
            m_NavMeshAgent.speed = Speed;
            m_OriginalStoppingDistance = m_NavMeshAgent.stoppingDistance;
            m_NavMeshAgent.stoppingDistance = DistanceThreshold + m_ColliderOffset;
            m_NavMeshAgent.SetDestination(m_ColliderAdjustedTargetPosition);
        }

        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        return Status.Running;
    }

    private Vector3 GetPositionColliderAdjusted()
    {
        switch (m_TargetPositionMode.Value)
        {
            case TargetPositionMode.ClosestPointOnAnyCollider:
                Collider anyCollider = Target.Value.GetComponentInChildren<Collider>(includeInactive: false);
                if (anyCollider == null || anyCollider.enabled == false) break;
                return anyCollider.ClosestPoint(Agent.Value.transform.position);

            case TargetPositionMode.ClosestPointOnTargetCollider:
                Collider targetCollider = Target.Value.GetComponent<Collider>();
                if (targetCollider == null || targetCollider.enabled == false) break;
                return targetCollider.ClosestPoint(Agent.Value.transform.position);
        }
        return Target.Value.transform.position;
    }

    private float GetDistanceXZ()
    {
        Vector3 agentPosition = new Vector3(Agent.Value.transform.position.x, m_ColliderAdjustedTargetPosition.y, Agent.Value.transform.position.z);
        return Vector3.Distance(agentPosition, m_ColliderAdjustedTargetPosition);
    }

    private void UpdateAnimatorSpeed(float explicitSpeed = -1)
    {
        if (m_Animator == null || string.IsNullOrEmpty(AnimatorSpeedParam.Value)) return;

        float speedToSet = explicitSpeed >= 0 ? explicitSpeed : m_CurrentSpeed;
        m_Animator.SetFloat(AnimatorSpeedParam.Value, speedToSet);
    }
}