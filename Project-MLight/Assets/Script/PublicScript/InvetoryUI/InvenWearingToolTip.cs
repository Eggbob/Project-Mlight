using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenWearingToolTip : MonoBehaviour
{
    [SerializeField]
    private Text nameTxt; //아이템 이름
    [SerializeField]
    private Image ItemImg; //아이템 이미지
    [SerializeField]
    private Text statTxt; //아이템 스탯 텍스트
    [SerializeField]
    private Text toolTipTxt; //아이템 설명
    [SerializeField]
    private Text priceTxt; //아이템 가치 텍스트
    [SerializeField]
    private Text weightTxt; //무개 텍스트
    [SerializeField]
    private Button unEquipBtn; //장비 해제 버튼
    [SerializeField]
    private Button dumpBtn; //버리기 버튼

    //버튼 동작
    private event Action UnEquipEvent;
    private event Action DumpBtnEvent;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        unEquipBtn.onClick.AddListener(() => UnEquipEvent());
        dumpBtn.onClick.AddListener(() => DumpBtnEvent());
    }


}
