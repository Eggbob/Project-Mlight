using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    //드랍할 아이템 구조체
    [System.Serializable] 
    private struct DropItem
    {
        [SerializeField]
        private ItemData dropData;
        [SerializeField]
        private int itemAmount;

        public ItemData DropData => dropData;
        public int ItemAmount => itemAmount;

        public void Init(ItemData data, int _itemAmount)
        {
            dropData = data;
            itemAmount = _itemAmount;
        }
    }

    [SerializeField] //드랍할 아이템 리스트
    private List<DropItem> dropItems = new List<DropItem>();
    //private List<ItemData> dropItems = new List<ItemData>();

    [SerializeField]
    private int goldAmount;

    private LivingEntity LCon;

    private void Awake()
    {
        LCon = this.GetComponent<LivingEntity>();
        LCon.AddDieAction(GenerateItems);     
    }

    //드랍할 아이템 초기화
    public void SetTable(List<ItemData> droplist, int _goldAmount)      
    {
        int radomAmount;
        
        foreach (ItemData dData in droplist)
        {
            DropItem dropItem = new DropItem();
            radomAmount = Random.Range(3, 10);

            dropItem.Init(dData, radomAmount);

            dropItems.Add(dropItem);
        }

        goldAmount = _goldAmount;
    }

    //아이템 생성
    private void GenerateItems()
    {
       foreach(DropItem data in dropItems)
       {     
            if (data.DropData is PotionItemData)
            {
                var obj = ItemObjectPool.GetPotionItem(data.DropData.ID);
                obj.GetComponent<ItemPickUp>().Init(data.DropData, data.ItemAmount);
                obj.transform.position = this.transform.position;
                obj.GetComponent<Rigidbody>().velocity =
                    new Vector3(Random.Range(0f, 5f), Random.Range(0f, 10f), Random.Range(0f, 5f));

            }
            else if(data.DropData is PropItemData)
            {
                var obj = ItemObjectPool.GetPropItem(data.DropData.ID);
                obj.GetComponent<ItemPickUp>().Init(data.DropData, data.ItemAmount);
                obj.transform.position = this.transform.position;
                obj.GetComponent<Rigidbody>().velocity = 
                    new Vector3(Random.Range(0f, 5f), Random.Range(0f, 10f), Random.Range(0f, 5f));
            }
       }

        var coin = ItemObjectPool.GetCoinItem();
        coin.GetComponent<CoinPickUp>().Init(goldAmount);
        coin.transform.position = this.transform.position;
        coin.GetComponent<Rigidbody>().velocity =
                    new Vector3(Random.Range(0f, 5f), Random.Range(0f, 5f), Random.Range(0f, 5f));

    }

   
}
