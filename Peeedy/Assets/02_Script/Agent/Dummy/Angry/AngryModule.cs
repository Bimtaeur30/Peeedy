using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AngryModule : MonoBehaviour, IModule
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI angryPercentTxt;
    [SerializeField] private Slider angrySlider;

    private Dummy dummy;
    private int angryPercnet = 0;

    public int AngryPercent
    {
        get
        {
            return angryPercnet;
        }
        set
        {
            angryPercnet = Math.Clamp(value, 0, 100);
        }
    }

    public void Initialize(ModuleOwner owner)
    {
        dummy = owner as Dummy;
        SetGageBar();
    }

    private void SetGageBar()
    {
        angrySlider.value = AngryPercent;
        angryPercentTxt.text = AngryPercent.ToString() + "%";
    }

    public void AddAngryRage(int amount)
    {
        AngryPercent += amount;
        SetGageBar();
        CheckAngryRage();
        FadeInBar();

        StopAllCoroutines();
        StartCoroutine(AngryCoolDownCor());
    }

    private void CheckAngryRage()
    {
        if (AngryPercent >= 90)
        {
            dummy.AngryBomb();
        }
    }

    private IEnumerator AngryCoolDownCor()
    {
        yield return new WaitForSeconds(3f);
        FadeOutBar();

        while(AngryPercent > 0)
        {
            yield return new WaitForSeconds(0.1f);
            AngryPercent -= 1;
            SetGageBar();
        }
    }

    private void FadeInBar() => canvasGroup.DOFade(1f, 1f);
    private void FadeOutBar() => canvasGroup.DOFade(0f, 1f);
}
