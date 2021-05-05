using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;


public static class ButtonPressed
{
    public static void AddEventListner<T> (this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate () { OnClick(param);  });
    }
}

public class SkillBookManager : MonoBehaviour
{
    public GameObject sButtonTemp; //스킬 버튼 템플릿
    public GameObject ActiveskillList; //액티브 스킬 리스트
    public GameObject PassiveSkillList; // 패시브 스킬 리스트
    public GameObject SkillPage; // 상세 스킬 페이지
    PlayerSkillController psCon; //플레이어 스킬 컨트롤러
    

    private void Start()
    {
        psCon = PlayerSkillController.instance;
        SkillButtonInit();
    }

    private void SkillButtonInit()//스킬 버튼 초기화
    {
        GameObject sButton;

        for(int i = 0; i< psCon.PlayerSkills.Count; i++)
        {
            switch (psCon.PlayerSkills[i].sType)
            {
                case Skill.SkillType.Active:
                    sButton = Instantiate(sButtonTemp, ActiveskillList.transform);
                    sButton.transform.GetChild(0).GetComponent<Image>().sprite = psCon.PlayerSkills[i].Icon;
                    sButton.transform.GetChild(1).GetComponent<Text>().text = psCon.PlayerSkills[i].SkillName;
                    sButton.transform.GetChild(2).GetComponent<Text>().text = SkillMaster(psCon.PlayerSkills[i].SkillExp);
                    sButton.GetComponent<Button>().AddEventListner(i, ShowSkillpage);
                    break;

                case Skill.SkillType.Passive:
                    sButton = Instantiate(sButtonTemp, PassiveSkillList.transform);
                    sButton.transform.GetChild(0).GetComponent<Image>().sprite = psCon.PlayerSkills[i].Icon;
                    sButton.transform.GetChild(1).GetComponent<Text>().text = psCon.PlayerSkills[i].SkillName;
                    sButton.transform.GetChild(2).GetComponent<Text>().text = SkillMaster(psCon.PlayerSkills[i].SkillExp);
                    sButton.GetComponent<Button>().AddEventListner(i, ShowSkillpage);
                    break;
            }

            
        }

        PassiveSkillList.SetActive(false);
    }

    public string SkillMaster(float exp)//스킬 숙련도 나타내기
    {
        if (exp >= 0 && exp < 100)
        {
           return "초급";
        }
        else if (exp >= 100 && exp < 200)
        {
            return "중급";
        }
        else if (exp >= 200 && exp < 300)
        {
            return "고급";
        }
        else
        {
            return "";
        }
    }

    private void ShowSkillpage(int i)
    {
        SkillPage.transform.GetChild(0).GetComponent<Text>().text = psCon.PlayerSkills[i].SkillName;
        SkillPage.transform.GetChild(1).GetComponent<Text>().text = "MP 소모량 : " + psCon.PlayerSkills[i].MpCost.ToString();
        SkillPage.transform.GetChild(2).GetComponent<Text>().text = "재사용 대기시간 : " + psCon.PlayerSkills[i].coolTime.ToString();
        SkillPage.transform.GetChild(3).GetComponent<Text>().text = psCon.PlayerSkills[i].Description;

        SkillPage.SetActive(true);
    }
}
