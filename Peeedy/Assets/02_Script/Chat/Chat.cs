using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Chat : PoolableMono
{
    [SerializeField] private PoolManagerSO poolManager;
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private Vector2 padding;


    private Transform _followTarget;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (_followTarget == null) return;
        transform.position = _followTarget.position;
    }
    

    public void Setup(string text, Transform followTarget)
    {
        _followTarget = followTarget;

        // 텍스트 설정 및 업데이트
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        backgroundSpriteRenderer.size = textSize + padding;

        _animator.SetTrigger("POP");
    }

    public void Close()
    {
        poolManager.Push(this);
    }

    public void ResetItem(){ }
}