using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InvenPopUpUIManager : MonoBehaviour
{
    [Header("확인 팝업")]
    [SerializeField] private GameObject confirmUI; //확인창
    [SerializeField] private Button confirmBtn;//확인 버튼
    [SerializeField] private Button cancelBtn; //취소 버튼

    [Header("수량 팝업")]
    [SerializeField] private GameObject popUpUI; //수량 팝업창
    [SerializeField] private Text  countTxt; //수량 텍스트
    [SerializeField] private Slider countSlider; //수량 슬라이더
    [SerializeField] private Button plusBtn; //플러스 버튼
    [SerializeField] private Button minusBtn; //마이너스 버튼
    [SerializeField] private Button okBtn; //확인 버튼
    [SerializeField] private Button pCancelBtn; //취소 버튼
    [SerializeField] private Image goldImg; //골드 이미지
    [SerializeField] private Text goldTxt; //골드 텍스트

    private Action<int> OkBtnEvent;//수량 팝업 이벤트
    private Action<int> SellBtnEvent;//판매 이벤트

    private int maxCount; //최대 버릴수 있는 개수
    private int preCount; //현재 개수

    private int itemPrice; //아이템 가격

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        //확인 팝업
        confirmBtn.onClick.AddListener(()=> confirmUI.SetActive(false));
        cancelBtn.onClick.AddListener(() => confirmUI.SetActive(false));

        //수량 팝업
        okBtn.onClick.AddListener(() => popUpUI.SetActive(false));
        okBtn.onClick.AddListener(() => OkBtnEvent(int.Parse(countTxt.text)));
        okBtn.onClick.AddListener(() =>
        { 
            if(goldTxt.gameObject.activeSelf)
            {
                SellBtnEvent(itemPrice * preCount);
            }
        });

        pCancelBtn.onClick.AddListener(()=> popUpUI.SetActive(false));

        //마이너스 버튼 이벤트
        minusBtn.onClick.AddListener(() =>
        {
            if(preCount > 0)
            {
                int nextCount = preCount - 1;

                if (nextCount <= 0) { preCount = 0; }
                else { preCount = nextCount; }  

                countTxt.text = preCount.ToString();
                countSlider.value = (float)preCount / (float)maxCount;
            }   
        });

        minusBtn.onClick.AddListener(() => SetItemPrice(0));

        //플러스 버튼 이벤트
        plusBtn.onClick.AddListener(() =>
        {
            if (preCount < maxCount)
            {
                int nextCount = preCount + 1;

                if (nextCount > maxCount) { preCount = maxCount; }
                else { preCount = nextCount; }

                countTxt.text = preCount.ToString();
                countSlider.value = (float)preCount / (float)maxCount;
            }
        });

        plusBtn.onClick.AddListener(() => SetItemPrice(0));

        //카운트 슬라이더 이벤트
        countSlider.onValueChanged.AddListener(CountUpdate);
        countSlider.onValueChanged.AddListener(SetItemPrice);

        
        //창 비활성화
        confirmUI.SetActive(false);
        popUpUI.SetActive(false);
    }

    //카운트 슬라이더 이벤트 메소드
    private void CountUpdate(float value)
    {
        preCount = Mathf.RoundToInt(value * maxCount);
        countTxt.text = preCount.ToString();
    }

    //아이템 가격 설정
    private void SetItemPrice(float dummyVal)
    {
        if(goldTxt.gameObject.activeSelf)
        {
            goldTxt.text = (itemPrice * preCount).ToString(); 
        }
    }
    
    private void SetOkBtnEvent(Action<int> action) => OkBtnEvent = action;
    private void SetSellBtnEvent(Action<int> action) => SellBtnEvent = action;

    //확인창 보여주기
    public void ShowConfirmUI(Action<int> okCallback, int currentAmount)
    {
        confirmUI.SetActive(true);    
        maxCount = currentAmount;
        preCount = 1;

        countTxt.text = preCount.ToString();
        countSlider.value = (float)preCount / (float)maxCount;

        goldImg.gameObject.SetActive(false);
        goldTxt.gameObject.SetActive(false);

        confirmBtn.onClick.AddListener(()=>popUpUI.SetActive(true));
       
        SetOkBtnEvent(okCallback);
    }

    //팝업창 보여주기
    public void ShowPopUpUI(Action<int> okCallback, Action<int> getGoldCallback ,int currentAmount, int price)
    {
        maxCount = currentAmount;
        itemPrice = price;
        preCount = 1;

        countTxt.text = preCount.ToString();
        countSlider.value = (float)preCount / (float)maxCount;

        goldTxt.text = itemPrice.ToString();

        goldImg.gameObject.SetActive(true);
        goldTxt.gameObject.SetActive(true);

        SetOkBtnEvent(okCallback);

        SetSellBtnEvent(getGoldCallback);   

        popUpUI.gameObject.SetActive(true);
    }
}
