using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//종합 스킬 클래스
public abstract class Skill: MonoBehaviour
{
    public Sprite Icon; // 스킬 아이콘
    public string SkillName; //스킬 이름
    public string Description; //스킬 설명
    public int Skillid; //스킬 아이디
    public float SkillExp; //스킬 경험치
    public int MpCost; //마나 사용량
    public float coolTime; // 재사용 대기시간
    public string SkillMastery; // 스킬 진척도

    public event Action contents; //스킬효과

    public enum SkillType
    {
        Active, Passive
    }

    public SkillType sType;

    private void Start()
    {
        contents += SKillContent;
        SkillMaster();
    }

    private void SKillContent()
    {

    }


    public void SkillMaster()
    {
        if(SkillExp > 0 && SkillExp < 100)
        {
            SkillMastery = "초급";
        }
        else if(SkillExp > 100 && SkillExp < 200)
        {
            SkillMastery = "중급";
        }
        else if (SkillExp > 200 && SkillExp < 300)
        {
            SkillMastery = "고급";
        }
    }
}
