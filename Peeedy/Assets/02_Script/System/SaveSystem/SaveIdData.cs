using UnityEngine;

[CreateAssetMenu(fileName = "Save id", menuName = "System/Save id", order = 10)]
public class SaveIdData : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [SerializeField, TextArea] private string description;
}
