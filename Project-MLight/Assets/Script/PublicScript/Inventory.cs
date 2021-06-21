using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //아이템 수용 한도
   public int Capacity { get; private set; }

    //초기 수용한도
    [SerializeField, Range(8, 64)]
    private int _initalCapacity = 32;

    //최대 수용 한도
    [SerializeField, Range(8, 64)]
    private int _maxCapacity = 64;

    [SerializeField]
    private InvenUIManager _inventoryUI; //인벤토리 UI

    [SerializeField] //아이템 목록
    private Item[] _items;

    private void Awake()
    {
        _items = new Item[_maxCapacity];
        Capacity = _initalCapacity;
    }

    private void Start()
    {
        UpdateAccessibleStatesAll();
    }

    //인덱스가 수용범위 내에 있는지 검사
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }

    //비어있는 슬롯 찾기
    private int FindEmptySlotIndex(int startIndex = 0)
    {
        for(int i = startIndex; i<Capacity; i++)
        {
            if (_items[i] == null)
                return i;
        }
        return -1;
    }

    /// <summary> 앞에서부터 개수 여유가 있는 Countable 아이템의 슬롯 인덱스 탐색 </summary>
    private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
        {
            var current = _items[i];
            if (current == null)
                continue;

            // 아이템 종류 일치, 개수 여유 확인
            if (current.Data == target && current is CountableItem ci)
            {
                if (!ci.IsMax)
                    return i;
            }
        }

        return -1;
    }

    //모든 슬롯 UI에 접근 가능 여부 업데이트
    public void UpdateAccessibleStatesAll()
    {
        _inventoryUI.SetAccessibleSlotRange(Capacity);
    }

    //해당 슬롯에 아이템이 있는지 여부
    private bool HasItem(int index)
    {
        return IsValidIndex(index) && _items[index] != null;
    }

    //해당 슬롯에 있는 아이템이 샐수있는 아이템인지
    public bool IsCountableItem(int index)
    {
        return HasItem(index) && _items[index] is CountableItem;
    }
    
    //현재 아이템 개수 리턴
    public int GetCurrentAmount(int index)
    {
        if (!IsValidIndex(index)) return -1; //잘못된 인덱스
        if (_items[index] == null) return 0; //해당 슬롯이 비어있으면

        CountableItem ci = _items[index] as CountableItem;
        if (ci == null)
            return 1; //셀수 없는 아이템이면

        return ci.Amount;
    }

    //해당 슬롯의 아이템 정보 리턴
    public ItemData GetItemData(int index)
    {
        if (!IsValidIndex(index)) return null; //잘못된 인덱스이면
        if (_items[index] == null) return null;  // 슬롯이 비어있으면

        return _items[index].Data;

    }

    //해당 슬롯의 아이템 이름 리턴
    public string GetItemName(int index)
    {
        if (!IsValidIndex(index)) return ""; //잘못된 인덱스이면
        if (_items[index] == null) return "";  // 슬롯이 비어있으면

        return _items[index].Data.Name; 
    }


    //해당 인덱스의 슬롯 상태및 UI갱신
    public void UpdateSlot(int index)
    {
        if (!IsValidIndex(index)) return;

        Item item = _items[index];

        if(item != null)//아이템이 슬롯에 존재할 경우
        {
            //아이콘 등록
            _inventoryUI.SetItemIcon(index, item.Data.IconSprite);

            if(item is CountableItem ci)//셀수있는 아이템일경우
            {
                if(ci.IsEmpty) //수량이 0일경우 아이템 제거
                {
                    _items[index] = null;
                    RemoveIcon();
                    return;
                }

                else //수량 텍스트 표시
                {
                    _inventoryUI.SetItemAmountText(index, ci.Amount);
                }
            }

            else //빈슬롯일경우 아이콘 제거
            {
                RemoveIcon();
            }

            void RemoveIcon()
            {
                _inventoryUI.RemoveItem(index);
                _inventoryUI.HideItemAmountText(index);
            }
        }
    }


    //아이템 집어 넣기
    public int Add(ItemData itemData, int amount = 1)
    {
        int index;

        //수량이 있는 아이템이라면
        if(itemData is CountableItemData ciData)
        {
            bool findNextCountable = true;
            index = -1;

            while(amount > 0)
            {
                // 해당 아이템이 인벤토리 내에 존재하고 더 추가 할수 있는지 검사
                if(findNextCountable)
                {
                    index = FindCountableItemSlotIndex(ciData, index + 1);

                    //더 추가 할수 없을 경우
                    if(index == -1)
                    {
                        findNextCountable = false;
                    }

                    //추가 할수 있는 슬롯을 찾았을 경우
                    else
                    {
                        CountableItem ci = _items[index] as CountableItem; //아이템 배열가져오기
                        amount = ci.AddAmountAndGetExcess(amount);// 양 증가시키기 

                        UpdateSlot(index);
                    }
                }

                //빈슬롯 탐색
                else
                {
                    index = FindEmptySlotIndex(index + 1);

                    //빈슬롯이 없을 경우
                    if(index == -1)
                    {
                        break;
                    }
                    else
                    {
                        //새로운 아이템 생성
                        CountableItem ci = ciData.CreateItem() as CountableItem;
                        ci.SetAmount(amount);

                        //슬롯에 추가
                        _items[index] = ci;

                        // 남은 개수 계산
                        amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                        UpdateSlot(index);
                    }
                }
            }
        }

        //수량이 없는 아이템이라면
        else
        {
            // 아이템 1개만 얻을시
            if(amount == 1)
            {
                index = FindEmptySlotIndex();

                if(index != -1)
                {
                    //아이템을 생성해서 슬롯에 추가
                    _items[index] = itemData.CreateItem();
                    amount = 0;

                    UpdateSlot(index);
                }
            }

            //2개이상의 아이템을 넣을 경우
            index = -1;
            for(; amount > 0; amount --)
            {
                index = FindEmptySlotIndex(index + 1);

                // 아이템을 다 넣지 못할경우
                if(index == -1)
                {
                    break;
                }

                //아이템을 생성하여 슬롯에 추가
                _items[index] = itemData.CreateItem();

                UpdateSlot(index);
            }

        }

        return amount;
    }

    //아이템 삭제
    public void Remove(int index)
    {
        if (!IsValidIndex(index)) return; //인덱스 범위가 정상이 아니라면 

        _items[index] = null; //배열 비우기
        UpdateSlot(index);
    }

    //아이템 사용
    public void Use(int index)
    {
        if (!IsValidIndex(index)) return;
        if (_items[index] == null) return;

        if(_items[index] is IUsableItem uItem)
        {
            if(uItem.Use())
            {
                UpdateSlot(index);
            }
        }
    }

}
