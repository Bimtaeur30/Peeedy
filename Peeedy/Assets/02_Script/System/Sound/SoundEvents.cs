using UnityEngine;

public class SoundEvents : GameEvent
{
    public static readonly PlaySoundEvent PlaySoundEvent = new PlaySoundEvent();
    public static readonly StopSoundEvent StopSoundEvent = new StopSoundEvent();
}

public class PlaySoundEvent : SoundEvents
{
    public Vector3 Position;
    public SoundClipSO ClipData;
    public int ChannelNumber;

    public PlaySoundEvent Init(Vector3 position, SoundClipSO clipData, int channelNumber)
    {
        Position = position;
        ClipData = clipData;
        ChannelNumber = channelNumber;
        return this;
    }
}

public class StopSoundEvent : GameEvent
{
    public int ChannelNumber;

    public StopSoundEvent Init(int channelNumber)
    {
        ChannelNumber = channelNumber;
        return this;
    }
}
