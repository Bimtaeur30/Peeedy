using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Chat : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private Vector2 padding;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Setup(string text)
    {
        // 텍스트 설정 및 업데이트
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        backgroundSpriteRenderer.size = textSize + padding;

        _animator.SetTrigger("POP");
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}