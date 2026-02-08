using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class Assistant : Agent
{
    public BehaviorGraphAgent BTAgent { get; private set; }
    public IRenderer Renderer { get; private set; }

    public NavMeshAgent agent { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        BTAgent = GetComponent<BehaviorGraphAgent>();
        Debug.Assert(BTAgent != null, "Assistant requires a BehaviorGraphAgent component.");

        Renderer = GetModule<IRenderer>();
        Debug.Assert(Renderer != null, "Assistant requires an IRenderer module.");

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    public void SetVariableValue<T>(string variableName, T value)
    {
        Debug.Assert(!string.IsNullOrEmpty(variableName), "Variable name cannot be null or empty.");

        if (BTAgent.GetVariable<T>(variableName, out BlackboardVariable<T> variable))
        {
            variable.Value = value;
        }
        else
        {
            Debug.LogError($"Variable '{variableName}' not found in BTAgent.");
        }
    }

    public bool GetVariableValue<T>(string variableName, out BlackboardVariable<T> variable)
    {
        Debug.Assert(!string.IsNullOrEmpty(variableName), "Variable name cannot be null or empty.");

        return BTAgent.GetVariable<T>(variableName, out variable);
    }
}
