using UnityEngine;

public class SystemEvents : MonoBehaviour
{
    public static readonly SavePrefEvent savePrefEvent = new SavePrefEvent();
    public static readonly LoadPrefEvent loadPrefEvent = new LoadPrefEvent();
    public static readonly PoliceCallEvent policeCallEvent = new PoliceCallEvent();
}
public class SavePrefEvent : GameEvent { }
public class LoadPrefEvent : GameEvent { }

public class PoliceCallEvent : GameEvent { }
