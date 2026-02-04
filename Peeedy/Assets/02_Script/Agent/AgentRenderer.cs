using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class AgentRenderer : MonoBehaviour, IModule, IRenderer
{
    private Agent _owner;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [field: SerializeField] public float FacingDirection { get; private set; } = -1f; // Ã·¿£ ¿ÞÂÊ º¸´ÂÁß


    public void Initialize(ModuleOwner owner)
    {
        _owner = owner as Agent;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void FlipController(float xMove)
    {
        if (Mathf.Abs(FacingDirection + xMove) < 0.5f)
            Flip();
    }

    private void Flip()
    {
        FacingDirection *= -1;
        _spriteRenderer.flipX = _spriteRenderer.flipX ? false : true;
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
