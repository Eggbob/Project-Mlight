using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfoUIManager : MonoBehaviour
{
    [SerializeField]
    private Text titleTxt; //퀘스트 타이틀

    [SerializeField]
    private Text questInfoTxt; //퀘스트 정보

    [SerializeField]
    private Text questContentsTxt; //퀘스트 상세 내용

    [SerializeField] //퀘스트 보상
    private List<QuestRewardUIManager> qRewards = new List<QuestRewardUIManager>();


    public void SetQuestInfo(Quest quest)//퀘스트 정보 지정
    {
        titleTxt.text = quest.Title;

        questInfoTxt.text = "퀘스트 임무 : " + quest.Info;

        questContentsTxt.text = quest.Contents;


        int curIndex = 0;

        for(int i= 0; i<quest.Rewards.RewardItems.Length; i++) //보상 아이템 설정
        {
            qRewards[i].SetItem(quest.Rewards.RewardItems[i].RewardItem, 
                quest.Rewards.RewardItems[i].ItmeAmount);

            qRewards[i].gameObject.SetActive(true);
            curIndex = i;
        }

        qRewards[curIndex + 1].SetGold(quest.Rewards.RewardGold);

        qRewards[curIndex + 2].SetExp(quest.Rewards.RewardExp);

        this.gameObject.SetActive(true); 

    }

}
