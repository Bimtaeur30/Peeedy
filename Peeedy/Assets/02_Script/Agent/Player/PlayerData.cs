using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour, IModule, ISaveable
{
    private Player _player;
    [field: SerializeField] public int CurrentExp { get; private set; }
    [field: SerializeField] public int CurrentMoney { get; private set; }
    [field: SerializeField] public List<ToolSO> InventoryTools { get; private set; }
    [field: SerializeField] public EventChannelSO PlayerChannel { get; private set; }

    #region 세이브 로직

    [Header("Save Section")]
    [field: SerializeField] public SaveIdData SaveId { get; private set; }

    [Serializable]
    public struct PlayerSaveData
    {
        public int currentExp;
        public int currentMoney;
        public List<ToolSO> inventoryTools;
    }

    public string GetSaveData()
    {
        PlayerSaveData saveData = new PlayerSaveData()
        {
            currentExp = CurrentExp,
            currentMoney = CurrentMoney,
            inventoryTools = InventoryTools
        };
        return JsonUtility.ToJson(saveData);

    }

    public void RestoreData(string data)
    {
        var parsedData = JsonUtility.FromJson<PlayerSaveData>(data);
        CurrentExp = parsedData.currentExp;
        CurrentMoney = parsedData.currentMoney;
        InventoryTools = parsedData.inventoryTools;
    }
    #endregion

    public void Initialize(ModuleOwner owner)
    {
        _player = owner as Player;
        PlayerChannel.AddListener<AddExp>(HandleAddExp);
        PlayerChannel.AddListener<AddMoney>(HandleAddMoney);
        PlayerChannel.AddListener<SubMoney>(HandleSubMoney);
        PlayerChannel.AddListener<AddInventoryTool>(HandleInvenToolAdd);
        PlayerChannel.AddListener<RemoveInventoryTool>(HandleInvenToolRemove);
    }


    private void Start()
    {
        Load();
    }

    private void OnDestroy()
    {
        PlayerChannel.RemoveListener<AddExp>(HandleAddExp);
        PlayerChannel.RemoveListener<AddMoney>(HandleAddMoney);
        PlayerChannel.RemoveListener<SubMoney>(HandleSubMoney);
        PlayerChannel.RemoveListener<AddInventoryTool>(HandleInvenToolAdd);
    }

    private void Load()
    {
        PlayerChannel.RaiseEvent(PlayerEvents.MoneyChanged.Init(CurrentMoney));
        PlayerChannel.RaiseEvent(PlayerEvents.LoadInventoryTools.Init(InventoryTools));
    }

    private void HandleAddMoney(AddMoney evt) // 돈을 더한다
    {
        CurrentMoney += evt.amount;
        PlayerChannel.RaiseEvent(PlayerEvents.MoneyChanged.Init(CurrentMoney));
    }
    private void HandleSubMoney(SubMoney evt)
    {
        if (CurrentMoney >= evt.amount)
        {
            CurrentMoney -= evt.amount;
            evt.Action?.Invoke(true);
            PlayerChannel.RaiseEvent(PlayerEvents.MoneyChanged.Init(CurrentMoney));
        }
        else
        {
            evt.Action?.Invoke(false);
        }
    }

    private void HandleAddExp(AddExp evt)       
    {
        CurrentExp += evt.amount;
        PlayerChannel.RaiseEvent(PlayerEvents.ExpChanged.Init(CurrentExp));
    }

    private void HandleInvenToolAdd(AddInventoryTool evt)
    {
        InventoryTools.Add(evt.ToolSO);
    }
    private void HandleInvenToolRemove(RemoveInventoryTool evt)
    {
        InventoryTools.Remove(evt.ToolSO);
    }
}