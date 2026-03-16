using UnityEngine;

public static class MarketEvents
{
    public static readonly SellTableOn SellTableInfoOff = new SellTableOn();
    public static readonly SellTableOff SellTableInfoOn = new SellTableOff();
}

public class SellTableOn : GameEvent
{
    public SellTable SellTable { get; private set; }
    public SellTableOn Init(SellTable sellTable)
    {
        SellTable = sellTable;
        return this;
    }
}
public class SellTableOff : GameEvent
{
    public SellTable SellTable { get; private set; }
    public SellTableOff Init(SellTable sellTable)
    {
        SellTable = sellTable;
        return this;
    }
}
