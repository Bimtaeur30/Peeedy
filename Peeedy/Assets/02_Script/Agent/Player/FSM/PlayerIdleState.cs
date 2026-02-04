using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : AbstractPlayerState
{
    public PlayerIdleState(Agent agent, AnimParamSO paramSO) : base(agent, paramSO)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _mover.StopImmediately();
    }

    public override void Update()
    {
        base.Update();
        Vector3 Input = _player.PlayerInput.InputDirection;

        if (Mathf.Abs(Input.x) > 0.1f || Mathf.Abs(Input.z) > 0.1f)
        {
            _player.ChangeState(PlayerStateEnum.WALK);
        }
    }
}
