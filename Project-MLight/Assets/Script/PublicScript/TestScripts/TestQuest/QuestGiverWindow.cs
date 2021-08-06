using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Npc 퀘스트 리스트 보여주기
public class QuestGiverWindow : Window
{
    private static QuestGiverWindow instance;

    [SerializeField]
    private GameObject backBtn, acceptBtn; //수락 및 뒤로가기 버튼

    [SerializeField]
    private GameObject questDescription; //퀘스트 내용

    [SerializeField]
    private GameObject completeBtn;

    private QuestGiver questGiver; //퀘스트 NPC

    [SerializeField]
    private GameObject questPrefab; //퀘스트 프리팹

    [SerializeField]
    private Transform questArea; //퀘스트 프리팹 생성할곳

    private List<GameObject> quests = new List<GameObject>();

    private SampleQuest selectedQuest;

    public static QuestGiverWindow MyInstance 
    {   get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }
            return instance;
        }
    
    }

    public void ShowQuests(QuestGiver _questGiver)
    {
        this.questGiver = _questGiver;

        foreach(GameObject go in quests)
        {
            Destroy(go);
        }

        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);

        foreach (SampleQuest quest in questGiver.MyQuests)
        {
            if(quest != null)
            {
                GameObject go = Instantiate(questPrefab, questArea);
                go.GetComponent<Text>().text = "["+ quest.MyLevel +"]"+quest.MyTitle;

                go.GetComponent<QGQuesteScript>().MyQuest = quest;

                quests.Add(go);

                if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
                {
                    go.GetComponent<Text>().text += "(C)";
                }

                else if (QuestLog.MyInstance.HasQuest(quest))
                {
                    Color c = go.GetComponent<Text>().color;

                    c.a = 0.5f;

                    go.GetComponent<Text>().color = c;
                }
            }

           
        }
    }

    public override void Open(NpcController npc)
    {
        ShowQuests((npc as QuestGiver));
        base.Open(npc);
    }

    public void ShowQuestInfo(SampleQuest quest)
    {
        this.selectedQuest = quest;

        if(QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if(!QuestLog.MyInstance.HasQuest(quest))
        {
            acceptBtn.SetActive(true);
        
        }

        backBtn.SetActive(true);

        acceptBtn.SetActive(true);
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        string description = quest.MyDescription;

        string objectives = string.Empty;

        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }

        //퀘스트 설명 
        questDescription.GetComponent<Text>().text = string.Format("{0}\n{1}\n", quest.MyTitle, quest.MyDescription);
    
    }

    //뒤로가기
    public void Back()
    {
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);
        ShowQuests(questGiver);
        completeBtn.SetActive(false);
    }

    //수락 버튼
    public void Accept()
    {
        QuestLog.MyInstance.AcceptQuest(selectedQuest);
        Back();
    }

    public override void Close()
    {
        completeBtn.SetActive(false);
        base.Close();
    }

    //퀘스트 클리어시
    public void CompleteQuest()
    {
        if(selectedQuest.IsComplete)
        {
            for(int i = 0; i<questGiver.MyQuests.Length; i++)
            {
                if(selectedQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyQuests[i] = null;
                }
            }
            
            foreach(CollectObjective o in selectedQuest.MyCollectObjectives)
            {
                //  InventoryScript.MyInstance.itemCountChangedEvent -= new ItemCountChanged(obj.UpdateItemCount);
                o.Complete(); //아이템 지우기
            }

            foreach (KillObjective o in selectedQuest.MyKillObjectives)
            {
             
                //GameManager.Instance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
                
            }

            QuestLog.MyInstance.RemoveQuest(selectedQuest.MyQuestScript);

            Back();
        }
    }
}
