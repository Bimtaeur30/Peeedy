using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private PoolManagerSO poolManager;
    [SerializeField] private PoolItemSO soundItem;

    [field:SerializeField] public EventChannelSO SoundChannel { get; private set; }
    private readonly Dictionary<int, SoundPlayer> _soundPlayerDict = new Dictionary<int, SoundPlayer>();

    private void Awake()
    {
        SoundChannel.AddListener<PlaySoundEvent>(HandlePlaySoundEvent);
        SoundChannel.AddListener<StopSoundEvent>(HandleStopSoundEvent);
    }

    private void OnDestroy()
    {
        SoundChannel.RemoveListener<PlaySoundEvent>(HandlePlaySoundEvent);
        SoundChannel.RemoveListener<StopSoundEvent>(HandleStopSoundEvent);
    }
    private void HandlePlaySoundEvent(PlaySoundEvent @event)
    {
        SoundPlayer player = poolManager.Pop<SoundPlayer>(soundItem);
        player.transform.position = @event.Position;
        player.PlaySound(@event.ClipData);
        player.OnSoundFinished += HandleSoundFinish;
    }

    private void HandleSoundFinish(SoundPlayer obj)
    {
        obj.OnSoundFinished -= HandleSoundFinish;
        poolManager.Push(obj);
    }

    private void HandleStopSoundEvent(StopSoundEvent @event)
    {
        if (_soundPlayerDict.TryGetValue(@event.ChannelNumber, out SoundPlayer beforePlayer))
        {
            beforePlayer.ForceStopSound();
            beforePlayer.OnSoundFinished -= HandleSoundFinish;
            poolManager.Push(beforePlayer);
            _soundPlayerDict.Remove(@event.ChannelNumber);
        }
    }
}
