using UnityEngine;

[CreateAssetMenu(fileName = "VoiceSoundGroupSO", menuName = "Message/VoiceSoundGroupSO")]
public class VoiceSoundGroupSO : ScriptableObject
{
     [field:SerializeField] public AudioClip[] MessageAudioClips { get; private set; }
}
