using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SystemEvents;

public class DataManager : MonoBehaviour
{
    [Serializable]
    public struct DataCollection
    {
        public List<SaveData> dataCollection;
    }

    [Serializable]
    public struct SaveData
    {
        public int Id;
        public string Data;
    }

    [SerializeField] private string prefKey = "saveData";

    private List<SaveData> _unUsedData = new List<SaveData>();

    [field: SerializeField] public EventChannelSO SystemChannel { get; private set; }

    private void Awake()
    {
        SystemChannel.AddListener<SavePrefEvent>(HandleSavePrefEvent);
        SystemChannel.AddListener<LoadPrefEvent>(HandleLoadPrefEvent);
    }

    private void OnDestroy()
    {
        SystemChannel.RemoveListener<SavePrefEvent>(HandleSavePrefEvent);
        SystemChannel.RemoveListener<LoadPrefEvent>(HandleLoadPrefEvent);
    }

    private void OnApplicationQuit()
    {
        HandleSavePrefEvent();
    }

    #region 데이터 세이브 로직

    private void HandleSavePrefEvent(SavePrefEvent evt = null)
    {
        string saveData = GetSceneSaveData();
        PlayerPrefs.SetString(prefKey, saveData);
        PlayerPrefs.Save();
        Debug.Log($"Save Data : {saveData}");
    }

    private string GetSceneSaveData()
    {
        IEnumerable<ISaveable> saveableObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>();

        List<SaveData> toSaveData = new List<SaveData>();
        foreach (ISaveable saveable in saveableObjects)
        {
            toSaveData.Add(new SaveData { Id = saveable.SaveId.Id, Data = saveable.GetSaveData() });
        }
        toSaveData.AddRange(_unUsedData); //이번 씬에서 사용하지 않았던 데이터도 같이 저장한다.
        DataCollection dataCollection = new DataCollection { dataCollection = toSaveData };

        return JsonUtility.ToJson(dataCollection);
    }

    #endregion

    #region 데이터 로드 관련 로직

    private void HandleLoadPrefEvent(LoadPrefEvent evt)
    {
        string loadJson = PlayerPrefs.GetString(prefKey, string.Empty);
        RestoreData(loadJson);
    }

    private void RestoreData(string json)
    {
        IEnumerable<ISaveable> saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>();
        DataCollection parsedData = string.IsNullOrEmpty(json)
            ? new DataCollection()
            : JsonUtility.FromJson<DataCollection>(json);

        _unUsedData.Clear();

        if (parsedData.dataCollection != null)
        {
            foreach (SaveData saveData in parsedData.dataCollection)
            {
                ISaveable saveable = saveables.FirstOrDefault(s => s.SaveId.Id == saveData.Id);
                if (saveable != null)
                    saveable.RestoreData(saveData.Data);
                else
                    _unUsedData.Add(saveData);
            }
        }

    }

    #endregion

    [ContextMenu("Clear Pref Data")]
    public void ClearPrefData()
    {
        PlayerPrefs.DeleteKey(prefKey);
    }
}
