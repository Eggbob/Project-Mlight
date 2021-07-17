using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUp : PassiveSkill
{
    public override void PassiveAction()
    {
        LCon.MaxHp += 100;
        LCon.Hp = LCon.MaxHp;
    }

    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;

        PassiveAction();
    }

    protected override void SkillLevelUp()
    {
     
        _skillPower += 100;

        base.SkillLevelUp();


        PassiveAction();
    }

}
