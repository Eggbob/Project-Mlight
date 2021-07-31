using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectPool : MonoBehaviour
{
    public static ItemObjectPool instance;

    [SerializeField]
    private GameObject coinPrefab; //코인 프리팹

    //코인 큐
    private Queue<CoinPickUp> coinQueue = new Queue<CoinPickUp>();

    //포션 아이템 데이터 리스트
    [SerializeField]
    private Dictionary<int,ItemData> potionItems = new Dictionary<int, ItemData>();

    //물품 아이템 데이터 리스트
    [SerializeField]
    private Dictionary<int, ItemData> propItems = new Dictionary<int, ItemData>();

    //장비 아이템 데이터 리스트
    [SerializeField]
    private Dictionary<int, ItemData> equipItems = new Dictionary<int, ItemData>();

    //포션 아이템 딕셔너리
    private Dictionary<int, Queue<GameObject>> potionDictionary = new Dictionary<int, Queue<GameObject>>();

    //물품 아이템 딕셔너리
    private Dictionary<int, Queue<GameObject>> propDictionary = new Dictionary<int, Queue<GameObject>>();

    //장비 아이템 딕셔너리
    private Dictionary<int, Queue<GameObject>> equipmentDictionary = new Dictionary<int, Queue<GameObject>>();

    private void Awake()
    {
        instance = this;
        AddItems();
        Initialize(10);
    }

    //리소스 폴더에서 아이템 가져오기
    private void AddItems()
    {   
        //포션 아이템 불러오기
        ItemData[] itemDatas = Resources.LoadAll<ItemData>("PotionItems");

        //포션 아이템 키값저장
        foreach(ItemData data in itemDatas)
        {
         
            potionItems.Add(data.ID, data);
        }

        Array.Clear(itemDatas, 0, itemDatas.Length);//배열 초기화 하기
    
        //물품 아이템 불러오기
        itemDatas = Resources.LoadAll<ItemData>("PropItems");

        //물품 아이템 키값저장
        foreach (ItemData data in itemDatas)
        {       
            propItems.Add(data.ID, data);
        }

        Array.Clear(itemDatas, 0, itemDatas.Length);//배열 초기화 하기

        //장비 아이템 불러오기
        itemDatas = Resources.LoadAll<ItemData>("EquipItems");

        //장비 아이템 키값저장
        foreach (ItemData data in itemDatas)
        {
            equipItems.Add(data.ID, data);
        }
    }

    //초기 생성
    private void Initialize(int count)
    {
        for(int i = 0; i< count; i++)
        {
            //포션 아이템 생성
            foreach(KeyValuePair<int, ItemData> index in potionItems)
            {
                CreateNewItemPrefab(potionDictionary, index.Value);          
            }
            //물품 아이템 생성
            foreach(KeyValuePair<int, ItemData> index in propItems)
            {
                CreateNewItemPrefab(propDictionary, index.Value);
            }

            CreateNewCoinPrefab();
        }

        //장비 아이템 생성
        foreach(KeyValuePair<int, ItemData> index in equipItems)
        {
            CreateNewItemPrefab(equipmentDictionary, index.Value);
        }
    }

    //새로운 드랍 아이템 생성
    private void CreateNewItemPrefab(Dictionary<int, Queue<GameObject>> itemDic,ItemData iData)
    {
        //만약 키값이 존재하지 않다면
        if(!itemDic.ContainsKey(iData.ID))
        {
            Queue<GameObject> itemPref = new Queue<GameObject>();
            var newObj = Instantiate(iData.DropItem, transform);
            newObj.SetActive(false);
            itemPref.Enqueue(newObj);
            itemDic.Add(iData.ID, itemPref);
        }
        else
        {
            var newObj = Instantiate(iData.DropItem, transform);
            newObj.SetActive(false);
            itemDic[iData.ID].Enqueue(newObj);
        }         
    }

    //코인 프리팹 생성
    private void CreateNewCoinPrefab()
    {
        var newObj = Instantiate(coinPrefab, transform).GetComponent<CoinPickUp>();
        newObj.gameObject.SetActive(false);
        coinQueue.Enqueue(newObj);
    }

    //포션 아이템 가져가기
    public static GameObject GetPotionItem(int id)
    {
        //만약 키값이 존재한다면
        if(instance.potionDictionary.ContainsKey(id))
        {
            var item = instance.potionDictionary[id].Dequeue();
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);
            return item;
        }
        else
        {
            instance.CreateNewItemPrefab(instance.potionDictionary, instance.potionItems[id]);
            var item = instance.potionDictionary[id].Dequeue();
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    //물품 아이템 가져가기
    public static GameObject GetPropItem(int id)
    {
        //만약 키값이 존재한다면
        if (instance.propDictionary.ContainsKey(id))
        {
            var item = instance.propDictionary[id].Dequeue();
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);
            return item;
        }
        else
        {
            instance.CreateNewItemPrefab(instance.propDictionary, instance.propItems[id]);
            var item = instance.propDictionary[id].Dequeue();
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    //장비 아이템 가져가기
    public static GameObject GetEquipItem(int id)
    {
        //만약 키값이 존재한다면
        if (instance.equipmentDictionary.ContainsKey(id))
        {
            var item = instance.equipmentDictionary[id].Dequeue();
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);
            return item;
        }
        else
        {
            instance.CreateNewItemPrefab(instance.equipmentDictionary, instance.equipItems[id]);
            var item = instance.equipmentDictionary[id].Dequeue();
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    //코인 프리팹 가져가기
    public static CoinPickUp GetCoinItem()
    {
        //가져갈 코인 갯수가 존재할시
        if(instance.coinQueue.Count > 0)
        {
            var obj = instance.coinQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            instance.CreateNewCoinPrefab();
            var obj = instance.coinQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }    
    }

    //아이템 회수하기
    public static void ReturnItem(GameObject item, int id)
    {
        item.SetActive(false);
        item.transform.SetParent(instance.transform);

        switch(item.tag)
        {
            case "Props":
                instance.propDictionary[id].Enqueue(item);
                break;

            case "Potion":
                instance.potionDictionary[id].Enqueue(item);
                break;

            case "Equipment":
                instance.equipmentDictionary[id].Enqueue(item);
                break;
        }
    }

    //코인 회수하기
    public static void ReturnCoin(CoinPickUp coin)
    {
        coin.gameObject.SetActive(false);
        coin.transform.SetParent(instance.transform);
        instance.coinQueue.Enqueue(coin);
    }

}
