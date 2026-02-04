using System;
using System.Collections.Generic;
using UnityEngine;

public class AgentStateMachine
{
    public AgentState CurrentState { get; private set; }
    private Dictionary<int, AgentState> _stateDict;
    public AgentStateMachine(Agent agent, StateSO[] stateList)
    {
        _stateDict = new Dictionary<int, AgentState>();
         
        foreach(StateSO stateData in stateList)
        {
            Type type = Type.GetType(stateData.className);
            Debug.Assert(type != null, $"Finding type is null {stateData.className}");//안전코드
            AgentState agentState = Activator.CreateInstance(type, agent, stateData.stateParam) as AgentState;

            _stateDict.Add(stateData.stateIndex, agentState);
        }
    }

    public void ChangeState(int newStateIndex)
    {
        CurrentState?.Exit();
        AgentState newState = _stateDict.GetValueOrDefault(newStateIndex);
        Debug.Assert(newState != null, $"State is null : {newStateIndex}");

        CurrentState = newState;
        CurrentState.Enter();
    }

    public void UpdateMachine()
    {
        CurrentState?.Update();
    }
}