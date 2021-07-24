using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField] //드랍할 아이템 리스트
    private List<ItemData> dropItems = new List<ItemData>();

    private LivingEntity LCon;

    private void Awake()
    {
        LCon = this.GetComponent<LivingEntity>();
        LCon.DieAction += GenerateItems; //구독시키기

    }

    //드랍할 아이템 초기화
    public void SetTable(List<ItemData> droplist)      
    {
        foreach(ItemData dData in droplist)
        {
            dropItems.Add(dData);
        }
    }

    //아이템 생성
    private void GenerateItems()
    {
       foreach(ItemData data in dropItems)
        {
        
           if (data is PotionItemData)
           {
                var obj = ItemObjectPool.GetPotionItem(data.ID);               
                obj.transform.position = this.transform.position;
                obj.GetComponent<Rigidbody>().velocity =
                  new Vector3(Random.Range(0f, 5f), Random.Range(0f, 10f), Random.Range(0f, 5f));

            }
           else if(data is PropItemData)
            {
                var obj = ItemObjectPool.GetPropItem(data.ID);
                obj.transform.position = this.transform.position;
                obj.GetComponent<Rigidbody>().velocity = 
                    new Vector3(Random.Range(0f, 5f), Random.Range(0f, 10f), Random.Range(0f, 5f));
            }
        }
    }

   
}
