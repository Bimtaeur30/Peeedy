using UnityEngine;

public class AbstractPlayerState : AgentState
{
    protected IMover _mover;
    protected Player _player;
    public AbstractPlayerState(Agent agent, AnimParamSO paramSO) : base(agent, paramSO)
    {
        _player = agent as Player;
        Debug.Assert(_player != null, $"{this} is not attached to player");
        _mover = agent.GetModule<IMover>();
    }
}
