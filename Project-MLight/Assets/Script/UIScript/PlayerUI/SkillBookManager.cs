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
    public static void AddEventListner<T>(this Button button, T param, T param2, Action<T, T> OnClick)
    {
        button.onClick.AddListener(delegate () { OnClick(param, param2); });
    }
}

public class SkillBookManager : MonoBehaviour
{
    public GameObject sButtonTemp; //스킬 버튼 템플릿
    public GameObject ActiveskillList; //액티브 스킬 리스트
    public GameObject PassiveSkillList; // 패시브 스킬 리스트

    public List<SkillQuickSlotUI> qSlotUIList = new List<SkillQuickSlotUI>();

    public List<Button> skillBtns; //플레이어 스킬 버튼 리스트

    private int clickedBtn = 0; //현재 클릭된 버튼


    [SerializeField]
    public SkillDetailPage SkillPage; // 상세 스킬 페이지
    [SerializeField]
    private Inventory inventory;


    PlayerSkillController psCon; //플레이어 스킬 컨트롤러
    

    private void Start()
    {
        psCon = GameManager.Instance.Player.psCon;       
        SkillButtonInit();
    }

    private void SkillButtonInit()//스킬 버튼 초기화
    {
        GameObject sButton;

        for(int i = 1; i< psCon.PlayerSkills.Count; i++)
        {
            switch (psCon.PlayerSkills[i].gameObject.tag) //스킬 목록 초기화
            {
                case "ActiveSkill":
                    sButton = Instantiate(sButtonTemp, ActiveskillList.transform);
                    sButton.transform.GetChild(0).GetComponent<Image>().sprite = psCon.PlayerSkills[i].Icon;
                    sButton.transform.GetChild(1).GetComponent<Text>().text = psCon.PlayerSkills[i].SkillName;
                    sButton.transform.GetChild(2).GetComponent<Text>().text = SkillMaster(psCon.PlayerSkills[i].SkillExp);
                    sButton.GetComponent<Button>().AddEventListner(i, ShowSkillpage);
                    break;

                case "PassiveSkill":
                    sButton = Instantiate(sButtonTemp, PassiveSkillList.transform);
                    sButton.transform.GetChild(0).GetComponent<Image>().sprite = psCon.PlayerSkills[i].Icon;
                    sButton.transform.GetChild(1).GetComponent<Text>().text = psCon.PlayerSkills[i].SkillName;
                    sButton.transform.GetChild(2).GetComponent<Text>().text = SkillMaster(psCon.PlayerSkills[i].SkillExp);
                    sButton.GetComponent<Button>().AddEventListner(i, ShowSkillpage);
                    break;
            }
        }

        for(int i = 0; i< skillBtns.Count; i++) //스킬 창에 있는 퀵슬롯 리스트 초기화
        {
            skillBtns[i].AddEventListner(i, SetSkill); // 퀵슬롯 버튼 클릭시 실행할 함수
            qSlotUIList[i] = skillBtns[i].GetComponent<SkillQuickSlotUI>();
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

    private void ShowSkillpage(int i) //스킬 페이지 보여주기
    {
        (int count, int index) = inventory.CountSkillBook();

        SkillPage.ShowSkillPage(psCon.PlayerSkills[i], count, cnt=> inventory.Remove(index, cnt));

        clickedBtn = i;
      
        SkillPage.gameObject.SetActive(true);
    } 

    private void SetSkill(int btnIndex) //스킬 설정하기
    {
        if (PassiveSkillList.activeSelf)
        { return; }

        if(clickedBtn != 0)
        {
            qSlotUIList[btnIndex].SetSkill(psCon.GetSkill(clickedBtn));
        }
    }

}
