using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenEquipToolTipManager : MonoBehaviour
{
    [SerializeField]
    private Text nameTxt; //아이템 이름
    [SerializeField]
    private Image ItemImg; //아이템 이미지
    [SerializeField]
    private GameObject equipImg; //착용중인지 이미지
    [SerializeField]
    private Text statTxt; //아이템 스탯 텍스트
    [SerializeField]
    private Text toolTipTxt; //아이템 설명
    [SerializeField]
    private Text priceTxt; //아이템 가치 텍스트
    [SerializeField]
    private Text weightTxt; //무게 텍스트
    [SerializeField]  
    private Button okBtn; //확인버튼
    [SerializeField]
    private Button unEquipBtn; //장비 착용 해제 버튼
    [SerializeField]
    private Button dumpBtn; //버리기 버튼

    //확인버튼 누를시 동작
    private event Action OkBtnEvent;
    private event Action DumpBtnEvent;
    private event Action<Item> UnEquipEvent;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {

        equipImg.SetActive(false);
        okBtn.onClick.AddListener(() => OkBtnEvent());
        dumpBtn.onClick.AddListener(() => DumpBtnEvent());
       
    }



    //아이템 설정
    public void SetItemInfo(ItemData data, Action okCallback, Action dumpCallback)
    {
        nameTxt.text = data.Name;
        toolTipTxt.text = data.Tooltip;
        ItemImg.sprite = data.IconSprite;

        okBtn.gameObject.SetActive(true);
        dumpBtn.gameObject.SetActive(true);
        unEquipBtn.gameObject.SetActive(false);
        equipImg.SetActive(false);

        if (data is WeaponItemData wdata)
        {
            statTxt.text = "공격력 : " + wdata.Damage;
            weightTxt.text = wdata.Weight.ToString();
        }
        else if(data is AmorItemData adata)
        {
            statTxt.text = "방어력 : " + adata.Defence;
            weightTxt.text = adata.Weight.ToString();
        }

        //버튼 이벤트 설정
        SetOkBtn(okCallback);
        SetDumpBtn(dumpCallback);
       

        this.gameObject.SetActive(true);
    }


    //장비중인 아이템 설정
    public void SetEquipItemInfo(Item item, Action<Item> unEquipCallback)
    {
        nameTxt.text = item.Data.Name;
        toolTipTxt.text = item.Data.Tooltip;
        ItemImg.sprite = item.Data.IconSprite;

        okBtn.gameObject.SetActive(false);
        dumpBtn.gameObject.SetActive(false);
        unEquipBtn.gameObject.SetActive(true);
        equipImg.SetActive(true);

        if (item.Data is WeaponItemData wdata)
        {
            statTxt.text = "공격력 : " + wdata.Damage;
            weightTxt.text = wdata.Weight.ToString();
        }
        else if (item.Data is AmorItemData adata)
        {
            statTxt.text = "방어력 : " + adata.Defence;
            weightTxt.text = adata.Weight.ToString();
        }

        //버튼 이벤트 설정
        unEquipBtn.onClick.AddListener(()=>UnEquipEvent(item));
        SetUnEquip(unEquipCallback);
        this.gameObject.SetActive(true);
    }

    private void SetOkBtn(Action action) => OkBtnEvent = action;
    private void SetDumpBtn(Action action) => DumpBtnEvent = action;
    private void SetUnEquip(Action<Item> action) => UnEquipEvent = action;
}
