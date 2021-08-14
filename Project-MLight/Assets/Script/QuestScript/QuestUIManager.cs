﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestUIManager : MonoBehaviour
{
    private List<Quest> quests = new List<Quest>();

    private List<QuestSlotUI> questSlots = new List<QuestSlotUI>();

    [SerializeField]
    private QuestInfoUIManager qInfoManager;

    [SerializeField]
    private Transform questParent; //퀘스트가 생성될 지역

    private QuestSlotUI prevClickedQuest; //이전 클릭한 퀘스트 슬롯
    private QuestSlotUI beginClickedQuest; //현재 클릭한 퀘스트 슬롯

    private static QuestUIManager instance; //싱글톤

    public static QuestUIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestUIManager>();
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
        }

        //처치 퀘스트 일시
        else if(quest is KillQuest kQuest)
        {
            foreach(var obj in kQuest.KillObjects)
            {
                GameManager.Instance.Player.killAction += obj.UpdateKillCount;
            }        
        }

        quest.qState = Quest.QuestState.Progressing;

        slot.gameObject.transform.SetParent(questParent);

        slot.Init(quest);

        slot.GetComponent<Button>().onClick.AddListener(() => ShowQuestInfo(slot));

        quests.Add(quest); 

        questSlots.Add(slot);
 
        CheckComplete();
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


    public void Close() //UI종료시
    {
        if(prevClickedQuest != null && beginClickedQuest != null)
        {
            prevClickedQuest.ShowClickedImg(false);
            beginClickedQuest.ShowClickedImg(false);
        }

      
        prevClickedQuest = null;
        beginClickedQuest = null;       
    }
}