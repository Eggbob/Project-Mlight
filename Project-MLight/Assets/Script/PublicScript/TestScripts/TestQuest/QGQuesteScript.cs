using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//npc 퀘스트 보여주기
public class QGQuesteScript : MonoBehaviour
{
    public SampleQuest MyQuest { get; set; }

    public void Select()
    {
        QuestGiverWindow.MyInstance.ShowQuestInfo(MyQuest);
    }
}
