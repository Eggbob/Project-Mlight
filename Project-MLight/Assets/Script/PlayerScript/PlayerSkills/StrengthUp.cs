using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthUp : Skill
{


    public override void ActiveAction()
    {
        StartCoroutine(BuffRoutine());
    }

    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;
     
        this.sAttr = SkillAttr.Buff;
    }

    IEnumerator BuffRoutine()
    {
        LCon.Power += (int)this.SkillPower;
        yield return new WaitForSeconds(this.ActTime);
        LCon.Power -= (int)this.SkillPower;
    }

}
