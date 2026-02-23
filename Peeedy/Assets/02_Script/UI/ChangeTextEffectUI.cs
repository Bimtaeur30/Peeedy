using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeTextEffectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private float effectDuration = 0.5f;

    [SerializeField] private List<string> textList = new List<string>();
    private int currentIndex = 0;
    
    private void Start()
    {
        if (textList.Count > 0)
        {
            StartCoroutine(TextChangeLoop());
        }
    }

    IEnumerator TextChangeLoop()
    {
        while (true)
        {
            textMeshProUGUI.text = textList[currentIndex];
            currentIndex = (currentIndex + 1) % textList.Count; // 다음 텍스트로 이동, 리스트 끝에 도달하면 처음으로 돌아감

            yield return new WaitForSeconds(effectDuration);
        }
    }
}
