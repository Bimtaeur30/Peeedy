using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Dummy : Agent
{
    [SerializeField] private EventChannelSO giftCallEventChannel;

    public ChatHandlerModule ChatHandlerModule { get; private set; }
    public NavMeshAgent agent { get; private set; }

    Coroutine _routine;
    DummyMessageSO _messageSO;

    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    protected override void AfterInitComponents()
    {
        base.AfterInitComponents();
        ChatHandlerModule = GetModule<ChatHandlerModule>();
    }

    // ToolRangeVisualizer가 Enter 시 호출
    public void EnterToolRange(ToolSO toolSO)
    {
        _messageSO = toolSO != null ? toolSO.messageSO : null;

        // 메시지 없으면 아무것도 안 함
        if (_messageSO == null || _messageSO.Messages == null || _messageSO.Messages.Length == 0)
        {
            StopMessageLoop();
            return;
        }

        StartMessageLoop();
    }

    // ToolRangeVisualizer가 Exit 시 호출
    public void ExitToolRange()
    {
        StopMessageLoop();
        _messageSO = null;
    }

    // (선택) 툴이 바뀌었는데 더미가 여전히 범위 안이면 갱신용
    public void UpdateTool(ToolSO toolSO)
    {
        _messageSO = toolSO != null ? toolSO.messageSO : null;

        if (_messageSO == null || _messageSO.Messages == null || _messageSO.Messages.Length == 0)
        {
            StopMessageLoop();
        }
        else if (_routine == null)
        {
            StartMessageLoop();
        }
    }

    void StartMessageLoop()
    {
        if (_routine != null) return;
        _routine = StartCoroutine(MessageLoop());
    }

    void StopMessageLoop()
    {
        if (_routine == null) return;
        StopCoroutine(_routine);
        _routine = null;
    }

    IEnumerator MessageLoop()
    {
        while (true)
        {
            // 메시지 소스가 중간에 날아가도 안전
            if (_messageSO == null || _messageSO.Messages == null || _messageSO.Messages.Length == 0)
            {
                _routine = null;
                yield break;
            }

            int idx = Random.Range(0, _messageSO.Messages.Length);
            ChatHandlerModule.NewChat(_messageSO.Messages[idx]);
            giftCallEventChannel.RaiseEvent(new GiftCallEvent(3000));
            //Debug.Log(_messageSO.Messages[idx]);

            yield return new WaitForSeconds(Random.Range(1f, 2.5f));
        }
    }

    private void OnDisable()
    {
        StopMessageLoop();
    }
}
