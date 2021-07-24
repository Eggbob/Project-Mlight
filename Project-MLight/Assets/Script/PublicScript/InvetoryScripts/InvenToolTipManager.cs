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
    private Button dumpBtn; //버리기 버튼

    //확인버튼 누를시 동작
    private event Action OkBtnEvent;
    private event Action OkBtnEvent2;
    private event Action DumpBtnEvent;
    

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        okBtn.onClick.AddListener(() => OkBtnEvent());
        okBtn.onClick.AddListener(() => OkBtnEvent2());
        dumpBtn.onClick.AddListener(() => DumpBtnEvent());    
    }

    //아이템 설정
    public void SetItemInfo(ItemData data, Action okCallback1, Action okCallback2, Action dumpCallback, int amount)
    {
        nameTxt.text = data.Name;
        toolTipTxt.text = data.Tooltip;
        ItemImg.sprite = data.IconSprite;
        countTxt.text = amount.ToString();
        priceTxt.text = data.ItemSellPrice.ToString() + "G";
        okBtn.gameObject.SetActive(true);

        //버튼 이벤트 설정
        SetOkBtn(okCallback1);
        SetOkBtn2(okCallback2);
        SetDumpBtn(dumpCallback);

        this.gameObject.SetActive(true);
    }

    //잡다한 아이템 설정
    public void SetPropItemInfo(ItemData data, Action dumpCallback, int amount)
    {
        nameTxt.text = data.Name;
        toolTipTxt.text = data.Tooltip;
        ItemImg.sprite = data.IconSprite;
        countTxt.text = amount.ToString();
        priceTxt.text = data.ItemSellPrice.ToString() + "G";
        okBtn.gameObject.SetActive(false);

        SetDumpBtn(dumpCallback);

        this.gameObject.SetActive(true);
    }

    private void SetOkBtn(Action action) => OkBtnEvent = action;
    private void SetOkBtn2(Action action) => OkBtnEvent2 = action;
    private void SetDumpBtn(Action action) => DumpBtnEvent = action;

   
}
