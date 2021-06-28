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

    private Action<int> OkBtnEvent;//수량 팝업 이벤트

    private int maxCount; //최대 버릴수 있는 개수

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
        pCancelBtn.onClick.AddListener(()=> popUpUI.SetActive(false));

        //마이너스 버튼 이벤트
        minusBtn.onClick.AddListener(() =>
        {
            int.TryParse(countTxt.text, out int count);
            if(count > 1)
            {
                int nextCount = count - 1;
                if (nextCount < 1)
                    nextCount = 1;

                countTxt.text = nextCount.ToString();
            }   
        });

        //플러스 버튼 이벤트
        plusBtn.onClick.AddListener(() =>
        {
            int.TryParse(countTxt.text, out int count);
            if (count < maxCount)
            {
                int nextCount = count + 1;
                if (nextCount > maxCount)
                    nextCount = maxCount;
                countTxt.text = nextCount.ToString();
            }
        });

        //카운트 슬라이더 이벤트
        countSlider.onValueChanged.AddListener(CountUpdate);

        //창 비활성화
        confirmUI.SetActive(false);
        popUpUI.SetActive(false);
    }

    //카운트 슬라이더 이벤트 메소드
    private void CountUpdate(float value)
    {
        countTxt.text = Mathf.RoundToInt(value * maxCount).ToString();
    }

    
    private void SetOkBtnEvent(Action<int> action) => OkBtnEvent = action;

    //확인창 보여주기
    public void ShowConfirmUI(Action<int> okCallback, int currentAmount)
    {
        confirmUI.SetActive(true);

        maxCount = currentAmount;
        countTxt.text = "1";

        confirmBtn.onClick.AddListener(()=>popUpUI.SetActive(true));
       
        SetOkBtnEvent(okCallback);
    }

    
}
