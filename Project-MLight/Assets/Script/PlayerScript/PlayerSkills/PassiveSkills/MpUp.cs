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

        this._description = "최대 MP가 " + _skillPower + "만큼 증가합니다.";
        PassiveAction();
    }

    protected override void SkillLevelUp()
    {
        _skillPower += 100;
        this._description = "최대 MP가 " + _skillPower + "만큼 증가합니다.";

        base.SkillLevelUp();

        PassiveAction();
    }
}
