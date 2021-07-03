using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemQuickSlotUI : MonoBehaviour
{
    //변수 선언
    #region .
    [Space]
    [Tooltip("아이템 아이콘 이미지")]
    [SerializeField] private Image itemImg;

    [Tooltip("아이템 해제 버튼")]
    [SerializeField] private Button removeBtn;

    [Tooltip("아이템 수량 이미지")]
    [SerializeField] private Image amountImg;

    [Tooltip("아이템 수량 텍스트")]
    [SerializeField] private Text amountTxt;

    [Tooltip("아이템 버튼")]
    [SerializeField] private Button itemBtn;


    [Space]
    [Tooltip("퀵슬롯 이미지")]
    [SerializeField] private Image quickImg;

    [Tooltip("퀵슬롯 수량 이미지")]
    [SerializeField] private Image quickAmountImg;
    
    [Tooltip("퀵슬롯 수량 텍스트")]
    [SerializeField] private Text quickAmountTxt;

    [Tooltip("퀵슬롯 버튼")]
    [SerializeField] private Button quickBtn;

    private GameObject imgGo;
    private GameObject amountImgGo;
    private GameObject amountTxtGo;
    private GameObject removeBtnGo;

    private GameObject qImgGo;
    private GameObject qAmountTxtGo;
    private GameObject qAmountImgGo;

    //아이템 사용 액션
    private event Action ItemUse;
    private event Action SetSlot;
    private event Func<int> UpdateAmount;
    #endregion

  
    public int Index { get; private set; } //슬롯 인덱스

    

    private void ShowImg() => imgGo.SetActive(true);
    private void HideImg() => imgGo.SetActive(false);

    private void ShowAmount()
    {  amountImgGo.SetActive(true); amountTxtGo.SetActive(true); }
    private void HideAmount()
    { amountImgGo.SetActive(false); amountTxtGo.SetActive(false); }

    private void ShowRemove() => removeBtnGo.SetActive(true);
    private void HideRemove() => removeBtnGo.SetActive(false);

    private void ShowQImg() => qImgGo.SetActive(true);
    private void HideQImg() => qImgGo.SetActive(false);

    private void ShowQAmount() { qAmountImgGo.SetActive(true); qAmountTxtGo.SetActive(true); }
    private void HideQAmount() { qAmountImgGo.SetActive(false); qAmountTxtGo.SetActive(false); }


    public void SetSlotIndex(int index) => Index = index; // 슬롯 인덱스 설정
    public void SetSlotEvent(Action action) => SetSlot = action;

    public bool HasItem => itemImg.sprite != null; //슬롯이 아이템을 보유중인지


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        imgGo = itemImg.gameObject;
        amountImgGo = amountImg.gameObject;
        amountTxtGo = amountTxt.gameObject;
        removeBtnGo = removeBtn.gameObject;

        qImgGo = quickImg.gameObject;
        qAmountImgGo = quickAmountImg.gameObject;
        qAmountTxtGo = quickAmountTxt.gameObject;

        itemBtn.onClick.AddListener(() => SetSlot());
        quickBtn.onClick.AddListener(()=> UseItem());
        removeBtn.onClick.AddListener(() => RemoveItem());


    }

    //아이템 사용
    private void UseItem()
    {
        UpdateItemAmount();
        ItemUse();
    }


    //아이템 제거
    private void RemoveItem()
    {
        SetUseEvent(null);

        HideImg();
        HideAmount();
        HideRemove();
        HideQImg();
        HideQAmount();
    }

    //아이템 등록
    public void SetItem(ItemData item, int amount, Action action, Func<int> action2)
    { 
       if(item != null)
       {
            itemImg.sprite = item.IconSprite;
            amountTxt.text = amount.ToString();
            quickImg.sprite = item.IconSprite;
            quickAmountTxt.text = amount.ToString();

            ShowImg();
            ShowAmount();
            ShowRemove();

            ShowQAmount();
            ShowQImg();

            SetUseEvent(action);
            SetAmountEvent(action2);
       }
    }

    //아이템 수량 업데이트
    public void UpdateItemAmount()
    {
        int amount = UpdateAmount();
        amountTxt.text = amount.ToString();
        quickAmountTxt.text = amount.ToString();
    }

    private void SetUseEvent(Action action) => ItemUse = action;
    private void SetAmountEvent(Func<int> action) => UpdateAmount = action;
}
