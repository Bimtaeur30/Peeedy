using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomMessageAction", story: "[Agent] Speack [RandomMessage]", category: "Action", id: "75f856501670aeb86b82616c4246fe8e")]
public partial class RandomMessageAction : Action
{
    [SerializeReference] public BlackboardVariable<Dummy> Agent;
    [SerializeReference] public BlackboardVariable<DummyMessageSO> RandomMessage;
    string[] _messages;

    protected override Status OnStart()
    {
        _messages = RandomMessage.Value.Messages;
        Agent.Value.ChatHandlerModule.NewChat(_messages[UnityEngine.Random.Range(0, _messages.Length - 1)]);
        return Status.Success;
    }
}

