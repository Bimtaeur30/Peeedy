using UnityEngine;

[CreateAssetMenu(fileName = "Fsm state manager", menuName = "FSM/State list")]
public class StateListSO : ScriptableObject
{
    public string enumName;
    public StateSO[] states;
}
