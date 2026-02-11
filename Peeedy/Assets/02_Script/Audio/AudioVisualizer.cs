using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    public AudioSource audioSource; // 소리를 가져올 오디오 소스
    public float minScale = 0.4f;   // 최소 크기
    public float maxScale = 0.6f;   // 최대 크기
    public float sensitivity = 100f; // 민감도 (값에 따라 조절)
    public float smoothness = 10f;  // 크기 변화의 부드러움 정도

    private float[] samples = new float[512]; // 샘플 데이터를 담을 배열

    void Update()
    {
        // 1. 오디오 소스로부터 샘플 데이터 추출
        audioSource.GetOutputData(samples, 0);

        // 2. 음량(RMS) 계산
        float sum = 0;
        foreach (float sample in samples)
        {
            sum += sample * sample; // 제곱의 합
        }
        float rmsValue = Mathf.Sqrt(sum / samples.Length); // 제곱평균제곱근

        // 3. 음량 값을 기반으로 타겟 스케일 계산
        // Clamp를 이용해 0.4 ~ 0.6 사이로 제한
        float targetScale = Mathf.Clamp(minScale + (rmsValue * sensitivity), minScale, maxScale);

        // 4. 부드럽게 크기 변경 (Lerp 사용)
        float currentScale = Mathf.Lerp(transform.localScale.x, targetScale, Time.deltaTime * smoothness);
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }
}
