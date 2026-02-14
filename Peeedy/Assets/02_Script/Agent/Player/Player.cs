using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Agent
{
    [SerializeField] private StateListSO stateList;

    private AgentStateMachine _stateMachine;
    private ICombatModule _combatModule;
    private ToolHandlerModule _toolHandlerModule;

    [field:SerializeField] public PlayerInputSO PlayerInput { get; private set; }

    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        _stateMachine = new AgentStateMachine(this, stateList.states);
    }

    protected override void AfterInitComponents()
    {
        base.AfterInitComponents();
        _combatModule = GetModule<ICombatModule>();
        _toolHandlerModule = GetModule<ToolHandlerModule>();

        PlayerInput.LeftMouseClickEvent += HandleAttackKeyPressed;
        PlayerInput.OnToolEquipEvent += HandleToolEquipKeyPressed;
        PlayerInput.OnToolUnEquipEvent += HandleToolUnEquipKeyPressed;
    }

    private void HandleToolUnEquipKeyPressed()
    {
        _toolHandlerModule.UnEquipTool();
    }

    private void HandleToolEquipKeyPressed()
    {
        _toolHandlerModule.EquipTool();
    }

    private void HandleAttackKeyPressed()
    {
        _combatModule.Attack();
    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.ChangeState((int)PlayerStateEnum.IDLE);
    }

    private void Update()
    {
        _stateMachine.UpdateMachine();

        // 마우스 위치를 가져옴
        Vector3 mousePos = Input.mousePosition;

        // 카메라와 오브젝트 사이의 거리를 Z값으로 넣어줘야 정확한 월드 좌표가 나옵니다.
        // 만약 카메라가 Z: -10, 플레이어가 Z: 0에 있다면 거리는 10입니다.
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        _combatModule.Aim(worldMousePos);
    }

    public void ChangeState(PlayerStateEnum newState)
    {
        _stateMachine.ChangeState((int)newState);
    }

    public AgentState GetCurrentState() => _stateMachine.CurrentState;
}
