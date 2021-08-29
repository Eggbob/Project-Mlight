using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestManager : MonoBehaviour
{
    private List<Quest> quests = new List<Quest>(); //플레이어가 가지고 있는 퀘스트

    private List<QuestSlotUI> questSlots = new List<QuestSlotUI>();

    [SerializeField]
    private QuestInfoUIManager qInfoManager;

    [SerializeField]
    private Transform questParent; //퀘스트가 생성될 지역

    private QuestSlotUI prevClickedQuest; //이전 클릭한 퀘스트 슬롯
    private QuestSlotUI beginClickedQuest; //현재 클릭한 퀘스트 슬롯

    private static QuestManager instance; //싱글톤

    public List<Quest> Quests => quests;
    public List<QuestSlotUI> Slots => questSlots;


    public static QuestManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }


    private void HighlightImg() //하이라이트 이미지 활성화 유무
    {
        //이전 클릭슬롯이 없고 현재 클릭슬롯이 있다면
        if (prevClickedQuest == null && beginClickedQuest != null)
        {
            beginClickedQuest.ShowClickedImg(true);
            prevClickedQuest = beginClickedQuest;
        }
        //이전 클릭슬롯과 현재 클릭슬롯이 동일하다면
        else if (prevClickedQuest == beginClickedQuest)
        {
            return;
        }
        //이전 클릭슬롯과 현재 클릭슬롯이 동일하지 않다면
        else if (prevClickedQuest != beginClickedQuest)
        {
            prevClickedQuest.ShowClickedImg(false);
            beginClickedQuest.ShowClickedImg(true);
            prevClickedQuest = beginClickedQuest;
        }
    }

    //퀘스트 정보 보여주기
    private void ShowQuestInfo(QuestSlotUI qSlotUI)
    {
        if (qSlotUI == null)
            return;

        beginClickedQuest = qSlotUI;

        HighlightImg();
        qInfoManager.SetQuestInfo(qSlotUI.SlotQuest);
    }


    //퀘스트 수락시
    public void AcceptQuest(Quest quest)
    {
        QuestSlotUI slot = ObjectPool.GetQuSlot();

        //수집 퀘스트 일시
        if(quest is CollectQuest cQuest)
        {
            foreach(var obj in cQuest.ColletObjects)
            {
                GameManager.Instance.Inven.itemAddEvent += obj.UpdateItemAmount;
            }
            var acceptQuest = ScriptableObject.CreateInstance<CollectQuest>();
            acceptQuest = cQuest;
            quests.Add(acceptQuest);
        }

        //처치 퀘스트 일시
        else if(quest is KillQuest kQuest)
        {
            foreach(var obj in kQuest.KillObjects)
            {
                GameManager.Instance.Player.killAction += obj.UpdateKillCount;
            }

            var acceptQuest = ScriptableObject.CreateInstance<KillQuest>();
            acceptQuest = kQuest;
            quests.Add(acceptQuest);
        }

        quest.qState = Quest.QuestState.Progressing;

        slot.gameObject.transform.SetParent(questParent);

        slot.Init(quest);

        slot.GetComponent<Button>().onClick.AddListener(() => ShowQuestInfo(slot));; 
      
        questSlots.Add(slot);
 
        //CheckComplete();
    }

    public void CheckComplete() //퀘스트 진행도 확인
    {
        foreach (QuestSlotUI qs in questSlots)
        {
            qs.SlotQuest.qGiver.UpdateQuestStatus(); //npc의 퀘스트 진행도 업데이트
            qs.CheckComplete(); //퀘스트의 완료 확인
        }
    }

    public bool HasQuest(Quest quest) //해당 퀘스트를 가지고 있는지
    {
        if (quests.Count > 0)
        {
            return quests.Exists(x => x.ID == quest.ID);
        }
        else
            return false;
        
    }

    public void FinishQuest(Quest quest) //퀘스트 클리어
    {     
        for(int i = 0; i< questSlots.Count; i++)
        {
            if (questSlots[i].SlotQuest.ID.Equals(quest.ID))
            {
                questSlots[i].SlotQuest.qState = Quest.QuestState.InActive;
                questSlots[i].SetQState();
            }
        }

        foreach (Quest qs in quests)
        {
           if(qs.ID.Equals(quest.ID))
           {
                qs.qState = Quest.QuestState.InActive;
           }
        }
    }

    public void Close() //UI종료시
    {
        if(!prevClickedQuest.Equals(null) && !beginClickedQuest.Equals(null))
        {
            prevClickedQuest.ShowClickedImg(false);
            beginClickedQuest.ShowClickedImg(false);
        }

      
        prevClickedQuest = null;
        beginClickedQuest = null;       
    }
}
