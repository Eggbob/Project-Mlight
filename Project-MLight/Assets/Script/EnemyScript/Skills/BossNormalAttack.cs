using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNormalAttack : ActiveSkill
{
    private LivingEntity target;

    public override void Init(LivingEntity _Lcon)
    {
        LCon = _Lcon;
        _skillPower = LCon.Power;

        target = GameManager.Instance.Player;
    }

    public override void ActiveAction()
    {
        target.OnDamage(this);
    }
}
