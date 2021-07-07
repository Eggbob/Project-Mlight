using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : ActiveSkill
{
    [SerializeField]
    private int Damage;
    public GameObject Effect;

    public override void ActiveAction()
    {
        Rigidbody tRigid = LCon.target.GetComponent<Rigidbody>();

        StartCoroutine(DamageRoutine(tRigid));
    }

    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;
        _skillPower = (LCon.Power * Damage) / 100;
        this.sAttr = SkillAttr.Melee;
  
    }

    IEnumerator DamageRoutine(Rigidbody tRigid)
    {
        LivingEntity enemytarget = LCon.target.GetComponent<LivingEntity>();
        Vector3 ePos = LCon.transform.position;
        ePos.y += 2f;
        yield return new WaitForSeconds(0.5f);
        Instantiate(Effect, ePos, this.transform.rotation);

        yield return new WaitForSeconds(0.5f);
        enemytarget.OnDamage(this);
        yield return new WaitForSeconds(0.9f);
        tRigid.velocity = Vector3.zero;
    }

    protected override void SkillLevelUp()
    {
        float remainSkillExp = SkillExp - MaxSkillExp;
        _skillExp = remainSkillExp;

        Damage += 10;

        _skillPower = (LCon.Power * Damage) / 100;
        _skillLevel++;
        _maxSkillExp *= 2;
    }

}
