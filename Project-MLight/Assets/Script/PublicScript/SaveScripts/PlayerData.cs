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

    public string skillStr;
    public string itemStr;
   
    public List<int> saveItemList = new List<int>();
    public List<int> equipItems = new List<int>();
 
    public Dictionary<int, int> saveItems = new Dictionary<int, int>(); //아이템 아이디 ,아이템 수량 저장
    public Dictionary<int, float> saveSkills = new Dictionary<int, float>(); 

    public Vector3 savePos;

    public void DictionaryToJson()
    {
        itemStr = JsonUtility.ToJson(new Serializtion<int, int>(saveItems));
        skillStr = JsonUtility.ToJson(new Serializtion<int, float>(saveSkills));
    }

    public void JsonToDictionary()
    {
        saveItems = JsonUtility.FromJson<Serializtion<int, int>>(itemStr).ToDictionary();

        saveSkills = JsonUtility.FromJson<Serializtion<int, float>>(skillStr).ToDictionary();
    }
   
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


public class Serializtion<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> keys;

    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() { return target; }

    public Serializtion(Dictionary<TKey, TValue>target)
    {
        this.target = target;
    }

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Mathf.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for(var i = 0; i<count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }

}