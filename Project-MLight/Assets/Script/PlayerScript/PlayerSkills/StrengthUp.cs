using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthUp : Skill
{

    public int Damage;

    public override void ActiveAction()
    {
        StartCoroutine(BuffRoutine());
    }

    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;
        this.SkillPower = (LCon.Power * Damage) / 100;
        this.sAttr = SkillAttr.Buff;
    }

    IEnumerator BuffRoutine()
    {
        yield return new WaitForSeconds(this.ActTime);                                                                                                                              
    }

}
