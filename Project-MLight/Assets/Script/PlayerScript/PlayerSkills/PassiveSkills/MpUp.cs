using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpUp : PassiveSkill
{
    public override void PassiveAction()
    {
        LCon.Mp += (int)_skillPower;
    }

    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;

        PassiveAction();
    }

    protected override void SkillLevelUp()
    {
        float remainSkillExp = SkillExp - MaxSkillExp;
        _skillExp = remainSkillExp;

        _skillPower += 100;

        _skillLevel++;
        _maxSkillExp *= 2;

        PassiveAction();
    }
}
