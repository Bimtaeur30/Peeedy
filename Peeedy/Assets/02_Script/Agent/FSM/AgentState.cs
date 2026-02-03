using UnityEngine;

public class AgentState
{
    protected Agent _agent;
    protected int _clipHash;
    protected bool _isTriggerCall;
    protected IRenderer _renderer;

    public AgentState(Agent agent, AnimParamSO paramSO)
    {
        _agent = agent;
        _clipHash = paramSO.ParamHash;
        _renderer = agent.GetModule<IRenderer>();
    }

    public virtual void Update() { }
    public virtual void Enter()
    {
        _renderer.PlayClip(_clipHash);
        _isTriggerCall = false;
    }

    public virtual void Exit() { }

    public virtual void AnimationEndTrigger() => _isTriggerCall = true;
}
