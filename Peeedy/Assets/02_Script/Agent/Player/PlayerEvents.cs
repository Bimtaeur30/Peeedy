using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerEvents
{
    public static readonly AddExp AddExp = new AddExp();
    public static readonly ExpChanged ExpChanged = new ExpChanged();
    public static readonly AddMoney AddMoney = new AddMoney();
    public static readonly SubMoney SubMoney = new SubMoney();
    public static readonly MoneyChanged MoneyChanged = new MoneyChanged();
    public static readonly AddInventoryTool AddInventoryTool = new AddInventoryTool();
    public static readonly RemoveInventoryTool RemoveInventoryTool = new RemoveInventoryTool();
    public static readonly LoadInventoryTools LoadInventoryTools = new LoadInventoryTools();
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
public class SubMoney : GameEvent
{
    public Action<bool> Action;
    public int amount;

    public SubMoney Init(int exp, Action<bool> action)
    {
        this.amount = exp;
        this.Action = action;
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

public class AddInventoryTool : GameEvent
{
    public ToolSO ToolSO;

    public AddInventoryTool Init(ToolSO toolSO)
    {
        this.ToolSO = toolSO;
        return this;
    }
}

public class RemoveInventoryTool : GameEvent
{
    public ToolSO ToolSO;

    public RemoveInventoryTool Init(ToolSO toolSO)
    {
        this.ToolSO = toolSO;
        return this;
    }
}

public class LoadInventoryTools : GameEvent
{
    public List<ToolSO> Tools;

    public LoadInventoryTools Init(List<ToolSO> tools)
    {
        this.Tools = tools;
        return this;
    }
}