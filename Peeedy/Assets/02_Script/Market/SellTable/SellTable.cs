using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class SellTable : OverlapTrigger
{
    [field: SerializeField] public ToolSO SellTool { get; private set; }
    [field: SerializeField] private TextMeshProUGUI ToolNameTxt;
    [field: SerializeField] private TextMeshProUGUI ToolCostTxt;
    [SerializeField] private CanvasGroup ToolInfoUI;
    [SerializeField] private EventChannelSO MarketEventChannel;
    [SerializeField] private GameObject Pad;
    [SerializeField] private ConfigurableJoint configurableJoint;

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
        MarketEventChannel.AddListener<SellTableInfoOff>(HandleInfoOff);
        MarketEventChannel.AddListener<SellTableInfoOn>(HandleInfoOn);

        GameObject tool = Instantiate(SellTool.toolPrefab.gameObject, configurableJoint.gameObject.transform);
        tool.GetComponent<Transform>().localPosition = Vector3.zero;
        configurableJoint.connectedBody = tool.GetComponent<Rigidbody>();
        HandleInfoOff();
    }

    private void HandleInfoOff(SellTableInfoOff @event = null)
    {
        ToolInfoUI.DOFade(0f, 0.3f);
        toolInfoUIRec.DOAnchorPosY(toolInfoRecYPos - 10f, 0.3f);
        Pad.SetActive(false);
    }

    private void HandleInfoOn(SellTableInfoOn @event = null)
    {
        ToolInfoUI.DOFade(1f, 0.3f);
        toolInfoUIRec.DOAnchorPosY(toolInfoRecYPos, 0.3f);
        Pad.SetActive(true);
    }

    protected override void OnTriggerEnterOverlap(Collider other)
    {
        base.OnTriggerEnterOverlap(other);
        if (other.gameObject.CompareTag("Player"))
        {
            MarketEventChannel.RaiseEvent(MarketEvents.SellTableInfoOff);
            HandleInfoOn();
        }
    }

    protected override void OnTriggerExitOverlap(Collider other)
    {
        base.OnTriggerExitOverlap(other);
        if (other.gameObject.CompareTag("Player"))
        {
            MarketEventChannel.RaiseEvent(MarketEvents.SellTableInfoOff);
        }
    }

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
