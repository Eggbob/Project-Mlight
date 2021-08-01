using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    public Quest MyQuest { get; set; }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    //퀘스트 클릭시
    public void Select()
    {
        GetComponentInChildren<Text>().color = Color.red;
        QuestLog.MyInstance.ShowDescription(MyQuest);
    }

    //퀘스트 클릭해제시
    public void DeSelect()
    {
        GetComponentInChildren<Text>().color = Color.white;
    }
}
