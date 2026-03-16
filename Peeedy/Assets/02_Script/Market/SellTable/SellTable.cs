using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class SellTable : MonoBehaviour
{
    [field: SerializeField] public ToolSO SellTool { get; private set; }
    [field: SerializeField] private TextMeshProUGUI ToolNameTxt;
    [field: SerializeField] private TextMeshProUGUI ToolCostTxt;

    [SerializeField] private CanvasGroup ToolInfoUI;
    [SerializeField] private GameObject Pad;
    [SerializeField] private ConfigurableJoint configurableJoint;
    [SerializeField] private ParticleSystem purchaseEffect; 
    [SerializeField] private EventChannelSO PlayerChannel;
    [SerializeField] private EventChannelSO MarketEventChannel;

    private RectTransform toolInfoUIRec;
    private float toolInfoRecYPos;

    private void Awake()
    {
        ToolNameTxt.text = SellTool.toolName;
        ToolCostTxt.text = SellTool.toolCost.ToString();

        toolInfoUIRec = ToolInfoUI.gameObject.GetComponent<RectTransform>();
        toolInfoRecYPos = toolInfoUIRec.position.y;
    }

    private void OnEnable()
    {
        MarketEventChannel.AddListener<SellTableOn>(HandleInfoOff);
        MarketEventChannel.AddListener<SellTableOff>(HandleInfoOn);

        GameObject tool = Instantiate(SellTool.toolPrefab.gameObject, configurableJoint.gameObject.transform);
        tool.GetComponent<Transform>().localPosition = Vector3.zero;
        configurableJoint.connectedBody = tool.GetComponent<Rigidbody>();
        Set();
    }

    private void Set()
    {
        ToolInfoUI.DOFade(0f, 0.3f);
        toolInfoUIRec.DOAnchorPosY(toolInfoRecYPos - 10f, 0.3f);
        Pad.SetActive(false);
    }

    private void HandleInfoOff(SellTableOn @event = null)
    {
        if (@event.SellTable == this)
        {
            ToolInfoUI.DOFade(0f, 0.3f);
            toolInfoUIRec.DOAnchorPosY(toolInfoRecYPos - 10f, 0.3f);
            Pad.SetActive(false);
        }
    }

    private void HandleInfoOn(SellTableOff @event = null)
    {
        if (@event.SellTable == this)
        {
            // 나라면
            ToolInfoUI.DOFade(1f, 0.3f);
            toolInfoUIRec.DOAnchorPosY(toolInfoRecYPos, 0.3f);
            Pad.SetActive(true);
        }
    }

    public void OnPurchaseRequest()
    {
        PlayerChannel.RaiseEvent(PlayerEvents.SubMoney.Init(SellTool.toolCost, OnPurchase));
    }

    private void OnPurchase(bool val)
    {
        if (val == true)
        {
            PlayerChannel.RaiseEvent(PlayerEvents.AddInventoryTool.Init(SellTool));
            purchaseEffect.Play();
            Debug.Log("성공적으로 구매함.");
        }
        else
        {
            //실패
            Debug.Log("잔액부족, 구매실패");
        }
    }

    //protected override void OnTriggerEnterOverlap(Collider other)
    //{
    //    base.OnTriggerEnterOverlap(other);
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        MarketEventChannel.RaiseEvent(MarketEvents.SellTableInfoOff);
    //        HandleInfoOn();
    //    }
    //}

    //protected override void OnTriggerExitOverlap(Collider other)
    //{
    //    base.OnTriggerExitOverlap(other);
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        MarketEventChannel.RaiseEvent(MarketEvents.SellTableInfoOff);
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        MarketEventChannel.RaiseEvent(MarketEvents.SellTableInfoOff);
    //        HandleInfoOn();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        MarketEventChannel.RaiseEvent(MarketEvents.SellTableInfoOff);
    //    }
    //}
}
