using UnityEngine;

public static class MarketEvents
{
    public static readonly SellTableInfoOff SellTableInfoOff = new SellTableInfoOff();
    public static readonly SellTableInfoOn SellTableInfoOn = new SellTableInfoOn();
}

public class SellTableInfoOff : GameEvent { }
public class SellTableInfoOn : GameEvent { }
