using UnityEditor.Analytics;
using UnityEngine;

public class DoorTriggerEvent : GameEvent
{
    public BuildingSO BuildingSO;
    public bool IsEnter;
    public DoorType DoorType;
    public DoorTriggerEvent(DoorType doorType, BuildingSO buildingSO, bool isEnter) // true = µé¾î¿È, false = ³ª°¨
    {
        DoorType = doorType;
        BuildingSO = buildingSO;
        IsEnter = isEnter;
    }
}
