using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private ShopBtnManager[] shopBtns; //아이템 슬롯

    [SerializeField]
    private Text pageNumber;

    private List<List<SalesManItem>> pages = new List<List<SalesManItem>>();

    private int pageIndex;

    private SalesMan sMan;

    public void CreatePages(SalesManItem[] items)
    {
        pages.Clear();

        List<SalesManItem> page = new List<SalesManItem>();

        for(int i = 0; i< items.Length; i++)
        {
            page.Add(items[i]);

            if(page.Count == 10 || i == items.Length - 1)
            {
                pages.Add(page);
                page = new List<SalesManItem>();
            }
        }

        AddItems();
    }

    public void AddItems()
    {
        pageNumber.text = pageIndex + 1 + "/" + pages.Count;

        //for(int i = 0; i< items.Length; i++)
        //{
        //    shopBtns[i].AddItem(items[i]);
        //}

        if(pages.Count > 0)
        {
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if(pages[pageIndex][i] != null)
                {
                    shopBtns[i].AddItem(pages[pageIndex][i]);
                  
                }
            }
        }
    }

    public void Open(SalesMan _sMan)
    {
        this.sMan = _sMan;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }


    public void Close()
    {
        sMan.IsOpen = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        sMan = null;
    }

    public void NextPage()
    {
        if(pageIndex < pages.Count - 1)
        {
            ClearButton();
            pageIndex++;
            AddItems();
        }
    }

    public void PreviousPage()
    {
        if(pageIndex > 0)
        {
            ClearButton();
            pageIndex--;
            AddItems();
        }
    }

    public void ClearButton()
    {
        foreach(ShopBtnManager btn in shopBtns)
        {
            btn.gameObject.SetActive(false);
        }
    }
}
