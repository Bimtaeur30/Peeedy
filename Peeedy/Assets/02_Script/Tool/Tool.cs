using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Tool : MonoBehaviour
{
    [SerializeField] private EventChannelSO toolInfoCallEventChannel;
    [SerializeField] private ToolSO toolSO;
    private Rigidbody body;

    //public bool IsToolEquiped { get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    //public void EquipTool() => IsToolEquiped = true;
    //public void UnEquipTool() => IsToolEquiped = false;

    public void ShowToolLabel()
    {
        toolInfoCallEventChannel.RaiseEvent(new ToolInfoCallEvent(gameObject.transform, true, toolSO));
    }

    public void HideToolLabel()
    {
        toolInfoCallEventChannel.RaiseEvent(new ToolInfoCallEvent(gameObject.transform, false, toolSO));
    }

    public Rigidbody GetRigidbody()
    {
        return body;
    }
}
