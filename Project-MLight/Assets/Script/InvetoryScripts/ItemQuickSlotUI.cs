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
    [Tooltip("인벤 아이템 아이콘 이미지")]
    [SerializeField] private Image itemImg;

    [Tooltip("인벤 아이템 해제 버튼")]
    [SerializeField] private Button removeBtn;

    [Tooltip("인벤 아이템 수량 이미지")]
    [SerializeField] private Image amountImg;

    [Tooltip("인벤 아이템 수량 텍스트")]
    [SerializeField] private Text amountTxt;

    [Tooltip("인벤 아이템 버튼")]
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

    private GameObject ItemImg => itemImg.gameObject;
    private GameObject AmountImg => amountImg.gameObject;
    private GameObject AmountTxt => amountTxt.gameObject;
    private GameObject RemoveBtn => removeBtn.gameObject;

    private GameObject QuickImg => quickImg.gameObject;
    private GameObject QuickAmountTxt => quickAmountTxt.gameObject;
    private GameObject QuickAmountImg => quickAmountImg.gameObject;

    private CoolDown coolDown;
    private float coolTime;

    //아이템 사용 액션
    private event Action ItemUse;
    private event Action SetSlot;
    private event Func<int> UpdateAmount;
    #endregion

    public int Index { get; private set; } //슬롯 인덱스

    private ItemData slotItem;

    private void ShowImg() => ItemImg.SetActive(true);
    private void HideImg() => ItemImg.SetActive(false);

    private void ShowAmount()
    {  AmountImg.SetActive(true); AmountTxt.SetActive(true); }
    private void HideAmount()
    { AmountImg.SetActive(false); AmountTxt.SetActive(false); }

    private void ShowRemove() => RemoveBtn.SetActive(true);
    private void HideRemove() => RemoveBtn.SetActive(false);

    private void ShowQImg() => QuickImg.SetActive(true);
    private void HideQImg() => QuickImg.SetActive(false);

    private void ShowQAmount() { QuickAmountImg.SetActive(true); QuickAmountTxt.SetActive(true); }
    private void HideQAmount() { QuickAmountImg.SetActive(false); QuickAmountTxt.SetActive(false); }


    public void SetSlotIndex(int index) => Index = index; // 슬롯 인덱스 설정
    public void SetSlotEvent(Action action) => SetSlot = action;

    public bool HasItem => itemImg.sprite != null; //슬롯이 아이템을 보유중인지

    public ItemData SlotItem => slotItem; //슬롯이 보유중이 안이템

    private void Awake()
    {
        Init();
    }

    private void Init()
    {        
        itemBtn.onClick.AddListener(() => SetSlot());
        quickBtn.onClick.AddListener(()=> UseItem());
        removeBtn.onClick.AddListener(() => RemoveItem());

        coolDown = quickBtn.gameObject.GetComponent<CoolDown>();
    }

    //아이템 사용
    private void UseItem()
    {
        if (!HasItem)
            return;

        UpdateItemAmount();
        ItemUse();
        coolDown.UseSpell(coolTime);
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
    public void SetItem(PotionItemData item, int amount, Action action, Func<int> action2)
    { 
       if(item != null)
       {
            itemImg.sprite = item.IconSprite;
            amountTxt.text = amount.ToString();
            quickImg.sprite = item.IconSprite;
            quickAmountTxt.text = amount.ToString();
            coolTime = item.CoolTime;

            ShowImg();
            ShowAmount();
            ShowRemove();

            ShowQAmount();
            ShowQImg();

            SetUseEvent(action);
            SetAmountEvent(action2);

            slotItem = item;
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
