using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//퀘스트 관리자
public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab; //퀘스트 프리팹

    [SerializeField]
    private Transform questParent; //퀘스트가 생성될 지역

    private SampleQuest selected; //현재 선택된 퀘스트

    [SerializeField]
    private Text questDescription; //퀘스트 설명


    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text questCountTxt; //퀘스트 갯수 카운트

    [SerializeField]
    private int maxCount;  //최대 퀘스트 갯수

    [SerializeField]
    private int currentCount; //현재 퀘스트 갯수

    private List<QuestScript> questScripts = new List<QuestScript>(); //퀘스트 슬롯 리스트


    private List<SampleQuest> quests = new List<SampleQuest>(); //퀘스트 리스트

    private static QuestLog instance; //싱글톤

    public static QuestLog MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }
            return instance;
        }
    }

    private void Start()
    {
        questCountTxt.text = currentCount + "/" + maxCount;
    }

    public void AcceptQuest(SampleQuest quest) //퀘스트 수락
    {
        if(currentCount < maxCount) //만약 현재 퀘스트 수가 max보다 적을시
        {
            currentCount++; //현재 퀘스트 수 늘리기
            questCountTxt.text = currentCount + "/" + maxCount; //퀘스트 숫자 업데이트

            //퀘스트 이벤트 등록
            foreach (CollectObjective obj in quest.MyCollectObjectives)
            {
                //  InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(obj.UpdateItemCount);

                obj.UpdateItemCount();
            }

            foreach (KillObjective obj in quest.MyKillObjectives)
            {
                //GameManager.Instance.killConfirmedEvent += new KillConfirmed(obj.UpdateKillCount);
            }

            quests.Add(quest);


            GameObject go = Instantiate(questPrefab, questParent);//퀘스트 슬롯 생성

            QuestScript qs = go.GetComponent<QuestScript>(); 
            quest.MyQuestScript = qs; //퀘스트에 자신의 퀘스트 슬롯 할당
            qs.MyQuest = quest; //퀘스트 슬롯에 자신의 퀘스트 할당


            go.GetComponentInChildren<Text>().text = quest.MyTitle; //퀘스트 타이틀 작성

            CheckCompletion(); //퀘스트 진행도 확인
        }

      
    }

    public void UpdateSelected() //현재 선택된 퀘스트 상황 업데이트
    {
        ShowDescription(selected); //퀘스트 설명 보여주기
    }

    public void ShowDescription(SampleQuest quest) //퀘스트 보여주기
    {
        if (quest != null) //퀘스트가 null이 아니라면
        {
            if (selected != null && selected != quest) 
            {
                selected.MyQuestScript.DeSelect();
            }

            string objectives = string.Empty;

            selected = quest;

            string title = quest.MyTitle;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            foreach (Objective obj in quest.MyKillObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            //퀘스트 설명 
            questDescription.text = string.Format("{0}\n{1}\nObjectives\n{2}", title, quest.MyDescription, objectives);
        }
    }

    public void CheckCompletion()//퀘스트 진행도 확인
    {
        foreach (QuestScript qs in questScripts) 
        {
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus(); //npc의 퀘스트 진행도 업데이트
            qs.IsComplete(); //
        }
    }

    public void OpenClose()//창 종료시
    {
        if(canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close() //
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    //퀘스트 포기하기
    public void AbandonQuest()
    {
        foreach (CollectObjective o in selected.MyCollectObjectives)
        {
            //  InventoryScript.MyInstance.itemCountChangedEvent -= new ItemCountChanged(obj.UpdateItemCount);
           
        }

        foreach (KillObjective o in selected.MyKillObjectives)
        {

            //GameManager.Instance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);

        }

        RemoveQuest(selected.MyQuestScript);
    }

    //퀘스트 없애기
    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        quests.Remove(qs.MyQuest);
        questDescription.text = string.Empty;
        selected = null;
        currentCount--;

        questCountTxt.text = currentCount + "/" + maxCount;
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
        qs = null;
    }

    //이미 해당 퀘스트를 가지고 있는지
    public bool HasQuest(SampleQuest quest)
    {
        return quests.Exists(x => x.MyTitle == quest.MyTitle);
    }
}
