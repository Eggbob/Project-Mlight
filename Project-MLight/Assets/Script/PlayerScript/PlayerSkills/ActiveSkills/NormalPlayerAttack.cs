﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlayerAttack : ActiveSkill
{
    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;
        this.sAttr = SkillAttr.Melee;
    }

    public override void ActiveAction()
    {
        LivingEntity enemytarget = LCon.target.GetComponent<LivingEntity>();
        _skillPower = LCon.Power + LCon.BonusPower;
        enemytarget.OnDamage(this);
    }
}
