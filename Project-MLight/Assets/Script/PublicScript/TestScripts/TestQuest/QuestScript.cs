using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//퀘스트 버튼
public class QuestScript : MonoBehaviour
{
    public SampleQuest MyQuest { get; set; }

    private bool markedComplete = false; //퀘스트가 해결되었는지


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

    //퀘스트 클리어시
    public void IsComplete()
    {
        if(MyQuest.IsComplete && !markedComplete)
        {
            markedComplete = true;
            GetComponent<Text>().text += "(C)";
            FeedManager.Instance.WriteMessage(string.Format("{0} (C)", MyQuest.MyTitle));
        }
        else if(!MyQuest.IsComplete)
        {
            markedComplete = false;
            GetComponent<Text>().text = "[" + MyQuest.MyLevel + "]"+MyQuest.MyTitle;
        }

    }


   
}
