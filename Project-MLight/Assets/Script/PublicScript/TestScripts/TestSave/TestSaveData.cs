using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//세이브할 데이터 보유
[System.Serializable]
public class TestSaveData
{
    public PlayerDataGroup MyPlayerData { get; set; }

    public InventoryData MyInventoryData { get; set; }
    //public List<ChestData> MyChestData { get; set; }

    public TestSaveData()
    {
        //MyChestData = new List<ChestData>();
    }
   
}

[System.Serializable]
public class PlayerDataGroup
{
    public int MyLevel { get; set; }

    public int MyExp { get; set; }

    public int MyHp { get; set; }

    public int MyMp { get; set; }

    public int MyPower { get; set; }

    public int MyDef { get; set; }

    public int MyInt { get; set; }

    public int MyStatPoint { get; set; }

    public float myX { get; set; }
    public float myY { get; set; }
    public float myZ { get; set; }

    public PlayerDataGroup(int level, int exp, int health,
        int mp, int power, int def, int intel, int statPoint, Vector3 position)
    {
        this.MyLevel = level;
        this.MyExp = exp;
       
        this.MyHp = health;
    
        this.MyMp = mp;
  
        this.MyPower = power;
        this.MyDef = def;
        this.MyInt = intel;
        this.MyStatPoint = statPoint;
        this.myX = position.x;
        this.myY = position.y;
        this.myZ = position.z;
    }
}

[System.Serializable]
public class ItemDataGroup
{
    public string MyTitle { get; set; }

    public int MyStackCount { get; set; }

    public int MySlotIndex { get; set; }

    public ItemDataGroup(string title, int stackCount= 0, int slotIndex = 0)
    {
        MyTitle = title;
        MyStackCount = stackCount;
        MySlotIndex = slotIndex;
    }
}

[SerializeField]
public class InventoryData
{
    public List<BagData> MyBags { get; set; }

    public InventoryData()
    {
        MyBags = new List<BagData>();
    }
}

[SerializeField]
public class BagData
{
    public int MysSlotCount { get; set; }
    public int MyInvenIndex { get; set; }

    public BagData(int count , int index)
    {
        MysSlotCount = count;
        MyInvenIndex = index;
    }
}

public class SaveData
{

}

//[System.Serializable]
//public class ChestData
//{
//    public string Myname { get; set; }

//    public List<ItemDataGroup> MyItems { get; set; }

//    public ChestData(string name)
//    {
//        Myname = name;

//        MyItems = new List<ItemDataGroup>();
//    }
//}

