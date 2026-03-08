using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Tool : MonoBehaviour
{
    [SerializeField] private SoundClipSO toolEquipClip;
    [SerializeField] private SoundClipSO toolDropClip;
    [SerializeField] private EventChannelSO toolInfoCallEventChannel;
    [field: SerializeField] protected EventChannelSO soundChannel { get; private set; }
    [field:SerializeField] public ToolSO toolSO { get; private set; }
    private Rigidbody body;
    private AudioSource audioSource;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void EquipTool()
    {
        soundChannel.RaiseEvent(SoundEvents.PlaySoundEvent.Init(transform.position, toolEquipClip, 0));
        //audioSource.PlayOneShot(toolEquipClip);
    }

    public virtual void UnEquipTool()
    {
        soundChannel.RaiseEvent(SoundEvents.PlaySoundEvent.Init(transform.position, toolDropClip, 0));
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
