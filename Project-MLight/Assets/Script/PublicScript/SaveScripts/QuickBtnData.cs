using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickBtnData 
{
    public Dictionary<int, int> itemQuickSlot = new Dictionary<int, int>(); //퀵슬롯 인덱스, 아이템 아이디 저장

    public Dictionary<int, int> skillQuickSlot = new Dictionary<int, int>(); //퀵슬롯 인덱스, 스킬 인덱스

    public string itemQuick;

    public string skillQuick;

    public void DictionaryToJson()
    {
        itemQuick = JsonUtility.ToJson(new Serializtion<int, int>(itemQuickSlot));
        skillQuick = JsonUtility.ToJson(new Serializtion<int, int>(skillQuickSlot));
    }

    public void JsonToDictionary()
    {
        itemQuickSlot = JsonUtility.FromJson<Serializtion<int, int>>(itemQuick).ToDictionary();

        skillQuickSlot = JsonUtility.FromJson<Serializtion<int, int>>(skillQuick).ToDictionary();
    }

}

