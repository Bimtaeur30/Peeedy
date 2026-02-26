using UnityEngine;

public static class PlayerEvents
{
    public static readonly AddExp AddExp = new AddExp();
    public static readonly ExpChanged ExpChanged = new ExpChanged();
    public static readonly AddMoney AddMoney = new AddMoney();
    public static readonly MoneyChanged MoneyChanged = new MoneyChanged();
}

public class AddExp : GameEvent
{
    public int amount;

    public AddExp Init(int exp)
    {
        this.amount = exp;
        return this;
    }
}
public class ExpChanged : GameEvent
{
    public int amount;

    public ExpChanged Init(int exp)
    {
        this.amount = exp;
        return this;
    }
}
public class AddMoney : GameEvent
{
    public int amount;

    public AddMoney Init(int exp)
    {
        this.amount = exp;
        return this;
    }
}
public class MoneyChanged: GameEvent
{
    public int amount;

    public MoneyChanged Init(int exp)
    {
        this.amount = exp;
        return this;
    }
}
