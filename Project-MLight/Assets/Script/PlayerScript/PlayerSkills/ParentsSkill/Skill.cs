using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//종합 스킬 클래스
public abstract class Skill: MonoBehaviour
{

    [SerializeField]
    protected Sprite _icon; // 스킬 아이콘
    [SerializeField]
    protected string _skillName; //스킬 이름
    [Multiline]
    [SerializeField]
    protected string _description; //스킬 설명
    [SerializeField]
    protected int _skillId; //스킬 아이디
    [SerializeField]
    protected int _skillLevel = 1; //스킬 레벨
    [SerializeField]
    protected float _skillExp = 0; //스킬 경험치
    [SerializeField]
    protected float _maxSkillExp = 200; //스킬 총 경험치
    [SerializeField]
    protected int _mpCost; //마나 사용량
    [SerializeField]
    protected float _coolTime; // 재사용 대기시간
    [SerializeField]
    protected float _actTime; //스킬 지속시간
    [SerializeField]
    protected float _skillPower; //스킬 위력

    protected LivingEntity LCon;  //스킬을 소유하고 있는 LivingEntity

    public Sprite Icon => _icon; // 스킬 아이콘
    public string SkillName => _skillName; //스킬 이름
    public string Description => _description; //스킬 설명
    public int SkillId => _skillId; //스킬 아이디
    public int SkillLevel => _skillLevel; //스킬 레벨
    public float SkillExp => _skillExp; //스킬 경험치
    public float MaxSkillExp => _maxSkillExp; //스킬 총 경험치
    public int MpCost => _mpCost; //마나 사용량
    public float CoolTime => _coolTime; // 재사용 대기시간
    public float ActTime => _actTime; //스킬 지속시간
    public float SkillPower => _skillPower; //스킬 위력



    protected virtual void SkillLevelUp() { }

    public virtual void Init(LivingEntity _Lcon) { }

    public void GetExp(float _exp)
    {
        _skillExp =+ _exp * 100;
        while (SkillExp >= MaxSkillExp)
        {
            SkillLevelUp();
        }
    }

}
