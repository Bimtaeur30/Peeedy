using UnityEngine;

[CreateAssetMenu(fileName = "AnimParamSO", menuName = "Scriptable Objects/AnimParamSO")]
public class AnimParamSO : ScriptableObject
{
    [field:SerializeField] public string ParamName { get; private set; }
    [field:SerializeField] public int ParamHash { get; private set; }

    private void OnValidate()
    {
        ParamHash = Animator.StringToHash(ParamName);
    }
}
