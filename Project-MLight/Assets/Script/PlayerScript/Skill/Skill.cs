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
    public float SkillExp = 0; //스킬 경험치
    public int MpCost; //마나 사용량
    public float coolTime; // 재사용 대기시간
    protected PlayerController pCon; 

    public event Action contents; //스킬효과

    public enum SkillType
    {
        None, Melee, KnockBack, Stun, Done
    }

    public SkillType sType;

    private  void Awake()
    {
       
    }

    public virtual void ActiveAction()
    {
        contents();
    }

    public virtual void PassiveAction()
    {
        contents();
    }

    private void SKillContent()
    {

    }


  
}
