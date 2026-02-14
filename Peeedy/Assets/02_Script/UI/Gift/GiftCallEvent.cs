using UnityEngine;

public class GiftCallEvent : GameEvent
{
    public int Amount { get; private set; }
    public GiftCallEvent(int amount)
    {
        Amount = amount;
    }
}
