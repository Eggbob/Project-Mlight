using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NpcController
{
    [SerializeField]
    private SampleQuest[] quests; //퀘스트 리스트

    [SerializeField]
    private Sprite question, questionSilver, exclemation; //물음표 표시

    private SpriteRenderer statusRenderer;

    public SampleQuest[] MyQuests { get => quests; set => quests = value; }


    private void Start()
    {
        foreach(SampleQuest quest in quests)
        {
            quest.MyQuestGiver = this;
        }
    }

    public void UpdateQuestStatus()
    {
        int count = 0;

        foreach(SampleQuest quest in quests)
        {
            if(quest != null)
            {
                if(quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    break;
                }
                else if(!QuestLog.MyInstance.HasQuest(quest)) //퀘스트를 가지고 있지 않다면
                {
                    statusRenderer.sprite = exclemation;
                    break;
                }
                else if(!quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                }            
            }
            else
            {
                count++;

                if(count == quests.Length)
                {
                    statusRenderer.enabled = false;
                }
            }
        }
    }
}
