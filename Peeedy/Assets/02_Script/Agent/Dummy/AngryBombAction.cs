using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AngryBomb", story: "[Dummy] angry run", category: "Action", id: "209362f8d9ddbc83ef61f433abe045a9")]
public partial class AngryBombAction : Action
{
    [SerializeReference] public BlackboardVariable<Dummy> Dummy;
    private EventChannelSO systemChannel;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

