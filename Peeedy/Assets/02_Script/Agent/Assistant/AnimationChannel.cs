using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/AnimationChannel")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "AnimationChannel", message: "Set Animation to [Clip]", category: "Events", id: "c32c27deac013b74244d3ce441f3568e")]
public sealed partial class AnimationChannel : EventChannel<AnimParamSO> { }

