using UnityEngine;

[CreateAssetMenu(fileName = "State data", menuName = "FSM/State data")]
public class StateSO : ScriptableObject
{
    public string stateName;
    public string className;
    public int stateIndex;
    public AnimParamSO stateParam;
}
