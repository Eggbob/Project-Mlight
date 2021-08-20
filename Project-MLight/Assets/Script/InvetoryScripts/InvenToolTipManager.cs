using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenToolTipManager : MonoBehaviour
{
    [SerializeField]
    private Text nameTxt; //아이템 이름
    [SerializeField]
    private Text toolTipTxt; //아이템 설명
    [SerializeField]
    private Image ItemImg; //아이템 이미지
    [SerializeField]
    private Image countImg; //수량 이미지
    [SerializeField]
    private Text countTxt; //수량 텍스트
    [SerializeField]
    private Text priceTxt; //아이템 가치 텍스트
    [SerializeField]
    private Button okBtn; //확인버튼
    [SerializeField]
    private Button sellBtn;

    [SerializeField]
    private Button dumpBtn; //버리기 버튼

    //확인버튼 누를시 동작
    private event Action OkBtnEvent;
    private event Action OkBtnEvent2;
    private event Action DumpBtnEvent;
    private event Action SellBtnEvent;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        this.transform.SetAsLastSibling();
    }

    private void Init()
    {
        okBtn.onClick.AddListener(() => OkBtnEvent());
        okBtn.onClick.AddListener(() => OkBtnEvent2());
        dumpBtn.onClick.AddListener(() => DumpBtnEvent());
        sellBtn.onClick.AddListener(() => SellBtnEvent());
    }

    //사용가능한 아이템 설정
    public void SetItemInfo(ItemData data, Action okCallback1, Action okCallback2, Action dumpCallback, int amount)
    {
        nameTxt.text = data.ItemName;
        toolTipTxt.text = data.Tooltip;
        ItemImg.sprite = data.IconSprite;
        countTxt.text = amount.ToString();
        priceTxt.text = data.ItemSellPrice.ToString() + "G";
        okBtn.gameObject.SetActive(true);
        dumpBtn.gameObject.SetActive(true);
        sellBtn.gameObject.SetActive(false);

        //버튼 이벤트 설정
        SetOkBtn(okCallback1);
        SetOkBtn2(okCallback2);
        SetDumpBtn(dumpCallback);

        this.gameObject.SetActive(true);
    }

    //사용 불가능한 아이템 설정
    public void SetPropItemInfo(ItemData data, Action dumpCallback, int amount)
    {
        nameTxt.text = data.ItemName;
        toolTipTxt.text = data.Tooltip;
        ItemImg.sprite = data.IconSprite;
        countTxt.text = amount.ToString();
        priceTxt.text = data.ItemSellPrice.ToString() + "G";

        okBtn.gameObject.SetActive(false);
        dumpBtn.gameObject.SetActive(true);
        sellBtn.gameObject.SetActive(false);

        SetDumpBtn(dumpCallback);
        this.gameObject.SetActive(true);
    }

    //판매 아이템 설정
    public void SetSellItemInfo(ItemData data, Action sellCallback, int amount)
    {
        nameTxt.text = data.ItemName;
        toolTipTxt.text = data.Tooltip;
        ItemImg.sprite = data.IconSprite;
        countTxt.text = amount.ToString();
        priceTxt.text = data.ItemSellPrice.ToString() + "G";

        okBtn.gameObject.SetActive(false);
        dumpBtn.gameObject.SetActive(false);
        sellBtn.gameObject.SetActive(true);

        SetSellBtn(sellCallback);   
        this.gameObject.SetActive(true);
    }

    private void SetOkBtn(Action action) => OkBtnEvent = action;
    private void SetOkBtn2(Action action) => OkBtnEvent2 = action;
    private void SetDumpBtn(Action action) => DumpBtnEvent = action;
    private void SetSellBtn(Action action) => SellBtnEvent = action;

}
