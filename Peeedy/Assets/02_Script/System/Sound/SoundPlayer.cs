using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;


[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour, IPoolable
{
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup bgmGroup;

    private AudioSource _audioSource;
    public GameObject GameObject => this == null ? null : gameObject;
    [field:SerializeField] public PoolItemSO PoolItem { get; set; }

    public event Action<SoundPlayer> OnSoundFinished;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundClipSO clipData)
    {
        if (clipData.audioTypes == AudioTypes.Sfx)
        {
            _audioSource.outputAudioMixerGroup = bgmGroup;
        }
        else
        {
            _audioSource.outputAudioMixerGroup = sfxGroup;
        }

        _audioSource.volume = clipData.volume;
        _audioSource.pitch = clipData.pitch;
        if (clipData.randomizePitch)
        {
            _audioSource.pitch += Random.Range(-clipData.randomPitchModifier, clipData.randomPitchModifier);
        }

        _audioSource.clip = clipData.audioClip;
        _audioSource.loop = clipData.loop;

        if (!clipData.loop)
        {
            float time = _audioSource.clip.length + 2f;
            _ = DisableSoundTimer(time);
            _audioSource.Play();
        }
    }

    public void ResetItem()
    {
        //do nothing
    }

    private async Task DisableSoundTimer(float time)
    {
        await Awaitable.WaitForSecondsAsync(time);
        OnSoundFinished?.Invoke(this);
        Debug.Log("Sound Finished");
    }

    public void ForceStopSound()
    {
        _audioSource.Stop();
    }
}
