using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    //아이템 수용 한도
    public int Capacity { get; private set; }

    [SerializeField]
    private LivingEntity player;

    //장비 착용창
    [SerializeField]
    private EquipManager eManager;
    public EquipManager EquipManager => eManager;

    //초기 수용한도
    [SerializeField, Range(8, 64)]
    private int initalCapacity = 32;

    //최대 수용 한도
    [SerializeField, Range(8, 64)]
    private int maxCapacity = 64;

    //최대 수용가능한 무게
    [SerializeField]
    private int maxWeight;

    //현재 소유중인 금화
    [SerializeField]
    private int gold;

    [SerializeField]
    private InvenUIManager inventoryUI; //인벤토리 UI
    public InvenUIManager InvenUI => inventoryUI;

    [SerializeField] //아이템 목록
    private Item[] items;
    public Item[] InvenItems => items;

    //아이템이 추가 되었는지?
    private bool isItemAdded;

    //현재 무게
    public int currentWeight { get; private set; }
    public int Gold => gold;
    public int MaxWeight => maxWeight;
    public Action<ItemData> itemAddEvent; //아이템 획득시 이벤트

    private readonly static Dictionary<Type, int> sortWeight = new Dictionary<Type, int>
    {
        {typeof(PotionItemData), 10000},
        {typeof(WeaponItemData), 20000 },
        {typeof(ArmorItemData), 30000 },
        {typeof(PropItemData), 40000 }
    };

    //가중치 비교
    private class ItemCorparer : IComparer<Item>
    {
        public int Compare(Item a, Item b)
        {
            var aDummy = a.Data;
            var bDummy = b.Data;

            if (!sortWeight.ContainsKey(a.Data.GetType()))
            {
                if(a is PotionItem)
                {
                    PotionItemData pdata = ScriptableObject.CreateInstance<PotionItemData>(); //new PortionItemData();

                    aDummy = pdata;                
                }
            }

            if (!sortWeight.ContainsKey(b.Data.GetType()))
            {
                if (b is PotionItem)
                {
                    PotionItemData pdata = ScriptableObject.CreateInstance<PotionItemData>();
                    bDummy = pdata;
                }
            }

            return ((a.Data.ID + sortWeight[aDummy.GetType()])
                   - (b.Data.ID + sortWeight[bDummy.GetType()]));
        }      
    }

    private static readonly ItemCorparer Icomparer = new ItemCorparer();
 

    private void Awake()
    {
        items = new Item[maxCapacity];
        Capacity = initalCapacity;
        isItemAdded = false;
        currentWeight = 0;     
    }

    private void Start()
    {
        UpdateAccessStateAll();
    }

    //인덱스가 수용범위 내에 있는지 검사
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }

    //비어있는 슬롯 찾기
    private int FindEmptySlot(int startIndex = 0)
    {
        for(int i = startIndex; i<Capacity; i++)
        {
            if (items[i] == null)
                return i;
        }
        return -1;
    }

    //앞에서부터 더 추가할수 있는 Countable 아이템의 슬롯 인덱스 탐색
    private int FindCountItemSlot(CountableItemData target, int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
        {
            var current = items[i];
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


    //해당 인덱스의 슬롯 상태및 UI갱신
    private void UpdateSlot(int index)
    {
        if (!IsValidIndex(index)) return; //접근 가능한 인덱스가 아닐시

        Item item = items[index];

        if (item != null)//아이템이 슬롯에 존재할 경우
        {
            //아이콘 등록
            inventoryUI.SetItemIcon(index, item.Data.IconSprite);

            if (item is CountableItem ci)//셀수있는 아이템일경우
            {
                if (ci.IsEmpty) //수량이 0일경우 아이템 제거
                {
                    items[index] = null;
                    RemoveIcon();
                    return;
                }

                else //수량 텍스트 표시
                {
                    inventoryUI.SetItemAmountText(index, ci.Amount);
                }
            }
            else if (item is EquipmentItem ei)//장비 아이템일경우
            {
                //장비 무게 업데이트
                if (isItemAdded && currentWeight + ei.PropWeight <= MaxWeight)
                {
                    currentWeight += ei.PropWeight;
                    inventoryUI.UpdateWeight();
                }
                inventoryUI.SetItemAmountText(index, 1);
                isItemAdded = false;
            }

            else //빈슬롯일경우 아이콘 제거
            {
                RemoveIcon();
            }

            void RemoveIcon()
            {
                inventoryUI.RemoveItem(index);               
            }
        }
        else
        {
            if (inventoryUI.HasIcon(index))
            { inventoryUI.RemoveItem(index); }
        }
    }


    //모든 슬롯 업데이트
    private void UpdateAllSlot()
    {
        for (int i = 0; i < Capacity; i++)
        {
            UpdateSlot(i);
        }
    }

    //모든 슬롯 UI에 접근 가능 여부 업데이트
    public void UpdateAccessStateAll()
    {
        inventoryUI.SetAccessibleSlotRange(Capacity);
    }

    //해당 슬롯에 아이템이 있는지 여부
    private bool HasItem(int index)
    {
        return IsValidIndex(index) && items[index] != null;
    }

    //최대 무게에 도달했는지
    private bool IsMaximunWeight(int weight)
    {
        int totalWeight = currentWeight + weight;
        if (totalWeight <= MaxWeight)
        {
            return false;
        }
        else
        {
            return true;
        }      
    }

    //해당 슬롯에 접근 가능한지
    private bool IsAccesible(int index)
    {
        //인덱스 범위가 정상이 아니거나 배열이 비어있다면
        if (!IsValidIndex(index) || items[index] == null)
        {
            return false;
        }
        else
        {
            return true;
        }    
    }

    //해당 슬롯에 있는 아이템이 샐수있는 아이템인지
    public bool IsCountableItem(int index)
    {
        return HasItem(index) && items[index] is CountableItem;
    }
    
    //해당 슬롯의 아이템 개수 리턴
    public int GetCurrentAmount(int index)
    {
        if (!IsValidIndex(index)) return -1; //잘못된 인덱스
        if (items[index] == null) return 0; //해당 슬롯이 비어있으면

        CountableItem ci = items[index] as CountableItem;
        if (ci == null)
            return 1; //셀수 없는 아이템이면

        return ci.Amount;
    }

    //해당 슬롯의 아이템 정보 리턴
    public ItemData GetItemData(int index)
    {
        if (!IsValidIndex(index)) return null; //잘못된 인덱스이면
        if (items[index] == null) return null;  // 슬롯이 비어있으면

        return items[index].Data;

    }

    //해당 슬롯의 아이템 이름 리턴
    public string GetItemName(int index)
    {
        if (!IsValidIndex(index)) return ""; //잘못된 인덱스이면
        if (items[index] == null) return "";  // 슬롯이 비어있으면

        return items[index].Data.ItemName; 
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
                    index = FindCountItemSlot(ciData, index + 1);

                    //더 추가 할수 없을 경우
                    if(index == -1)
                    {
                        findNextCountable = false;
                    }

                    //추가 할수 있는 슬롯을 찾았을 경우
                    else
                    {
                        CountableItem ci = items[index] as CountableItem; //아이템 배열가져오기
                        amount = ci.AddAmountAndGetExcess(amount);// 양 증가시키기 

                        UpdateSlot(index);
                    }
                }

                //빈슬롯 탐색
                else
                {
                    index = FindEmptySlot(index + 1);
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
                        items[index] = ci;

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
                index = FindEmptySlot();

                if(index != -1)
                {
                    EquipItemData edata = itemData as EquipItemData;

                    //무게를 더 추가 가능한지 확인
                    if(IsMaximunWeight(edata.Weight))
                    {
                        NotificationUI.Instance.GenerateTxt("더 이상 아이템을 넣을수 없습니다");                       
                        return -1;
                    }
                    
                    //아이템을 생성해서 슬롯에 추가
                    items[index] = edata.CreateItem();
                    amount = 0;

                    isItemAdded = true;
                    UpdateSlot(index);                 
                }
            }

            //2개이상의 아이템을 넣을 경우
            else
            {
                index = -1;
                for (; amount > 0; amount--)
                {
                    EquipItemData edata = itemData as EquipItemData;

                    //무게를 더 추가 가능한지 확인
                    if (IsMaximunWeight(edata.Weight))
                    {
                        NotificationUI.Instance.GenerateTxt("더 이상 아이템을 넣을수 없습니다");
                        break;
                    }

                    index = FindEmptySlot(index + 1);

                    // 아이템을 다 넣지 못할경우
                    if (index == -1)
                    {
                        break;
                    }

                    //아이템을 생성하여 슬롯에 추가
                    items[index] = edata.CreateItem();

                    isItemAdded = true;
                    UpdateSlot(index);
                }
            }
          
                
        }

        if(itemAddEvent != null)
        {
            itemAddEvent(itemData);
        }

        return index;
    }

    //아이템 삭제
    public void Remove(int index, int count)
    {
        //접근 가능한 아이템인지
        if(!IsAccesible(index))
        {
            return;
        }

        //셀수 있는 아이템이라면
        if (items[index] is CountableItem ciData)
        {
            ciData.RemoveItem(count);

            if(ciData.IsEmpty)
            {
                items[index] = null;
                inventoryUI.RemoveItem(index);
            }

        }
        else
        {
            //장비아이템일시 무게 감소
            if (items[index] is EquipmentItem eItem)
            {
                currentWeight -= eItem.PropWeight;
                inventoryUI.UpdateWeight();
            }          

            items[index] = null; //배열 비우기
            inventoryUI.RemoveItem(index);
        }

        UpdateSlot(index);
    }

    //아이템 사용
    public void Use(int index)
    {
        //접근 가능한 아이템인지
        if (!IsAccesible(index))
        {
            return;
        }
        //사용가능한 아이템이라면
        if (items[index] is IUsableItem uItem)
        {
            if(uItem.Use(player))
            {
                UpdateSlot(index);
            }
        }
    }

    //아이템 착용
    public void Equip(int index)
    { 
        //접근 가능한 아이템인지
        if (!IsAccesible(index))
        {
            return;
        }

        if (items[index] is WeaponItem wItem) //무기 아이템일시
        {
            if(eManager.hasWeapon)
            {
                Item changeItem = eManager.WITEM;
                eManager.SetWeapon(player,wItem,()=> Add(wItem.Data,1));
                items[index] = changeItem;
            }
            else
            {
                eManager.SetWeapon(player,wItem, () => Add(wItem.Data, 1));
                items[index] = null;
                inventoryUI.RemoveItem(index);
            }
               
        }
        else if(items[index] is ArmorItem aItem)//방어구 아이템일시
        {
            if (eManager.hasArmor) //이미 착용중인 방어구가 있다면
            {
                Item changeItem = eManager.AITEM; //인벤토리에 집어넣을 아이템 가져오기
                eManager.SetArmor(player, aItem, () => Add(aItem.Data, 1)); //아이템 장비 시키기
                items[index] = changeItem; //장비했던 아이템 해재시키기
            }
            else
            {
                eManager.SetArmor(player, aItem, () => Add(aItem.Data, 1));
                items[index] = null;
                inventoryUI.RemoveItem(index);
            } 
        }

        UpdateSlot(index); 
    }

    //아이템 정렬
    public void SortAll()
    {
        int i = -1;
        while (items[++i] != null) ;
        int j = i;

        while (true)
        {
            while (++j < Capacity && items[j] == null) ;

            if (j == Capacity)
                break;

            items[i] = items[j];
            items[j] = null;
            i++;
        }
        Array.Sort(items, 0, i, Icomparer);

        UpdateAllSlot();
        inventoryUI.UpdateAllSlots();
    }

    //골드 얻기
    public void GetGold(int amount)
    {
        gold += amount;
        inventoryUI.UpdateGold();
    }

    //골드 소모
    public bool UseGold(int amount)
    {
        if(gold - amount >=0)
        {
            gold -= amount;

            inventoryUI.UpdateGold();
            return true;
        }
        else
        {
            return false;
        }
    }

    //아이템 수량 반환
    public (int count, int index) GetItemCount(int itmeId)
    {
        int itemCount = 0;
        int index = 0;

        for(int i = 0; i<Capacity; i++)
        {
            var current = items[i];
            if (current == null)
                continue;

            if (current.Data.ID == itmeId && current is CountableItem ci)
            {
                itemCount += ci.Amount;
                index = i;
            }
        }

        return (itemCount,index);
    }

    //스킬북 갯수 반환
    public (int count, int index) CountSkillBook()
    {          
        int amount = 0;
        int index = 0;

        for(int i = 0; i<Capacity; i++)
        {
            var current = items[i];
            if (current == null)
                continue;

          
            if(current.Data.ID == 36 && current is CountableItem ci)
            {
                amount += ci.Amount;
                index = i;
            }

        }

        return (amount, index);
    }

    //최대 무게 설정
    public void SetMaxWeight(int _weight)
    {
        this.maxWeight += _weight;
    }
}
