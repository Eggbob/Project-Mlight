using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : ActiveSkill
{
 
    private GameObject effect;

    public override void ActiveAction()
    {
        Rigidbody tRigid = LCon.target.GetComponent<Rigidbody>();

        StartCoroutine(DamageRoutine(tRigid));
    }

    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;
        _skillPower = (LCon.Power * Damage) / 100;

        this._description = "물리공격력의 " + this._skillPower + "%만큼 단일 공격을 합니다.";
     
        this.sAttr = SkillAttr.Melee;

        effect = Instantiate(EffectPrefab, this.transform);
        effect.gameObject.SetActive(false);
    }

    IEnumerator DamageRoutine(Rigidbody tRigid)
    {
        LivingEntity enemytarget = LCon.target.GetComponent<LivingEntity>();
        Vector3 ePos = LCon.transform.position;
        ePos.y += 2f;
        yield return new WaitForSeconds(0.5f); 
        
        effect.gameObject.transform.position = ePos;
        effect.gameObject.transform.rotation = this.transform.rotation;
        effect.gameObject.SetActive(true);

         yield return new WaitForSeconds(0.5f);
        enemytarget.OnDamage(this);
        yield return new WaitForSeconds(0.9f);
        effect.gameObject.SetActive(false);
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

        this._description = "물리공격력의 " + this._skillPower + "%만큼 단일 공격을 합니다.";
    }

}
