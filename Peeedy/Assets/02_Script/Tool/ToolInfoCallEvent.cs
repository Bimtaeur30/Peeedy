using UnityEngine;
public class ToolUnEquipEvent : GameEvent { }
public class ToolEquipEvent : GameEvent
{
    public Transform ToolPosition { get; private set; }
    public ToolSO ToolSO { get; private set; }
    public ToolEquipEvent(Transform toolPosition, ToolSO toolSO)
    {
        ToolPosition = toolPosition;
        ToolSO = toolSO;
    }
}
public class ToolInfoCallEvent : GameEvent
{
    public Transform ToolPosition { get; private set; }
    public bool IsActivate { get; private set; }
    public ToolSO ToolSO { get; private set; }
    public ToolInfoCallEvent(Transform toolPosition, bool isActivate, ToolSO toolsO)
    {
        ToolPosition = toolPosition;
        IsActivate = isActivate;
        ToolSO = toolsO;
    }
}
