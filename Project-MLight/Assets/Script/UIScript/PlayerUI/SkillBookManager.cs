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

    public List<SkillQuickSlotUI> qSlotUIList = new List<SkillQuickSlotUI>(); //퀵슬롯 

    public List<Button> skillBtns; //플레이어 스킬 목록 리스트

    [SerializeField]
    public SkillDetailPage SkillPage; // 상세 스킬 페이지


    private PlayerSkillController psCon; //플레이어 스킬 컨트롤러
    private Inventory inventory;
    private int clickedBtn = 0; //현재 클릭된 버튼

    private void Start()
    {
        psCon = GameManager.Instance.Player.psCon;
        inventory = GameManager.Instance.Inven;
        SkillButtonInit();
    }

    private void SkillButtonInit()//스킬 버튼 초기화
    {
        GameObject sButton = null;

        for(int i = 1; i< psCon.PlayerSkills.Count; i++)
        {
            switch (psCon.PlayerSkills[i].gameObject.tag) //스킬 목록 초기화
            {
                case "ActiveSkill":
                    sButton = Instantiate(sButtonTemp, ActiveskillList.transform);
                    break;

                case "PassiveSkill":
                    sButton = Instantiate(sButtonTemp, PassiveSkillList.transform);
                    break;
            }

            sButton.transform.GetChild(0).GetComponent<Image>().sprite = psCon.PlayerSkills[i].Icon;
            sButton.transform.GetChild(1).GetComponent<Text>().text = psCon.PlayerSkills[i].SkillName;
            sButton.transform.GetChild(2).GetComponent<Text>().text = SkillMaster(psCon.PlayerSkills[i].SkillExp);
            sButton.GetComponent<Button>().AddEventListner(i, ShowSkillpage);
            sButton.GetComponent<Button>().onClick.AddListener(()=>BgmManager.Instance.PlayEffectSound("Button"));
        }

        for (int i = 0; i< skillBtns.Count; i++) //스킬 창에 있는 퀵슬롯 리스트 초기화
        {
            int btnindex = i;
            skillBtns[i].onClick.AddListener(() => SetSkill(btnindex, clickedBtn));
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

    public void SetSkill(int btnIndex, int skillIndex) //스킬 설정하기
    {
        if (PassiveSkillList.activeSelf)
        { return; }

        if (skillIndex.Equals(0))
        { return; }


        for (int i = 0; i< qSlotUIList.Count; i++)
        {
            if (qSlotUIList[i].slotSkill == psCon.GetSkill(skillIndex))
            { return; }
        }

        qSlotUIList[btnIndex].SetSkill(psCon.GetSkill(skillIndex), btnIndex);
        
    }

}
