using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDetailPage : MonoBehaviour
{
    #region
    [SerializeField]
    private Text sNameTxt;
    [SerializeField]
    private Text sExpTxt;
    [SerializeField]
    private Slider expSlider;
    [SerializeField]
    private Text sliderTxt;
    [SerializeField]
    private Text mpCosTxt;
    [SerializeField]
    private Text sCoolTimeTxt;
    [SerializeField]
    private Text sDescTxt;
    [SerializeField]
    private Text sBookTxt;
    [SerializeField]
    private Text sBookAmountTxt;
    [SerializeField]
    private Button plusBtn;
    [SerializeField]
    private Button minusBtn;
    [SerializeField]
    private Button okBtn;
    #endregion //변수선언

    private int maxCount; //최대 적용가능한 스킬북 갯수
    private Action<int> OkBtnEvent;
    private Skill selectedSkill;


    private void Start()
    {
        Init();
    }

    private void Init()
    {

        //마이너스 버튼 
        minusBtn.onClick.AddListener(() =>
        {
            int.TryParse(sBookTxt.text, out int count);
            if (count > 0)
            {
                int nextCount = count - 1;
                if (nextCount < 1)
                    nextCount = 1;

                sBookTxt.text = nextCount.ToString();
            }


        });

        //플러스 버튼
        plusBtn.onClick.AddListener(() =>
        {
            int.TryParse(sBookTxt.text, out int count);
            if (count < maxCount)
            {
                int nextCount = count + 1;
                if (nextCount > maxCount)
                    nextCount = maxCount;
                sBookTxt.text = nextCount.ToString();
            }
           
        });

        //OK버튼 
        okBtn.onClick.AddListener(() =>
        {
            if(int.Parse(sBookTxt.text) > 0)
            {
                OkBtnEvent(int.Parse(sBookTxt.text));
                this.gameObject.SetActive(false);
                selectedSkill.GetExp(int.Parse(sBookTxt.text));
            }    
        });

    }

    //스킬 상세창 보여주기
    public void ShowSkillPage(Skill skill,int skillBook, Action<int> okCallback)
    {
        sNameTxt.text = skill.SkillName;
        sExpTxt.text = skill.SkillLevel + "Lv " + SkillMaster(skill.SkillLevel);
        expSlider.value = skill.SkillExp / skill.MaxSkillExp;
        sliderTxt.text = Mathf.Round(skill.SkillExp / skill.MaxSkillExp * 100).ToString() + "%";
        mpCosTxt.text = "MP 소모량 : " + skill.CoolTime;
        sCoolTimeTxt.text = "재사용 대기시간 : " + skill.CoolTime;
        sDescTxt.text = skill.Description;
        sBookTxt.text = 0.ToString();
        sBookAmountTxt.text = skillBook.ToString();

        maxCount = skillBook;
        selectedSkill = skill;

        SetOkBtnEvent(okCallback);
    }

    private string SkillMaster(int level)//스킬 숙련도 나타내기
    {
        if (level >= 0 && level < 10)
        {
            return "초급";
        }
        else if (level >= 10 && level < 20)
        {
            return "중급";
        }
        else if (level >= 20 && level < 30)
        {
            return "고급";
        }
        else
        {
            return "";
        }
    }


    private void SetOkBtnEvent(Action<int> action) => OkBtnEvent = action;
}
