using UnityEngine;

public enum AudioTypes
{
    Sfx,
    Bgm
}

[CreateAssetMenu(fileName = "Sound clip data", menuName = "Sound/ClipData")]
public class SoundClipSO : ScriptableObject
{
    public AudioTypes audioTypes;
    public AudioClip audioClip;
    public bool loop = false;
    public bool randomizePitch = false;

    [Range(0, 1f)]
    public float randomPitchModifier = 0.1f;
    [Range(0.1f, 2f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
}
