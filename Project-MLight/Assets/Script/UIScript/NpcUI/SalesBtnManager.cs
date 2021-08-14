using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalesBtnManager : MonoBehaviour
{
    [Tooltip("아이템 아이콘 이미지")]
    [SerializeField] private Image iconImg;

    [Tooltip("하이라이트 이미지")]
    [SerializeField] private GameObject highlightImg;

    public int Index { get; private set; } //슬롯 인덱스
    public bool HasItem => iconImg.sprite != null; //슬롯이 아이템을 보유하고 있는지 유무

    public void SetSlotIndex(int index) => Index = index; // 슬롯 인덱스 설정

    public void SetIcon(Sprite itemSprite)
    {
        if(itemSprite != null)
        {
            iconImg.sprite = itemSprite;
            iconImg.gameObject.SetActive(true);
        }
        else
        {
            iconImg.sprite = null;
            iconImg.gameObject.SetActive(false);
        }
    }

    //하이라이트 이미지 표시
    public void ShowHighLight(bool show)
    {
        if (!HasItem) return;

        if (show)
            highlightImg.SetActive(true);
        else
            highlightImg.SetActive(false);
    }
}
