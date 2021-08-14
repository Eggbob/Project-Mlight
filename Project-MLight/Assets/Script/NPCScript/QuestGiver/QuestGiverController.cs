using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiverController : NpcController
{
    public Quest[] Quests => quests;

    [SerializeField]
    private Quest[] quests; //퀘스트 

    [SerializeField]
    private QuestGiverUIManager questGiverManger;

    [SerializeField]
    private GameObject questionMark; //물음표 마크

    [SerializeField]
    private GameObject exclamationMark;//느낌표 마크


    private void Awake()
    {
        Init();  
    }

    private void Start()
    {
        UpdateQuestStatus();
    }

    private void Init()
    {
        foreach(Quest quest in quests)
        {
            quest.qGiver = this;
        }
    }

    //퀘스트 진행도 업데이트
    public void UpdateQuestStatus()
    {
        foreach(Quest quest in quests)
        {
            if(quest != null)
            {
                if(quest.IsComplete() && QuestUIManager.Instance.HasQuest(quest))
                {
                    questionMark.SetActive(true);
                    exclamationMark.SetActive(false);
                    break;
                }
                else if(!QuestUIManager.Instance.HasQuest(quest))
                {
                    exclamationMark.SetActive(true);
                    questionMark.SetActive(false);
                    break;
                }
                else if(!quest.IsComplete() && QuestUIManager.Instance.HasQuest(quest))
                {
                    exclamationMark.SetActive(false);
                    questionMark.SetActive(false);
                    break;
                }
                else
                {
                    exclamationMark.SetActive(false);
                    questionMark.SetActive(false);
                    break;
                }
            }
        }
    }

    //접촉시에
    public override void Interact()
    {
        if (!IsInteracting)
        {
            base.Interact();
            UpdateQuestStatus();
            questGiverManger.Open(this);
        }

    }

    //접촉종료시에
    public override void StopInteract()
    {
        if (IsInteracting)
        {
            base.StopInteract();
            questGiverManger.Close();
        }
    }
}
