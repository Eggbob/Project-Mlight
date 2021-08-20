using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopBtnManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text price;
    [SerializeField]
    private Text quantity;

    [SerializeField]
    private SalesManItem sItem;

    public void AddItem(SalesManItem _sItem)
    {
        this.sItem = _sItem;

        if(_sItem.Quantity > 0 || (_sItem.Quantity == 0 && _sItem.Unlimited))
        {
            icon.sprite = _sItem.Item.IconSprite;
            title.text = _sItem.Item.ItemName;
         

            if (!_sItem.Unlimited)
            {
                quantity.text = _sItem.Quantity.ToString();
            }
            else
            {
                quantity.text = string.Empty;
            }

            if (_sItem.Item.ItemPrice > 0)
            {
                price.text = _sItem.Item.ItemPrice.ToString();
            }
            else
            {
                price.text = string.Empty;
            }
            gameObject.SetActive(true);
        }
       

       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if(PlayerController.instance.Inven.Add(sItem.Item))
        //{
        SellItem();
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       //툴팁보여주기
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //툴팁끄기
    }

    public void SellItem()
    {
        GameManager.Instance.Inven.UseGold(sItem.Item.ItemPrice);

        if(!sItem.Unlimited)
        {
            sItem.Quantity--;
            quantity.text = sItem.Quantity.ToString();

            if (sItem.Quantity == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
