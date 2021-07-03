using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvenUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform contentArea; //슬롯이 위치할 영역
    [SerializeField] private RectTransform quickArea; //슬롯이 위치할 영역
    [SerializeField] private Button sortBtn; //정렬 버튼
    [SerializeField] private Slider weightSlider; //무게 슬라이더
    [SerializeField] private Text weightTxt; //무게 텍스트
    [SerializeField] private Text goldTxt; //골드 텍스트


    [Space]
    [SerializeField] private bool showItemTooltip = false; //아이템 툴팁 활성화 여부
    [SerializeField] private bool showEquipTooltip = false; //장비 툴팁 활성화 여부
    [SerializeField] private InvenToolTipManager tooltip; //툴팁
    [SerializeField] private InvenPopUpUIManager popUp; //팝업 UI
    [SerializeField] private InvenEquipToolTipManager eTooltip; //장비 툴팁

    private ItemSlotUI[] slotUiList; //아이템 슬롯
    private ItemQuickSlotUI[] QuickSlots; //퀵슬롯 버튼

    private GraphicRaycaster gr;
    private PointerEventData ped;//마우스 입력감지
    private List<RaycastResult> rrList;

    //연결된 인벤토리 
    [SerializeField]
    private Inventory inventory;

    private ItemSlotUI prevClickSlot; // 이전 클릭한 슬롯
    private ItemSlotUI beginClickSlot; //현재 클릭한  슬롯
 
    //인벤토리 UI 내 아이템 필터링 옵션 
    private enum FilterOption
    {
        All, Equipment, Portion, Prop
    }

    private FilterOption currentFilterOption = FilterOption.All;


    private void Awake()
    {
        Init();
        QuickInit();
    }

    private void Update()
    {
        ped.position = Input.mousePosition;

        OnPointerDown();
        OnPointerUp();
    }

    //초기 설정
    private void Init()
    {
        TryGetComponent(out gr); //그래픽 레이캐스터 가져오기
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);

        //무게 설정
        weightSlider.value = 0;        
        weightTxt.text =  inventory.currentWeight + " / " + inventory.MaxWeight;

        UpdateGold();

        //정렬 버튼 이벤트 등록
        sortBtn.onClick.AddListener(() => inventory.SortAll());

        slotUiList = contentArea.GetComponentsInChildren<ItemSlotUI>();

        for (int i = 0; i < slotUiList.Length; i++)
        {
            slotUiList[i].name = $"Item Slot [{i}]";
            slotUiList[i].SetSlotIndex(i);
        }

      
    }

    //퀵슬롯 초기 설정
    private void QuickInit()
    {
        QuickSlots = quickArea.GetComponentsInChildren<ItemQuickSlotUI>();

        QuickSlots[0].SetSlotIndex(0);
        QuickSlots[0].SetSlotEvent(()=>SetQuickSlot(0));

        //for (int i = 0; i <= 2; i++)
        //{
        //    QuickSlots[i].SetSlotIndex(i);
        //    QuickSlots[i].SetSlotEvent(()=>SetQuickSlot(i));

        //}
    }

    //레이캐스트시 첫 번째 UI에서 컴포넌트를 찾아 리턴
    private T RaycastAndGetFirstComponent<T>() where T : Component
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
        if(prevClickSlot == null && beginClickSlot != null)
        {
            beginClickSlot.Highlight(true);
            prevClickSlot = beginClickSlot;
        }
        //이전 클릭슬롯과 현재 클릭슬롯이 동일하다면
        else if(prevClickSlot == beginClickSlot)
        {
            return;
        }
        //이전 클릭슬롯과 현재 클릭슬롯이 동일하지 않다면
        else if(prevClickSlot != beginClickSlot)
        {
            prevClickSlot.Highlight(false);
            beginClickSlot.Highlight(true);
            prevClickSlot = beginClickSlot;
        }
    }



    public void OnPointerDown()//마우스 클릭시
    {
       if(Input.GetMouseButtonDown(0))//좌클릭시
        {
            beginClickSlot = RaycastAndGetFirstComponent<ItemSlotUI>(); //클릭한 슬롯 가져오기

            //아이템을 갖고 있는 슬롯만 해당
            if(beginClickSlot != null && beginClickSlot.HasItem && beginClickSlot.IsAccesible)
            {         
                HighlightImg(); //하이라이트 이미지 보여주기
                ShowToolTip(beginClickSlot.Index); //툴팁보여주기
            }
            //else
            //{
            //    beginClickSlot = null;
            //}
        }
    }

    public void OnPointerUp()//마우스 클릭을 뗄 경우
    {
       if(Input.GetMouseButtonUp(0))
       {
           if(beginClickSlot != null) //현재 드래그 중인 슬롯이 null이 아닐경우
            {
                //참조 제거
                //beginClickSlot = null;      
            }
       }
    }


    // 접근 가능한 슬롯 범위 설정 
    public void SetAccessibleSlotRange(int accessibleSlotCount)
    {
        for (int i = 0; i < slotUiList.Length; i++)
        {
            slotUiList[i].SetSlotAccessibleState(i < accessibleSlotCount);
        }
    }

    //슬롯에 아이템 아이콘 등록
    public void SetItemIcon(int index, Sprite icon)
    {
        EditorLog($"Set Item Icon : Slot [{index}]");

        slotUiList[index].SetItem(icon);
    }

    //해당 슬롯의 아이템 개수 텍스트 지정 
    public void SetItemAmountText(int index, int amount)
    {
        EditorLog($"Set Item Amount Text : Slot [{index}], Amount [{amount}]");

        // amount가 1 이하일 경우 텍스트 미표시
        slotUiList[index].SetItemAmount(amount);

        //만약 슬롯의 아이템이 퀵슬롯에 장착이 되어있다면
        if(slotUiList[index].IsQuicked)
        {
            int slotindex = slotUiList[index].QuickedIndex;
            QuickSlots[slotindex].UpdateItemAmount();
        }
    }

    public bool HasIcon(int index)
    {
        return slotUiList[index].HasItem;
    }

    // 해당 슬롯의 아이템 개수 텍스트 지정 
    public void HideItemAmountText(int index)
    {
        EditorLog($"Hide Item Amount Text : Slot [{index}]");

        slotUiList[index].SetItemAmount(1);
    }

    // 슬롯에서 아이템 아이콘 제거, 개수 텍스트 숨기기
    public void RemoveItem(int index)
    {
        EditorLog($"Remove Item : Slot [{index}]");

        slotUiList[index].RemoveItem();
        HideItemAmountText(index);
        slotUiList[index].Highlight(false);
    }

    //특정 슬롯 상태 업데이트
    public void UpdatSlotState(int index, ItemData itemData)
    {
        bool isUpdated = true;

        if(itemData != null)
        {
            switch(currentFilterOption)
            {
                case FilterOption.Equipment:
                    isUpdated = (itemData is EquipItemData);
                    break;

                case FilterOption.Portion:
                    isUpdated = (itemData is PortionItemData);
                    break;                   
            }
        }

        slotUiList[index].SetItemAccessibleState(isUpdated);
    }

    //모든 슬롯 상태 업데이트
    public void UpdateAllSlots()
    {
        int capacity = inventory.Capacity;

        for(int i = 0; i<capacity; i++)
        {
            ItemData data = inventory.GetItemData(i);
            UpdatSlotState(i, data);
        }
    }
    //무게 업데이트
    public void UpdateWeight()
    {
        weightTxt.text = inventory.currentWeight + " / " + inventory.MaxWeight;
        weightSlider.value = (float)inventory.currentWeight / (float)inventory.MaxWeight;
    }

    //골드 업데이트
    public void UpdateGold()
    {
        string amountTxt = string.Format("{0:#,0}", inventory.Gold);
        goldTxt.text = amountTxt + "G";
    }

    //툴팁 보여주기
    private void ShowToolTip(int index)
    {
        ItemData data = inventory.GetItemData(index);

        if(data is PortionItemData) //셀수있는 아이템이면
        {
            int currentAmount = inventory.GetCurrentAmount(index);

            tooltip.SetItemInfo(data, () => inventory.Use(index), () => ShowToolTip(index),
                () => ShowConfirm(index, currentAmount), currentAmount);
            
        
            if(showEquipTooltip)
            {
                eTooltip.gameObject.SetActive(false);
                showEquipTooltip = false;
            }

            showItemTooltip = true;
        }
        else if(data is PropItemData)
        {
            int currentAmount = inventory.GetCurrentAmount(index);
            tooltip.SetPropItemInfo(data, () => ShowConfirm(index, currentAmount), currentAmount);

            if (showEquipTooltip)
            {
                eTooltip.gameObject.SetActive(false);
                showEquipTooltip = false;
            }

            showItemTooltip = true;
        }

        else if(data is EquipItemData)//착용 아이템이면
        {
            eTooltip.SetItemInfo(data, () => inventory.Equip(index), () => ShowConfirm(index, 1));

            if (showItemTooltip)
            {
                tooltip.gameObject.SetActive(false);
                showItemTooltip = false;
            }
            showEquipTooltip = true;
        }
      
    }

    //확인창 보여주기
    private void ShowConfirm(int index, int currentAmount)
    {
       // popUp.ShowConfirmUI(cnt => TryRemoveItem(index, cnt), currentAmount);
       popUp.ShowConfirmUI(cnt => inventory.Remove(index, cnt), currentAmount);
    }

    //아이템 제거
    private void TryRemoveItem(int index, int count)
    {
        inventory.Remove(index, count);
    }

    //아이템 퀵슬롯 등록
    private void SetQuickSlot(int qIndex)
    {
        if (prevClickSlot == null) return;

        int index = prevClickSlot.Index;


        ItemData data = inventory.GetItemData(index);

        if (data is PortionItemData) //포션 아이템이면
        {
            int currentAmount = inventory.GetCurrentAmount(index);

            prevClickSlot.SetQuickSlotIndex(qIndex); //아이템이 퀵슬롯에 장착되었는지 업데이트
            QuickSlots[qIndex].SetItem(data, currentAmount, () => inventory.Use(index), () => inventory.GetCurrentAmount(index));

        }
       

    }

#if UNITY_EDITOR
    [Header("Editor Options")]
    [SerializeField] private bool _showDebug = true;
#endif

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void EditorLog(object message)
    {
        if (!_showDebug) return;
        UnityEngine.Debug.Log($"[InventoryUI] {message}");
    }
}
