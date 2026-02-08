using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerWalkState : AbstractPlayerState
{
    private ParticleSystem _particle;
    public PlayerWalkState(Agent agent, AnimParamSO paramSO) : base(agent, paramSO)
    {
        _particle = _mover.GetParticle();
    }

    public override void Enter()
    {
        base.Enter();
        _particle.Play();
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

    public override void Exit()
    {
        base.Exit();
        _particle.Stop();
    }
}                                                                                                                