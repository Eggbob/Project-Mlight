using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUp : PassiveSkill
{
    public override void PassiveAction()
    {
        LCon.MaxHp += 10;
        LCon.Hp = LCon.MaxHp;
    }

    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;

        this._description = "최대 HP가 " + _skillPower + "만큼 증가합니다.";

        PassiveAction();
    }

    protected override void SkillLevelUp()
    {
     
        _skillPower += 10;
        this._description = "최대 HP가 " + _skillPower + "만큼 증가합니다.";

        base.SkillLevelUp();

        PassiveAction();
    }

}
