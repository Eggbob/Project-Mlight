using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverUIManager : MonoBehaviour
{

    [Header("퀘스트 상세 정보")]

    [SerializeField]
    private Text titleTxt; //퀘스트 타이틀

    [SerializeField]
    private Text questInfoTxt; //퀘스트 정보

    [SerializeField]
    private Text questContentsTxt; //퀘스트 상세 내용

    [SerializeField]
    private Button backBtn; //뒤로가기 버튼

    [SerializeField]
    private Button acceptBtn; // 수락버튼

    [SerializeField]
    private GameObject questInfoGroup; //퀘스트 상세 내용 그룹

    [SerializeField]
    private GameObject questTitleGroup; //퀘스트 타이틀 그룹

    [SerializeField] //퀘스트 보상
    private List<QuestRewardUIManager> qRewards = new List<QuestRewardUIManager>();


    private QuestGiverController qCon; //퀘스트 부여자

    private List<QuestSlotUI> questSlots = new List<QuestSlotUI>();

    //보유중인 퀘스트 보여주기
    private void ShowQuestList() 
    {
        this.gameObject.SetActive(true);

        foreach(QuestSlotUI obj in questSlots)
        {      
            ObjectPool.ReturnQuSlot(obj);            
        }

        questInfoGroup.SetActive(false);
        questTitleGroup.SetActive(true);

        acceptBtn.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);

        foreach (Quest q in qCon.Quests)
        {
            if(!q.Equals(null))
            {
                QuestSlotUI qSlot = ObjectPool.GetQuSlot();

                Button btn = qSlot.GetComponent<Button>();

                qSlot.Init(q);

                qSlot.transform.SetParent(questTitleGroup.transform);

                btn.onClick.AddListener(() => ShowQuestInfo(q)); //클릭 리스너 등록

                questSlots.Add(qSlot);
            }         
        }
    }

    //퀘스트 상세정보 표시
    private void ShowQuestInfo(Quest quest)
    {
        questInfoGroup.SetActive(true);
        questTitleGroup.SetActive(false);

        backBtn.gameObject.SetActive(true);

        titleTxt.text = quest.Title;

        questInfoTxt.text = quest.Info;

        questContentsTxt.text = quest.Contents;


        int curIndex = 0;

        for (int i = 0; i < quest.Rewards.RewardItems.Length; i++) //보상 아이템 설정
        {
            qRewards[i].SetItem(quest.Rewards.RewardItems[i].RewardItem,
                quest.Rewards.RewardItems[i].ItmeAmount);

            qRewards[i].gameObject.SetActive(true);
            curIndex = i;
        }

        qRewards[curIndex + 1].SetGold(quest.Rewards.RewardGold);

        qRewards[curIndex + 2].SetExp(quest.Rewards.RewardExp);

        if(quest.qState.Equals(Quest.QuestState.Start)) //퀘스트 수락이 가능한 상태라면
        {
            acceptBtn.GetComponentInChildren<Text>().text = "수락하기";
            acceptBtn.onClick.AddListener(() => AcceptQuest(quest));
            acceptBtn.gameObject.SetActive(true);
        }
        else if(quest.qState.Equals(Quest.QuestState.Complete))
        {
            acceptBtn.GetComponentInChildren<Text>().text = "완료하기";
            acceptBtn.onClick.AddListener(() => CompleteQuest(quest));
            acceptBtn.gameObject.SetActive(true);
        }

    }

    //클릭 리스너 해제
    private void RemoveListner()
    {
        foreach(QuestSlotUI qSlot in questSlots)
        {
            Button btn = qSlot.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
        }
        acceptBtn.onClick.RemoveAllListeners();
    }

  

    private void AcceptQuest(Quest quest) //퀘스트 수락
    {
        QuestManager.Instance.AcceptQuest(quest);
        qCon.UpdateQuestStatus();
        Back();
    }

    private void CompleteQuest(Quest quest) //퀘스트 클리어시
    {
        if(quest.IsComplete())
        {
            for(int i = 0; i<qCon.Quests.Count; i++)
            {
                if(quest == qCon.Quests[i])
                {
                    qCon.Quests[i] = null;
                }
                 
                if(quest is CollectQuest cQuest) //수집 퀘스트일시
                {
                    foreach(ColletObject cObj in cQuest.ColletObjects )
                    {
                        GameManager.Instance.Inven.itemAddEvent -= cObj.UpdateItemAmount;
                        cObj.CompleteQuest();
                    }
                }

                else if(quest is KillQuest kQuest) //처치 퀘스트일시
                {
                    foreach(KillObject kObj in kQuest.KillObjects)
                    {
                        GameManager.Instance.Player.killAction -= kObj.UpdateKillCount;
                    }
                }

                quest.Rewards.RewardRoutine();
                quest.qState = Quest.QuestState.InActive;
                QuestManager.Instance.FinishQuest(quest);

                Back();
            }
        }
    }

    public void Back() //뒤로가기
    {
        backBtn.gameObject.SetActive(false);
        acceptBtn.gameObject.SetActive(false);
        RemoveListner();
        ShowQuestList();
    }

    public void Open(QuestGiverController _qCon) //오픈시
    {
        qCon = _qCon;
        ShowQuestList();      
    }

    public void Close() //종료시
    {
        backBtn.gameObject.SetActive(false);
        acceptBtn.gameObject.SetActive(false);
        RemoveListner();
        this.gameObject.SetActive(false);
    }


}
