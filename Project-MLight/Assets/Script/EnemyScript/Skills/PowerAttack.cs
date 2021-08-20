using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerAttack : Skill
{
    private Rigidbody rigid;
    private LivingEntity target;
    private GameObject effect;

    public GameObject pAttackEffectPrefab;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        effect = Instantiate(pAttackEffectPrefab, this.transform);
        effect.SetActive(false);
    }

    private void OnEnable()
    {
        target = GameManager.Instance.Player;
        //rigid.velocity = this.transform.forward * 30f;

        //if (LCon != null)
        //{
        //    effect.transform.SetParent(null);
        //    effect.transform.position = LCon.transform.position;
        //    effect.SetActive(true);
        //}
     
        //Invoke("ReturnRoutine", 4.5f);
    } 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {       
            target.OnDamage(this);
            ReturnRoutine();
        }
    }

    private void ReturnRoutine()
    {
        effect.transform.SetParent(this.transform);
        effect.SetActive(false);
        BossObjectPool.ReturnPowerAttack(this);
    }

    public override void Init(LivingEntity _Lcon)
    {
        LCon = _Lcon;
    }

    public void SkillActive()
    {
        rigid.velocity = this.transform.forward * 30f;

        if (!LCon.Equals(null))
        {
            effect.transform.SetParent(null);
            effect.transform.position = LCon.transform.position;
            effect.SetActive(true);
        }

        Invoke("ReturnRoutine", 4.5f);
    }
}
