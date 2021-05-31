using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlayerAttack : Skill
{
    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;
        this.SkillPower = LCon.Power;
        this.sAttr = SkillAttr.Melee;
    }
}
