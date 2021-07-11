using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipManager : MonoBehaviour
{
    [Tooltip("장비 아이템 버튼")]
    [SerializeField] private Button weaponBtn; //무기슬롯 
    [SerializeField] private Button armorBtn; //방어구 슬롯

    [Tooltip("장비 아이템 아이콘 이미지")]
    [SerializeField] private Image weaponImg; //무기이미지
    [SerializeField] private Image armorImg; //방어구 이미지
    

    [SerializeField]
    private InvenEquipToolTipManager eManger; //장비 툴팁
  

    public Sprite weaponNormalImg; //무기 기본이미지
    public Sprite armorNormalImg; //방어구 기본이미지

    public bool hasWeapon => wItem != null; // 착용한 무기가 있는지
    public bool hasArmor => aItem != null; // 착용한 방어구가 있는지

    public WeaponItem WITEM => wItem; //현재 착용중인 무기 참조
    public ArmorItem AITEM => aItem; //현재 착용중인 방어구 참조

    private WeaponItem wItem; // 착용중인 무기
    private ArmorItem aItem; // 착용중인 방어구

    private Action<Item> unEquipEvent;
    private Action armorReturn;
    private Action weaponReturn;
    

    //기본 장비 이미지 알파값
    private static readonly Color normalAlpha = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    void Start()
    {
        Init();
    }

    //초기 설정
    private void Init()
    {
        weaponImg.sprite = weaponNormalImg;
        weaponImg.color = normalAlpha;
        armorImg.sprite = armorNormalImg;
        armorImg.color = normalAlpha;
        wItem = null;
        aItem = null;
        unEquipEvent = UnEquip; //장비 해제 이벤트 등록

        //온클릭 이벤트 등록
        weaponBtn.onClick.AddListener(()=>{
            if (!hasWeapon)
                return;
            else                          
                eManger.SetEquipItemInfo(wItem, unEquipEvent);                             
        });
        

        armorBtn.onClick.AddListener(() =>
        {
            if (!hasArmor)
                return;
            else
                eManger.SetEquipItemInfo(aItem, unEquipEvent);
        });
    }



    //장비해제 메소드
    private void UnEquip(Item item)
    {
        //아이템이 방어구일시
        if(item is ArmorItem)
        {
            armorImg.sprite = armorNormalImg;
            armorImg.color = normalAlpha;
            aItem = null;

            armorReturn(); //리턴 이벤트 실행
        }
        //아이템이 무기일시
        else if(item is WeaponItem)
        {
            weaponImg.sprite = weaponNormalImg;
            weaponImg.color = normalAlpha;
            wItem = null;

            weaponReturn();
        }

    }

    //무기 착용
    public void SetWeapon(WeaponItem _wItem, Action returnCallback)
    {
        weaponImg.sprite = _wItem.Data.IconSprite;
        weaponImg.color = Color.white;
        wItem = _wItem;
        SetWeaponReturn(returnCallback);//콜백함수 등록
    }

    //방어구 착용
    public void SetArmor(ArmorItem _aItem, Action returnCallback)
    {
        armorImg.sprite = _aItem.Data.IconSprite;
        armorImg.color = Color.white;
        aItem = _aItem;
        SetArmorReturn(returnCallback);
    }

    private void SetArmorReturn(Action action) => armorReturn = action;
    private void SetWeaponReturn(Action action) => weaponReturn = action;
  
}
