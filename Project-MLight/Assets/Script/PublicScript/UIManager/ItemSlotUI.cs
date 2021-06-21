using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [Tooltip("아이템 아이콘 이미지")]
    [SerializeField] private Image iconImg;

    [Tooltip("아이템 개수 이미지")]
    [SerializeField] private GameObject amountImg;

    [Tooltip("아이템 개수 텍스트")]
    [SerializeField] private Text amountTxt;

    [Tooltip("하이라이트 이미지")]
    [SerializeField] private GameObject highlightImg;

    
    public int Index { get; private set; } //슬롯 인덱스

    public bool HasItem => iconImg.sprite != null; //슬롯이 아이템을 보유하고 있는지 유무

    public bool IsAccesible => isAccessibleSlot && isAccessibleItem; //접근 가능한 슬롯인지 여부

    private InvenUIManager inventroyUI;

    private GameObject _iconGo;
    private GameObject _textImgGo;
    private GameObject _textGo;
    private GameObject _highlightGo;

    private Image slotImg; //슬롯이미지

    private bool isAccessibleSlot = true; // 슬롯 접근가능 여부
    private bool isAccessibleItem = true; // 아이템 접근 가능 여부


    private void ShowIcon() => _iconGo.SetActive(true);//아이콘 활성화
    private void HideIcon() => _iconGo.SetActive(false);//아이콘 비활성화

    private void ShowText() => _textGo.SetActive(true); //텍스트 활성화
    private void HideText() => _textGo.SetActive(false); // 텍스트 비활성화

    private void ShowImg() => _textImgGo.SetActive(true);//텍스트 이미지 활성화
    private void HideImg() => _textImgGo.SetActive(false); // 텍스트 이미지 비활성화

    public void SetSlotIndex(int index) => Index = index; // 슬롯 인덱스 설정


    private void Awake()
    {
        InitComponents();
    }

    private void InitComponents() //초기설정
    {
        inventroyUI = GetComponentInParent<InvenUIManager>();

        // Game Objects
        _iconGo = iconImg.gameObject;
        _textGo = amountTxt.gameObject;
        _highlightGo = highlightImg;
        _textImgGo = amountImg;

        // Images
        slotImg = GetComponent<Image>();
    }



    /// <summary> 슬롯 자체의 활성화/비활성화 여부 설정 </summary>
    public void SetSlotAccessibleState(bool value)
    {
        // 중복 처리는 지양
        if (isAccessibleSlot == value) return;

        if (!value)//슬롯에 접근이  불가능하다면
        {              
            HideIcon(); //아이콘 비활성화
            HideText(); //텍스트 비활성화
            HideImg(); //이미지 비활성화
        }

        isAccessibleSlot = value; //슬롯 접근 여부 설정
    }

    /// <summary> 아이템 활성화/비활성화 여부 설정 </summary>
    public void SetItemAccessibleState(bool value)
    {     
        // 중복 처리는 지양
        if (isAccessibleItem == value) return;

        if (value)//아이템이 활성화 되어있다면
        {
            iconImg.color = Color.white; //아이콘 이미지 색상 변경
            amountTxt.color = Color.white;//텍스트 색상 변경
        }
        //else //비활성화라면
        //{
        //    iconImg.color = InaccessibleIconColor; //비활성화 색상으로변경
        //    amountTxt.color = InaccessibleIconColor;
        //}

        isAccessibleItem = value; //아이템 접근 여부 설정
    }

    /// <summary> 다른 슬롯과 아이템 아이콘 교환 </summary>
    public void SwapOrMoveIcon(ItemSlotUI other)
    {
        if (other == null) return; //타 아이템이 null일시
        if (other == this) return; // 자기 자신과 교환 불가
        if (!this.IsAccesible) return; //이 슬롯이 비활성화 상태라면
        if (!other.IsAccesible) return; //타 슬롯이 비활성화 상태라면

        var temp = iconImg.sprite; //아이콘 스프라이트 할당

        // 1. 대상에 아이템이 있는 경우 : 교환
        if (other.HasItem) SetItem(other.iconImg.sprite);

        // 2. 없는 경우 : 이동
        else RemoveItem();

        other.SetItem(temp);//타 슬롯 아이템 변경
    }

    /// <summary> 슬롯에 아이템 등록 </summary>
    public void SetItem(Sprite itemSprite)
    {
        if (itemSprite != null)//스프라이트가 null이 아닐시
        {
            iconImg.sprite = itemSprite; //스프라이트 변경
            ShowIcon(); //아이콘 활성화
        }
        else
        {
            RemoveItem();//아이템 삭제
        }
    }

    /// <summary> 슬롯에서 아이템 제거 </summary>
    public void RemoveItem()
    {
        iconImg.sprite = null;
        HideIcon();
        HideText();
        HideImg();
    }

    /// <summary> 아이템 개수 텍스트 설정(amount가 1 이하일 경우 텍스트 미표시) </summary>
    public void SetItemAmount(int amount)
    {
        if (HasItem && amount > 1)
        {
            ShowText();
            ShowImg();
        }
        else
        {
            HideText();
            HideImg();
        }
          
        amountTxt.text = amount.ToString();
    }

    //하이라이트 이미지 표시
    public void Highlight(bool show) 
    {
        if (!this.IsAccesible) return;

        if (show)
            highlightImg.SetActive(true);
        else
            highlightImg.SetActive(false);
    }
}
