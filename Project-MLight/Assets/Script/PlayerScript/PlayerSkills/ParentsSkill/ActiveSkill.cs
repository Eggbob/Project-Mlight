using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    public GameObject effectPrefab;
    public AudioClip effectSound;

    [SerializeField]
    protected float Damage;

    protected GameObject effect;


    public virtual void ActiveAction() { }

    public override void Init(LivingEntity _Lcon) { }


    public enum SkillAttr //스킬 속성
    {
        None, Melee, Stun, Buff, Done
    }

    protected SkillAttr sAttr; //스킬 속성 변수

    public SkillAttr SAttr => sAttr;


  
}
