using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthUp : Skill
{
    public GameObject EffectPrefab;
    GameObject effect;

    public override void ActiveAction()
    {
        Vector3 ePos = LCon.transform.position;
        ePos.y += 4f;

        effect = Instantiate(EffectPrefab, ePos, this.transform.rotation);
        Destroy(effect, 1f);
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
        Debug.Log(LCon.Power);
        yield return new WaitForSeconds(this.ActTime);
        LCon.Power -= (int)this.SkillPower;
      
    }

}
