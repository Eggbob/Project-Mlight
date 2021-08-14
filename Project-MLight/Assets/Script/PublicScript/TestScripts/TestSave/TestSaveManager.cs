using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;



public class TestSaveManager : MonoBehaviour
{
 

    [SerializeField]
    private Dictionary<int, ItemData> itemDic = new Dictionary<int, ItemData>();

    private PlayerController pCon;
    private Inventory inven;

    //private Chest[] chests;

    private void Awake()
    {
        //chests = FindObjectOfType<Chest>();
    }

    private void Start()
    {
        pCon = GameManager.Instance.Player;
        inven = GameManager.Instance.Inven;
        LoadAllItemData();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Save();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            Load();
        }
    }

    //파일 저장하기
    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat",
                FileMode.Create); //세이브할 데이터 오픈시 있다면 오픈 없다면 새로  생성

            TestSaveData data = new TestSaveData();

            SaveBags(data);

            SavePlayer(data);

            bf.Serialize(file, data); //데이터 직렬화 시키기

            file.Close(); //save를 위해서 파일 닫아주기
        }
        catch(System.Exception e)
        {
            //에러 발생시
            Debug.Log(e);
          
        }
    }

    //플레이어 저장
    private void SavePlayer(TestSaveData data)
    {
        data.MyPlayerData = new PlayerDataGroup(pCon.Level,
           pCon.Exp,  pCon.Hp,  pCon.Mp, pCon.Power,
           pCon.DEF, pCon.Int, pCon.StatPoint, pCon.gameObject.transform.position );
    }

    private void SaveBags(TestSaveData data)
    {
       // for(int i = 0; i<)
    }

    ////상자 저장
    //private void SaveChests(TestSaveData data)
    //{
    //    for(int i = 0; i< chests.Length; i++)
    //    {
    //        data.MyChestData.Add(new ChestData(chests[i].name));

    //        foreach(Item item in chests[i].items)
    //        {
    //            if(chests[i].items.Count > 0)
    //            {
    //                data.MyChestData[i].MyItems.Add(new ItemDataGroup(item.MyTitle,
    //                    item.MySlot.MyItems.Count, item.MySlot.MyIndex));
    //            }
    //        }
    //    }
    //}

    // 저장 파일 불러오기


    //저장한 데이터 로드하기
    private void Load()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat",
                FileMode.Open); //로드할 데이터 오픈

            TestSaveData data = (TestSaveData)bf.Deserialize(file);
          
            file.Close(); //파일 닫기

            LoadPlayer(data);
        }
        catch (System.Exception)
        {
            //에러 발생시


        }
    }

    //플레이어 정보 불러오기
    private void LoadPlayer(TestSaveData data)
    {
        pCon.statusInit(data.MyPlayerData.MyLevel,
          data.MyPlayerData.MyExp, data.MyPlayerData.MyHp, data.MyPlayerData.MyMp,
          data.MyPlayerData.MyPower, data.MyPlayerData.MyInt, data.MyPlayerData.MyDef,
          data.MyPlayerData.MyStatPoint);

        pCon.gameObject.transform.position = new Vector3(data.MyPlayerData.myX, data.MyPlayerData.myY,
            data.MyPlayerData.myZ);
     
    }

    //모든 아이템 정보 불러오기
    private void LoadAllItemData()
    {
        ItemData[] datas = Resources.LoadAll<ItemData>("");
        foreach (ItemData data in datas)
        {
            itemDic.Add(data.ID, data);
            
        }
    }

    //public void LoadBags(TestSaveData data)
    //{
    //    foreach(BagData bag in data.MyInventoryData.MyBags)
    //    {
    //        Bag newBag = (Bag)Instantiate(items[0]); //아이템's에 있는 아이템을 찾아서 생성하기
    //        newBag.Initialize(bag.MysSlotCount);

    //        InventoryScript.MyInstance.AddBag(newBag, bag.MyBagIndex);
    //    }
    //}

    //상자 정보 불러오기
    //private void LoadChest(TestSaveData data)
    //{
    //    foreach(ChestData chest in data.MyChestData)
    //    {
    //        Chest c = Array.Find(chests x => x.name == chest.MyName);

    //        foreach(ItemDataGroup items in chest.MyItems)
    //        {
    //            Item item = Array.Find(items, x => x.myTitle == items.MyTitle);
    //            item.MySlot = c.MyBag.MySlots.Find(x => x.MyIndex = items.MySlotIndex);
    //            c.MyItems.Add(item);
    //        }
    //    }
    //}
}
