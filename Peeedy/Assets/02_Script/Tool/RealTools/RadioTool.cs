using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RadioTool : Tool
{
    //[SerializeField] private SoundClipSO song;
    [SerializeField] private AudioClip song;
    private AudioSource source;
    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
    }
    public override void EquipTool()
    {
        base.EquipTool();
        source.clip = song;
        //soundChannel.RaiseEvent(SoundEvents.PlaySoundEvent.Init(transform.position, song, 0));
        source.Play();
    }

    public override void UnEquipTool()
    {
        base.UnEquipTool();
        source.Stop();
    }
}
