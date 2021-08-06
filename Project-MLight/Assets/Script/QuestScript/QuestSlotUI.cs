using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUI : MonoBehaviour
{
    [SerializeField]
    private Quest quest; //슬롯이 가지고 있는 퀘스트

    [SerializeField]
    private Text titleTxt; //퀘스트 타이틀

    [SerializeField]
    private Text qStateTxt; //퀘스트 진행도 

    [SerializeField]
    private GameObject clickedImg; //클릭 이미지

    public Quest SlotQuest => quest;

 
    public void Init(Quest _quest) //초기설정
    {
        this.quest = _quest;

        transform.SetAsFirstSibling();

      //  quest.qState = Quest.QuestState.Progressing;

        titleTxt.text = quest.Title;

        SetQState();

        clickedImg.SetActive(false);
    }

    public void SetQState() //퀘스트 진행도 표시
    {
        switch(quest.qState)
        {
            case Quest.QuestState.Start:
                qStateTxt.text = "수락가능";
                break;

            case Quest.QuestState.Progressing:
                qStateTxt.text = "진행중";
                break;

            case Quest.QuestState.Complete:
                qStateTxt.text = "완료가능";
                break;

            case Quest.QuestState.InActive:
                qStateTxt.text = "완료";
                break;
        }
    }

    //클릭 이미지 표시
    public void ShowClickedImg(bool show)
    {
        if (!this.gameObject.activeSelf) return;


        if (show)
            clickedImg.SetActive(true);
        else
            clickedImg.SetActive(false);
    }

    //완료확인
    public void CheckComplete()
    {
        if(!quest.qState.Equals(Quest.QuestState.Complete) && !quest.qState.Equals(Quest.QuestState.InActive)
            &&quest.IsComplete())
        {
            quest.qState = Quest.QuestState.Complete;
            SetQState();
        }
    }
}
