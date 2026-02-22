using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MoneyEvent : GameEvent
{
    public int MoneyAmount { get; private set; }

    public MoneyEvent(int moneyAmount)
    {
        this.MoneyAmount = moneyAmount;
    }
}
