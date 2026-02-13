using System;
using Unity.AppUI.UI;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayClip", story: "[Assistant] play [Animation]", category: "Action/Animation", id: "d32e421608316963e75536c45400b5ea")]
public partial class PlayClipAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Assistant;
    [SerializeReference] public BlackboardVariable<AnimParamSO> Animation;

    protected override Status OnStart()
    {
        if (Assistant.Value == null || Animation.Value == null || Assistant.Value.GetModule<IRenderer>() == null)
            return Status.Failure;

        Assistant.Value.GetModule<IRenderer>().PlayClip(Animation.Value.ParamHash);

        return Status.Success;
    }

}

