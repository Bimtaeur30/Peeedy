using DG.Tweening;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SceneEffectManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeImage;
    [field:SerializeField] public EventChannelSO UIChannel { get; private set; }

    private void OnEnable()
    {
        UIChannel.AddListener<FadeEvent>(HandleFadeEvent);
    }

    private void OnDisable()
    {
        UIChannel.RemoveListener<FadeEvent>(HandleFadeEvent);
    }

    private void HandleFadeEvent(FadeEvent @event)
    {
        float fadeValue = @event.IsFadeIn ? 1f : 0f;
        float startValue = @event.IsFadeIn ? 0f : 1f;

        fadeImage.alpha = startValue;

        fadeImage.DOFade(fadeValue, @event.FadeDuration).OnComplete(() =>
        {
            @event.EndCallback?.Invoke();
        });
    }
}
