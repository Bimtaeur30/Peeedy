using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerWalkState : AbstractPlayerState
{
    public PlayerWalkState(Agent agent, AnimParamSO paramSO) : base(agent, paramSO)
    {
    }

    public override void Update()
    {
        base.Update();
        Vector3 Input = _player.PlayerInput.InputDirection;
        _mover.SetMovement(Input);
        if (Mathf.Approximately(Input.x, 0) && Mathf.Approximately(Input.z, 0))
        {
            _player.ChangeState(PlayerStateEnum.IDLE);
        }
    }
}                                                                                                                