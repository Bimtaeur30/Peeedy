using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RadioTool : Tool
{
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
        source.Play();
    }

    public override void UnEquipTool()
    {
        base.UnEquipTool();
        source.Stop();
    }
}
