using UnityEngine;

[DefaultExecutionOrder(-20)]
public class FirstLoader : MonoBehaviour
{
    [field: SerializeField] public EventChannelSO UIChannel { get; private set; }

    private void Start()
    {
        //Awake에서는 구독이 일어날 거라 Start에서 진행한다.
        UIChannel.RaiseEvent(UIEvents.FadeEvent.Init(true, 0.3f, null));
    }

}
