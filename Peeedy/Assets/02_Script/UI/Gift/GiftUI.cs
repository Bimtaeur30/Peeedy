using DG.Tweening;
using TMPro;
using UnityEngine;

public class GiftUI : MonoBehaviour
{
    [SerializeField] private EventChannelSO giftCallEventChannel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI giftAmountTxt;
    [SerializeField] private TextMeshProUGUI gifterNameTxt;
    [SerializeField] private AudioClip giftSound;

    private Sequence _sequence;
    private AudioSource _audioSource;
    private void Awake()
    {
        _sequence = DOTween.Sequence();
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        giftCallEventChannel.AddListener<GiftCallEvent>(OnGiftCall);
    }

    private void OnGiftCall(GiftCallEvent @event)
    {
        _sequence.Kill();
        _sequence = DOTween.Sequence();

        giftAmountTxt.text = @event.Amount.ToString()+"원 후원!";
        gifterNameTxt.text = "익명시청자 " + Random.Range(1, 100).ToString() + "번";
        _audioSource.PlayOneShot(giftSound);

        _sequence.Append(canvasGroup.DOFade(1f, 0.5f));
        _sequence.AppendInterval(0.5f);
        _sequence.Append(canvasGroup.DOFade(0f, 0.5f));
    }
}
