using UnityEngine;

public interface IRenderer
{
    void PlayClip(int clipHash, int layer = -1, float normalPosition = float.NegativeInfinity);
    void SetBool(AnimParamSO param, bool value);
    void SetFloat(AnimParamSO param, float value);
    void SetInt(AnimParamSO param, int value);
    void SetTrigger(AnimParamSO param);
}
