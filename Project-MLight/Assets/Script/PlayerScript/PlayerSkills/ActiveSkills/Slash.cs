using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : ActiveSkill
{
    [SerializeField]
    private int nuckBackForce;

    private GameObject effect;

    public override void ActiveAction()
    {
        Rigidbody tRigid = LCon.target.GetComponent<Rigidbody>();

        StartCoroutine(DamageRoutine(tRigid));
    }


    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;

        _skillPower = LCon.Power * (1 + (Damage / 100));
        this.sAttr = SkillAttr.Stun;
        nuckBackForce = 5;

        this._description = "물리공격력의 " + this._skillPower + "%만큼 단일 공격을 합니다.\n"
        +"- 대상을 기절시킵니다.";

        effect = Instantiate(EffectPrefab, this.transform);
        effect.gameObject.SetActive(false);
    }

    IEnumerator DamageRoutine(Rigidbody tRigid)
    {
        LivingEntity enemytarget = LCon.target.GetComponent<LivingEntity>();
 
        effect.transform.position = this.transform.position;
        effect.transform.rotation = this.transform.rotation;
        effect.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        enemytarget.OnDamage(this);
        tRigid.AddForce(LCon.target.transform.forward * nuckBackForce * -1, ForceMode.Impulse);
        yield return new WaitForSeconds(0.9f);
        effect.gameObject.SetActive(true);
        tRigid.velocity = Vector3.zero;
    }

    protected override void SkillLevelUp()
    {
        float remainSkillExp = SkillExp - MaxSkillExp;
        _skillExp = remainSkillExp;

        Damage += 10;

        _skillPower = LCon.Power * (1 + (Damage / 100));
        _skillLevel++;
        _maxSkillExp *= 2;

        this._description = "물리공격력의 " + this._skillPower + "%만큼 단일 공격을 합니다.\n"
    + "- 대상을 기절시킵니다.";
    }
}
