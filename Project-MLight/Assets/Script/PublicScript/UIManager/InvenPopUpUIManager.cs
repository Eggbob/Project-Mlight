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

    private Action 

}
