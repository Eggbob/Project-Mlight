using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiverController : NpcController
{
    public List<Quest> Quests => quests;

    private QuestManager qManager;

    [SerializeField]
    private List<Quest> quests = new List<Quest>(); //퀘스트 

    [SerializeField]
    private Quest[] originalQuests; //퀘스트 원본

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
        qManager = QuestManager.Instance;
        UpdateQuestStatus();    
    }

    private void Init()
    {
        foreach (Quest quest in originalQuests)
        {
            if(quest is KillQuest)
            {
                Quest giverQuest = ScriptableObject.CreateInstance<KillQuest>();
                giverQuest = quest;

                giverQuest.qGiver = this;
                quests.Add(giverQuest);              
            }

            else if(quest is CollectQuest)
            {
                Quest giverQuest = ScriptableObject.CreateInstance<CollectQuest>();
                giverQuest = quest;

                giverQuest.qGiver = this;
                quests.Add(giverQuest);
            }
        }
      
    }

    //퀘스트 진행도 업데이트
    public void UpdateQuestStatus()
    {
        int completeCnt = 0;
        int startCnt = 0;

        foreach(Quest quest in quests)
        {
            if(quest != null)
            {
                if (quest.qState == Quest.QuestState.Complete)
                {
                    completeCnt++;                
                }
                else if (quest.qState == Quest.QuestState.Start)
                {
                    startCnt++;
                }               
            }
        }

        if(completeCnt > 0)
        {
            questionMark.SetActive(true);
            exclamationMark.SetActive(false);
            return;
        }
        else if(startCnt > 0)
        {
            exclamationMark.SetActive(true);
            questionMark.SetActive(false);
            return;
        }
        else
        {
            exclamationMark.SetActive(false);
            questionMark.SetActive(false);
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
            UpdateQuestStatus();
            questGiverManger.Close();
        }
    }
}
