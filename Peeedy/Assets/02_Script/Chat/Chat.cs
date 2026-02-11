using TMPro;
using UnityEngine;

public class Chat : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private Vector2 padding;

    private void Start()
    {
        // 테스트용 호출
        Setup("으악 너무 시끄러워!");
    }

    private void Setup(string text)
    {
        // 텍스트 설정 및 업데이트
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();

        // 텍스트의 실제 렌더링된 크기를 가져옵니다.
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        // 여백(Padding) 설정
        //Vector2 padding = new Vector2(4f, 2f);

        // 배경 스프라이트의 크기를 (텍스트 크기 + 여백)으로 조절합니다.
        // 주의: 배경 스프라이트의 Draw Mode가 'Sliced'로 되어 있어야 합니다.
        backgroundSpriteRenderer.size = textSize + padding;
    }
}