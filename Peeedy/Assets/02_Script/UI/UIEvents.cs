using System;
using UnityEngine;

public static class UIEvents
{
    public static readonly FadeEvent FadeEvent = new FadeEvent();
}

public class FadeEvent : GameEvent
{
    public bool IsFadeIn;
    public float FadeDuration;
    public Action EndCallback;

    public FadeEvent Init(bool isFadeIn, float fadeDuration, Action endCallback = null)
    {
        IsFadeIn = isFadeIn;
        FadeDuration = fadeDuration;
        EndCallback = endCallback;
        return this;
    }
}
