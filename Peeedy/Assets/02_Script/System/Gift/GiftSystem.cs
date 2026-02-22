using UnityEngine;

public class GiftSystem : MonoBehaviour
{
    [SerializeField] private EventChannelSO giftCallEventChannel;

    private void OnEnable()
    {
        giftCallEventChannel.AddListener<GiftCallEvent>(OnGiftCall);
    }

    private void OnGiftCall(GiftCallEvent @event)
    {
        MoneyManager.Instance.CurrentMoney += @event.Amount;
    }
}
