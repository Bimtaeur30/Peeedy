using System;
using UnityEngine;

public class PlayerData : MonoBehaviour, IModule, ISaveable
{
    private Player _player;
    [field: SerializeField] public int CurrentExp { get; private set; }
    [field: SerializeField] public int CurrentMoney { get; private set; }

    [field: SerializeField] public EventChannelSO PlayerChannel { get; private set; }

    #region 세이브 로직

    [Header("Save Section")]
    [field: SerializeField] public SaveIdData SaveId { get; private set; }

    [Serializable]
    public struct PlayerSaveData
    {
        public int currentExp;
        public int currentMoney;
    }

    public string GetSaveData()
    {
        PlayerSaveData saveData = new PlayerSaveData()
        {
            currentExp = CurrentExp,
            currentMoney = CurrentMoney
        };
        return JsonUtility.ToJson(saveData);

    }

    public void RestoreData(string data)
    {
        var parsedData = JsonUtility.FromJson<PlayerSaveData>(data);
        CurrentExp = parsedData.currentExp;
        CurrentMoney = parsedData.currentMoney;
    }
    #endregion


    public void Initialize(ModuleOwner owner)
    {
        _player = owner as Player;
        PlayerChannel.AddListener<AddExp>(HandleAddExp);
        PlayerChannel.AddListener<AddMoney>(HandleAddMoney);
    }

    private void OnDestroy()
    {
        PlayerChannel.RemoveListener<AddExp>(HandleAddExp);
        PlayerChannel.RemoveListener<AddMoney>(HandleAddMoney);
    }

    private void HandleAddMoney(AddMoney evt)
    {
        CurrentMoney += evt.amount;
        PlayerChannel.RaiseEvent(PlayerEvents.MoneyChanged.Init(CurrentMoney));
    }
    private void HandleAddExp(AddExp evt)       
    {
        CurrentExp += evt.amount;
        PlayerChannel.RaiseEvent(PlayerEvents.ExpChanged.Init(CurrentExp));
    }
}