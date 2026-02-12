using UnityEngine;

[CreateAssetMenu(fileName = "DummyMessageSO", menuName = "Message/DummyMessageSO")]
public class DummyMessageSO : ScriptableObject
{
    [field: SerializeField] public NuisanceTagEnum Tag;
    [field: SerializeField] public string[] Messages;
}
