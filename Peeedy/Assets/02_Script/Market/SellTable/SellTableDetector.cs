using UnityEngine;

public class SellTableDetector : RangeSensor
{
    [SerializeField] private EventChannelSO MarketEventChannel;

    protected override void OnDetected(GameObject obj)
    {
        SellTable st = obj.GetComponent<SellTable>();
        MarketEventChannel.RaiseEvent(MarketEvents.SellTableInfoOn.Init(st));
    }

    protected override void OnUnDetected(GameObject obj)
    {
        Debug.Log(obj.name + " »ç¶óÁü");
        SellTable st = obj.GetComponent<SellTable>();
        MarketEventChannel.RaiseEvent(MarketEvents.SellTableInfoOff.Init(st));
    }
}
