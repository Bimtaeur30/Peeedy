using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class UIEvents
{
    public static readonly FadeEvent FadeEvent = new FadeEvent();
    public static readonly InventoryToggleEvent InventoryToggleEvent = new InventoryToggleEvent();
    public static readonly InventoryDrawEvent InventoryDrawEvent = new InventoryDrawEvent();
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
public class InventoryToggleEvent : GameEvent
{
    public bool IsOpen;
    public InventoryToggleEvent Init(bool isOpen)
    {
        IsOpen = isOpen;
        return this;
    }
}
public class InventoryDrawEvent : GameEvent
{
    public int TotalPage;
    public int CurrentPage;
    public ToolSO ToolSO;
    public InventoryDrawEvent Init(ToolSO toolSO, int currentPage, int totalPage)
    {
        TotalPage = totalPage;
        CurrentPage = currentPage;
        ToolSO = toolSO;
        return this;
    }
}

public class InventoryNullDrawEvent : GameEvent { }
