using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private Quest[] quests; //퀘스트 리스트

    //Debugging
    [SerializeField]
    private QuestLog tmpLog; //퀘스트 로그

    private void Awake()
    {
        tmpLog.AcceptQuest(quests[0]);
        //tmpLog.AcceptQuest(quests[1]);
    }
}
