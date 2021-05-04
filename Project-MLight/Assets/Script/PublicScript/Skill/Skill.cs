using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//종합 스킬 클래스
public abstract class Skill: MonoBehaviour
{
    public Texture Icon; // 스킬 아이콘
    public string SkillName; //스킬 이름
    public string Description; //스킬 설명
    public int Skillid; //스킬 아이디
    public float SkillExp; //스킬 진척도
    public int MpCost; //마나 사용량
    public float coolTime; // 재사용 대기시간
    public event Action contents; //스킬 액티브

    private void Start()
    {
        contents += SKillContent;
    }

    private void SKillContent()
    {

    }

}
