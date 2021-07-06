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
    public int SkillLevel = 1; //스킬 레벨
    public float SkillExp = 0; //스킬 경험치
    public float MaxSkillExp =200; //스킬 총 경험치
    public int MpCost; //마나 사용량
    public float CoolTime; // 재사용 대기시간
    public float ActTime; //스킬 지속시간
    public float SkillPower; //스킬 위력
   
    public LivingEntity LCon;  //스킬을 소유하고 있는 LivingEntity

    //public event Action contents; 스킬효과

    public enum SkillAttr //스킬 속성
    {
        None, Melee, Stun, Buff, Done
    }

    public SkillAttr sAttr; //스킬 속성 변수

    private  void Awake()
    {
       
    }


    public virtual void Init(LivingEntity _LCon)
    {

    }


    public virtual void ActiveAction() //액티브 스킬일 경우
    {
 
    }

    public virtual void PassiveAction() // 패시브 스킬일 경우
    {
       
    }


   public void GetExp(float _exp)
   {
        SkillExp =+ _exp * 100;
        while (SkillExp > MaxSkillExp)
        {
            SkillLevelUp();
        }
   }

   protected void SkillLevelUp()
   {
       float remainSkillExp = SkillExp - MaxSkillExp;
       SkillExp = remainSkillExp;

       SkillLevel++;
       MaxSkillExp *= 2;

   }


  
}
