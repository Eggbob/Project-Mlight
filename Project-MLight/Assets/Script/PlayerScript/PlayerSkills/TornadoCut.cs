using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoCut : Skill
{
    public GameObject SkillRange;
    public int Damage;
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
        this.SkillPower = (LCon.Power * Damage) / 100;
        this.sAttr = SkillAttr.Melee;
        SkillRange.SetActive(false);
    }

    IEnumerator DamageRoutine (Collider[] _colliders)
    {
        SkillRange.SetActive(true);
        foreach (Collider col in _colliders)
        {
            LivingEntity enemytarget = col.GetComponent<LivingEntity>();
            enemytarget.OnDamage(this);
        }      
        yield return new WaitForSeconds(0.9f);
        SkillRange.SetActive(false);
    }

    private void OnDrawGizmos() // 범위 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(LCon.transform.position, fRange);
    }

}
