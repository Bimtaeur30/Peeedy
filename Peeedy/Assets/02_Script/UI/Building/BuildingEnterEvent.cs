using UnityEditor.Analytics;
using UnityEngine;

public class BuildingEnterEvent : GameEvent
{
    public BuildingSO BuildingSO;
    public bool IsEnter;
    public BuildingEnterEvent(BuildingSO buildingSO, bool isEnter) // true = µé¾î¿È, false = ³ª°¨
    {
        BuildingSO = buildingSO;
        IsEnter = isEnter;
    }
}
