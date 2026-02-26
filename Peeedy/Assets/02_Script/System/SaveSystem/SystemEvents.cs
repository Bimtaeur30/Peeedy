using UnityEngine;

public class SystemEvents : MonoBehaviour
{
    public static readonly SavePrefEvent savePrefEvent = new SavePrefEvent();
    public static readonly LoadPrefEvent loadPrefEvent = new LoadPrefEvent();
}
public class SavePrefEvent : GameEvent { }
public class LoadPrefEvent : GameEvent { }
