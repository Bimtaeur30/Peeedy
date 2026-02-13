using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetObjRandomPos", story: "Set [Object] random points", category: "Action", id: "a2557a6b0dec6853d83a25762e1e9bc2")]
public partial class SetObjRandomPosAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);

    protected override Status OnStart()
    {
        return Status.Running;
    }
}

