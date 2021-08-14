using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int saveLevel;

    public int saveExp;

    public int saveHp;

    public int saveMp;

    public int savePower;

    public int saveDef;
    public int saveInt;

    public int saveStatPoint;

    public int saveGold;

    public Dictionary<int, int> saveItems = new Dictionary<int, int>(); //아이템 아이디 ,아이템 수량 저장

    public List<int> equipItems = new List<int>();

    // public List<ItemData> equipItems = new List<ItemData>();

    public Vector3 savePos;


    public PlayerData(int level, int exp, int health,
        int mp, int power, int def, int intel, int statPoint, Vector3 position)
    {
        this.saveLevel = level;
        this.saveExp = exp;

        this.saveHp = health;

        this.saveMp = mp;

        this.savePower = power;
        this.saveDef = def;
        this.saveInt = intel;
        this.saveStatPoint = statPoint;
        savePos = position;    
    }
}
