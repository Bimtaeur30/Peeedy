using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Tool : MonoBehaviour
{
    [SerializeField] private AudioClip toolEquipClip;
    [SerializeField] private AudioClip toolDropClip;
    [SerializeField] private EventChannelSO toolInfoCallEventChannel;
    [SerializeField] private ToolSO toolSO;
    private Rigidbody body;
    private AudioSource audioSource;

    //public bool IsToolEquiped { get; private set; }
    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void EquipTool()
    {
        audioSource.PlayOneShot(toolEquipClip);
    }
    public virtual void UnEquipTool()
    {
        audioSource.PlayOneShot(toolDropClip);
    }

    public virtual void ShowToolLabel()
    {
        toolInfoCallEventChannel.RaiseEvent(new ToolInfoCallEvent(gameObject.transform, true, toolSO));
    }

    public virtual void HideToolLabel()
    {
        toolInfoCallEventChannel.RaiseEvent(new ToolInfoCallEvent(gameObject.transform, false, toolSO));
    }

    public Rigidbody GetRigidbody()
    {
        return body;
    }
}
