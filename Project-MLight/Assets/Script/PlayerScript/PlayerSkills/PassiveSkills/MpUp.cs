using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpUp : PassiveSkill
{
    public override void PassiveAction()
    {
        LCon.MaxMp += 100;
        LCon.Mp = LCon.MaxMp;
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
