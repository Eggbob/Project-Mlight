using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobeAttack : Skill
{  

    protected override void SkillLevelUp()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(LivingEntity _Lcon)
    {
        LCon = _Lcon;
        _skillPower = LCon.Power;
    }
}
