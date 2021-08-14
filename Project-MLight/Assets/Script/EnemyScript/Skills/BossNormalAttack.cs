using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNormalAttack : ActiveSkill
{
    public override void Init(LivingEntity _Lcon)
    {
        LCon = _Lcon;
        _skillPower = LCon.Power;
    }
}
