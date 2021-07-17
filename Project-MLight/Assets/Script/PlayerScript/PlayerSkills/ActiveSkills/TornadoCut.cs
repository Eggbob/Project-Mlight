using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoCut : ActiveSkill
{
    public GameObject SkillRangePrefab;
    private GameObject SkilLRange;
    public GameObject Effect;
 
    public LayerMask targetLayer; // 공격 대상 레이어
    public float fRange; // 수색범위

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
    }

    IEnumerator DamageRoutine (Collider[] _colliders)
    {
        SkilLRange.SetActive(true);
        yield return new WaitForSeconds(1f);
        Instantiate(Effect, this.transform.position, Quaternion.identity);
        foreach (Collider col in _colliders)
        {
            LivingEntity enemytarget = col.GetComponent<LivingEntity>();
            enemytarget.OnDamage(this);
        }      
        yield return new WaitForSeconds(1.1f);
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
    }

}
