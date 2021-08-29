using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
   //public Quest sampleQuest;
    public List<Quest> playerQuests = new List<Quest>();

    public Dictionary<int, int> questStates = new Dictionary<int, int>(); //퀘스트 아이디, 퀘스트 진행도

    //public Dictionary<int, List<int>> questObjects = new Dictionary<int, List<int>>();//퀘스트 아이디, 퀘스트 목표

    public string qStates;

    public string qObjects;

    public void DictionaryToJson()
    {
        qStates = JsonUtility.ToJson(new Serializtion<int, int>(questStates));
       // qObjects = JsonUtility.ToJson(new Serializtion<int, List<int>>(questObjects));
    }

    public void JsonToDictionary()
    {
        questStates = JsonUtility.FromJson<Serializtion<int, int>>(qStates).ToDictionary();

       // questObjects = JsonUtility.FromJson<Serializtion<int, List<int>>>(qObjects).ToDictionary();
    }
}
