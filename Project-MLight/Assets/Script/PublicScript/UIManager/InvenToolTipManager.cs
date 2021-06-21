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
    private Button okBtn; //확인버튼
    [SerializeField]
    private Button dumpBtn; //버리기 버튼

    //확인버튼 누를시 동작
    private event Action OkBtnEvent;
    private event Action DumpBtnEvent;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        okBtn.onClick.AddListener(() => OkBtnEvent());
        dumpBtn.onClick.AddListener(() => DumpBtnEvent());
    }

    //아이템 설정
    public void SetItemInfo(ItemData data, Action okCallback, Action dumpCallback, int amount)
    {
        nameTxt.text = data.Name;
        toolTipTxt.text = data.Tooltip;
        ItemImg.sprite = data.IconSprite;
        countTxt.text = amount.ToString();

        //버튼 이벤트 설정
        SetOkBtn(okCallback); 
        SetDumpBtn(dumpCallback);
    }

    private void SetOkBtn(Action action) => OkBtnEvent = action;
    private void SetDumpBtn(Action action) => DumpBtnEvent = action;
   
}
