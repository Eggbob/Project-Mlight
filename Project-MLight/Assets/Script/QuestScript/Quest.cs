using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public enum QuestState
    {
        None = 0,
        Start,
        Progressing,
        Complete,
        InActive,
        Done
    }

    public int ID => _id;
    public string Title => _title;
    public string Info => _info;
    public string Contents => _contents;
    public QuestRewards Rewards => rewards;
    public QuestState qState; //퀘스트 진행 상황
    public QuestGiverController qGiver; //퀘스트 NPC

    [SerializeField] protected int _id; //퀘스트 아이디
    [SerializeField] protected string _title; //퀘스트 타이틀
    [SerializeField] [TextArea(2, 6)] protected string _info; //퀘스트 정보
    [SerializeField] [TextArea(2, 6)] protected string _contents; //퀘스트 내용
    [SerializeField] protected QuestRewards rewards; //퀘스트 보상


    public abstract bool IsComplete();
    
 }

//퀘스트 보상
[System.Serializable]
public class QuestRewards
{
    //보상 아이템 구조체
    [System.Serializable]
    public struct RewardItemStruct
    {
        [SerializeField]
        private ItemData rewardItem; //보상할 아이템
        [SerializeField]
        private int itmeAmount; // 아이템 갯수

        public ItemData RewardItem { get => rewardItem; }
        public int ItmeAmount { get => itmeAmount; }
    }

    public int RewardGold => _rewardGold;

    public int RewardExp => _rewardExp;

    public RewardItemStruct[] RewardItems => _rewardItems;

    [SerializeField]
    protected RewardItemStruct[] _rewardItems;

    [SerializeField]
    protected int _rewardGold;

    [SerializeField]
    protected int _rewardExp; 
  

    //보상 부여
    public bool RewardRoutine()
    {
        int isRewarded = 0;

        //보상 아이템이 존재한다면
        if(!_rewardItems.Equals(null))
        {
            foreach(RewardItemStruct ritem in _rewardItems)
            {
                isRewarded = GameManager.Instance.Inven.Add(ritem.RewardItem, ritem.ItmeAmount);
            }
        }

        GameManager.Instance.Player.ExpGetRoutine(RewardExp);

        GameManager.Instance.Inven.GetGold(RewardGold);


        return isRewarded != -1 ? true : false; 
    }

}
