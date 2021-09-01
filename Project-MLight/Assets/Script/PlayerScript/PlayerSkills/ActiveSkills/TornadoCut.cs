using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoCut : ActiveSkill
{
    public GameObject SkillRangePrefab;
    public LayerMask targetLayer; // 공격 대상 레이어
    public float fRange; // 수색범위

    private GameObject SkilLRange;

    public override void ActiveAction()
    {
        Collider[] colliders = Physics.OverlapSphere(LCon.transform.position, fRange, targetLayer);//콜라이더 설정하기

        if (colliders != null) //콜라이더가 비어있지 않으면
        {
            StartCoroutine(DamageRoutine(colliders));
        }
    }

    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;
        _skillPower = (LCon.Power * Damage) / 100;
        this.sAttr = SkillAttr.Melee;
        SkilLRange = Instantiate(SkillRangePrefab, LCon.gameObject.transform);
        SkilLRange.SetActive(false);
        effect = Instantiate(effectPrefab, this.gameObject.transform);
        effect.SetActive(false);

        this._description = "범위 내의 모든 적들에게 물리 공격력의 " + Damage + "%만큼 공격을 합니다.";

        base.Init(LCon);
    }

    IEnumerator DamageRoutine (Collider[] _colliders)
    {
        SkilLRange.SetActive(true);
        yield return new WaitForSeconds(1f);
 
        effect.transform.position = this.transform.position;
        effect.gameObject.SetActive(true);
        pAudio.PlayOneShot(effectSound);

        foreach (Collider col in _colliders)
        {
            LivingEntity enemytarget = col.GetComponent<LivingEntity>();
            enemytarget.OnDamage(this);
        }      
        yield return new WaitForSeconds(1.1f);
        effect.SetActive(false);
        SkilLRange.SetActive(false);
    }

    protected override void SkillLevelUp()
    {
        float remainSkillExp = SkillExp - MaxSkillExp;
        _skillExp = remainSkillExp;

        Damage += 10;

        _skillPower = (LCon.Power * Damage) / 100;
        _skillLevel++;
        _maxSkillExp *= 2;

        this._description = "범위 내의 모든 적들에게 물리 공격력의 " + Damage + "%만큼 공격을 합니다.";
    }

}
