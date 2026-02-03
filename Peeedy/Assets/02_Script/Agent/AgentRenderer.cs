using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentRenderer : MonoBehaviour, IModule, IRenderer
{
    private Agent _owner;
    private Animator _animator;
    public void Initialize(ModuleOwner owner)
    {
        _owner = owner as Agent;
        _animator = GetComponent<Animator>();
    }

    public void PlayClip(int clipHash, int layer = -1, float normalPosition = float.NegativeInfinity)
        => _animator.Play(clipHash, layer, normalPosition);

    public void SetBool(AnimParamSO param, bool value)
        => _animator.SetBool(param.ParamHash, value);
    public void SetFloat(AnimParamSO param, float value)
        => _animator.SetFloat(param.ParamHash, value);
    public void SetInt(AnimParamSO param, int value)
        => _animator.SetInteger(param.ParamHash, value);
    public void SetTrigger(AnimParamSO param)
        => _animator.SetTrigger(param.ParamHash);   
}
