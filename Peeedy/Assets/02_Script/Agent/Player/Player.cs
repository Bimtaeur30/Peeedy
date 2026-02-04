using UnityEngine;

public class Player : Agent
{
    [SerializeField] private StateListSO stateList;

    private AgentStateMachine _stateMachine;

    [field:SerializeField] public PlayerInputSO PlayerInput { get; private set; }

    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        _stateMachine = new AgentStateMachine(this, stateList.states);
    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.ChangeState((int)PlayerStateEnum.IDLE);
    }

    private void Update()
    {
        _stateMachine.UpdateMachine();
    }

    public void ChangeState(PlayerStateEnum newState)
    {
        _stateMachine.ChangeState((int)newState);
    }

    public AgentState GetCurrentState() => _stateMachine.CurrentState;
}
