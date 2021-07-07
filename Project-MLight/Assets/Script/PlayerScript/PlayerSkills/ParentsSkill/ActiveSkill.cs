using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
   public virtual void ActiveAction()
    {

    }

    public enum SkillAttr //스킬 속성
    {
        None, Melee, Stun, Buff, Done
    }

    protected SkillAttr sAttr; //스킬 속성 변수

    public SkillAttr SAttr => sAttr;


    protected override void SkillLevelUp()
    {
        throw new System.NotImplementedException();
    }
}
