using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerchaseToolTipManager : MonoBehaviour
{
    #region
    [Header("아이템 정보 관련")]
    [SerializeField]
    private Text nameTxt; //아이템 이름
    [SerializeField]
    private Text toolTipTxt; //아이템 설명
    [SerializeField]
    private Image ItemImg; //아이템 이미지
    [SerializeField]
    private Text weightTxt; //아이템 무게 텍스트
    [SerializeField]
    private Image weightImg; //아이템 무게 이미지

    [Header("구매관련")]
    [SerializeField]
    private Button minusBtn;
    [SerializeField]
    private Button minusTenBtn;

    [SerializeField]
    private Button plusBtn;
    [SerializeField]
    private Button plusTenBtn;

    [SerializeField]
    private Text countTxt; //수량 텍스트
    [SerializeField]
    private Text priceTxt; //아이템 가격
    [SerializeField]
    private Button perChaseBtn; //구매버튼
    [SerializeField]
    private Button cancelBtn; //취소버튼
    [SerializeField]
    private Text notEnoughTxt; //골드 부족 텍스트

    private event Action<ItemData, int> PerchaseItemEvent;
    private event Func<int,bool> PayGoldEvent;

    private ItemData iData;
    private int pCount; //구매수량
    private int itemPrice; //아이템 가격
    private int itemTotalPrice;

    #endregion
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        perChaseBtn.onClick.AddListener(() =>
        {
          
            if(PayGoldEvent(itemTotalPrice))
            {
                PerchaseItemEvent(iData, pCount);
                this.gameObject.SetActive(false);
            }  
            else
            {
                StartCoroutine(ShowNotEnoughGold());
            }
        });

        minusBtn.onClick.AddListener(() =>
        {
            if(pCount - 1 > 0)
            {
                pCount--;
                countTxt.text = pCount.ToString();
                SetPrice();
            }

        });

        minusTenBtn.onClick.AddListener(()=>
        {
            if (pCount - 10 > 0)
            {
                pCount -= 10;
                countTxt.text = pCount.ToString();
                SetPrice();
            }

        });

        plusBtn.onClick.AddListener(() =>
        {
            if(pCount+1 <= 100)
            {
                pCount++;
                countTxt.text = pCount.ToString();
                SetPrice();
            }

        });

        plusTenBtn.onClick.AddListener(() =>
        {
            if (pCount + 10 <= 100)
            {
                pCount+= 10;
                countTxt.text = pCount.ToString();
                SetPrice();
            }

        });

        notEnoughTxt.gameObject.SetActive(false);
       
    }

    //가격 설정
    private void SetPrice()
    {
        itemTotalPrice = itemPrice * pCount;
        priceTxt.text = itemTotalPrice.ToString();
    }

    //골드 부족 텍스트
    private IEnumerator ShowNotEnoughGold()
    {
        notEnoughTxt.gameObject.SetActive(true);

        notEnoughTxt.color =
            new Color(notEnoughTxt.color.r, notEnoughTxt.color.g, notEnoughTxt.color.b, 1);

        while (notEnoughTxt.color.a > 0f)
        {
            notEnoughTxt.color =
                new Color(notEnoughTxt.color.r, notEnoughTxt.color.g, notEnoughTxt.color.b, notEnoughTxt.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
       

        notEnoughTxt.gameObject.SetActive(false);

    }

    //아이템 정보 등록
    public void SetItemInfo(ItemData data, Action<ItemData, int> pCallBack, Func<int, bool> payCallBack)
    {
        pCount = 1;

        nameTxt.text = data.ItemName;
        toolTipTxt.text = data.Tooltip;
        ItemImg.sprite = data.IconSprite;
        countTxt.text = pCount.ToString();

        if(data is EquipItemData eData)
        {
            weightImg.gameObject.SetActive(true);
            weightTxt.text = eData.Weight.ToString();
        }
        else if(data is CountableItemData cData)
        {
            weightTxt.gameObject.SetActive(false);
            weightImg.gameObject.SetActive(false);
        }

        itemPrice = data.ItemPrice;
        itemTotalPrice = itemPrice;
        priceTxt.text = itemTotalPrice.ToString();

        iData = data;

        SetPerchase(pCallBack);
        SetPayEvent(payCallBack);

        this.transform.SetAsLastSibling();
        this.gameObject.SetActive(true);
    }


    private void SetPerchase(Action<ItemData, int> action) => PerchaseItemEvent = action;
    private void SetPayEvent(Func<int, bool> action) => PayGoldEvent = action;


}
