using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRewardUIManager : MonoBehaviour
{
 
    public Sprite goldSprite; //골드 스프라이트

    public Sprite expSprite; //경험치 스프라이트

    [SerializeField]
    private Image rewardImg; //보상 이미지

    [SerializeField]
    private Text amountTxt;// 수량 텍스트


    public void SetGold(int goldAmount)//골드 보상정하기
    {
        rewardImg.sprite = goldSprite;

        amountTxt.text = goldAmount.ToString();

        this.gameObject.SetActive(true);
    }

    public void SetExp(int expAmount)//경험치 보상 정하기
    {
        rewardImg.sprite = expSprite;

        amountTxt.text = expAmount.ToString();

        this.gameObject.SetActive(true);
    }

    public void SetItem(ItemData data, int amount)//아이템 보상정하기
    {
        rewardImg.sprite = data.IconSprite;

        amountTxt.text = amount.ToString();

        this.gameObject.SetActive(true);
    }

}
