using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SalesWindowManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform contentsArea;
    [SerializeField]
    private PerchaseToolTipManager pToolTip;
    [SerializeField]
    private SalesBtnManager[] salesBtnList;
    [SerializeField]
    private List<ItemData> salesItems = new List<ItemData>();

    private GraphicRaycaster gr;
    private PointerEventData ped;//마우스 입력감지
    private List<RaycastResult> rrList;

    private SalesBtnManager prevClickSlot; // 이전 클릭한 슬롯
    private SalesBtnManager beginClickSlot; //현재 클릭한  슬롯

    private PlayerController pCon;
    private Inventory inven;

    private void Awake()
    {
        Init();  
    }

    private void OnEnable()
    {
        this.transform.SetSiblingIndex(2);
        pCon = GameManager.Instance.Player;
        inven = GameManager.Instance.Inven;
    }

    private void Update()
    {
        ped.position = Input.mousePosition;
        OnPointerDown();
    }

    //초기설정
    private void Init()
    {  
        TryGetComponent(out gr); //그래픽 레이캐스터 가져오기
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);

        salesBtnList = contentsArea.GetComponentsInChildren<SalesBtnManager>();

        for (int i = 0; i < salesBtnList.Length; i++)
        {
            salesBtnList[i].name = $"Sale Slot [{i}]";
            salesBtnList[i].SetSlotIndex(i);
        }
    }


    //레이캐스트시 첫 번째 UI에서 컴포넌트를 찾아 리턴
    private T GetFirstComponent<T>() where T : Component
    {
        rrList.Clear();// 레이캐스트 리스트 초기화

        gr.Raycast(ped, rrList);//가장 위쪽의 UGUI오브젝트 부터 rrList에 저장

        if (rrList.Count == 0) //리스트가 0이 아니라면
            return null;

        return rrList[0].gameObject.GetComponent<T>();
    }

    private void HighlightImg() //하이라이트 이미지 활성화 유무
    {
        //이전 클릭슬롯이 없고 현재 클릭슬롯이 있다면
        if (prevClickSlot == null && beginClickSlot != null)
        {
            beginClickSlot.ShowHighLight(true);
            prevClickSlot = beginClickSlot;
        }
        //이전 클릭슬롯과 현재 클릭슬롯이 동일하다면
        else if (prevClickSlot == beginClickSlot)
        {
            return;
        }
        //이전 클릭슬롯과 현재 클릭슬롯이 동일하지 않다면
        else if (prevClickSlot != beginClickSlot)
        {
            prevClickSlot.ShowHighLight(false);
            beginClickSlot.ShowHighLight(true);
            prevClickSlot = beginClickSlot;
        }
    }

    //구매 페이지 활성화
    private void ShowPerchaseToolTip(int index)
    {
        ItemData data = salesItems[index];

        pToolTip.SetItemInfo(data, (datainfo ,cnt)  =>  inven.Add(data,cnt), gold => inven.UseGold(gold));
    }

    //모든 버튼 초기화
    private void ClearBtn()
    {
        foreach(SalesBtnManager btn in salesBtnList)
        {
            btn.SetIcon(null);
        }
    }

    private void OnPointerDown()//마우스 클릭시
    {
        if (Input.GetMouseButtonDown(0))//좌클릭시
        {
            beginClickSlot = GetFirstComponent<SalesBtnManager>(); //클릭한 슬롯 가져오기

            //아이템을 갖고 있는 슬롯만 해당
            if (beginClickSlot != null && beginClickSlot.HasItem)
            {
                HighlightImg(); //하이라이트 이미지 보여주기
                ShowPerchaseToolTip(beginClickSlot.Index);
                BgmManager.Instance.PlayEffectSound("Button");
            }
        }
    }

    //모든 슬롯 상태 업데이트
    private void UpdateAllSlots()
    {
        for (int i = 0; i < salesItems.Count; i++)
        {
            ItemData data = salesItems[i];
            salesBtnList[i].SetIcon(data.IconSprite);
        }
    }

    //아이템 추가
    public void AddItem(ItemData[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            salesItems.Add(items[i]);
        }

        UpdateAllSlots();
    }

    //윈도우 활성화시
    public void Open()
    {
        GameManager.Instance.Inven.InvenUI.gameObject.SetActive(true);
        GameManager.Instance.Inven.InvenUI.isBusiness = true;
        this.gameObject.SetActive(true);
    }

    //윈도우 종료시
    public void Close()
    {
        if (prevClickSlot != null) { prevClickSlot.ShowHighLight(false); }//하이라이트 이미지 종료

        GameManager.Instance.Inven.InvenUI.gameObject.SetActive(false);
        GameManager.Instance.Inven.InvenUI.isBusiness = false;
        ClearBtn();
        salesItems.Clear();      
        this.gameObject.SetActive(false);
    }

  
}
