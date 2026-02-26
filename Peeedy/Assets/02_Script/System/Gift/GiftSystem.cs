using UnityEngine;

public class GiftSystem : MonoBehaviour
{
    [SerializeField] private EventChannelSO giftCallEventChannel;
    [SerializeField] private EventChannelSO playerChannel;

    private void OnEnable()
    {
        giftCallEventChannel.AddListener<GiftCallEvent>(OnGiftCall);
    }

    private void OnGiftCall(GiftCallEvent @event)
    {
        //MoneyManager.Instance.CurrentMoney += @event.Amount;
        playerChannel.RaiseEvent(PlayerEvents.AddMoney.Init(@event.Amount));
    }
}
