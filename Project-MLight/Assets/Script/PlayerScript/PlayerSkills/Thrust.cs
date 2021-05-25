using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust : Skill
{
    public int nuckBackForce;
    public int Damage;

    public override void ActiveAction()
    {
        if (LCon.target == null)
        {
            Debug.Log("대상이 없습니다");
            return;
        }
        else if (LCon.Mp < this.MpCost)
        {
            Debug.Log("마나가 부족합니다");
            return;
        }      
       else
        {
            LCon.Mp -= this.MpCost;
            //LCon.SkillUpdate(Skillid);
            base.ActiveAction();
        }

    }


    private void Start()
    {
        LCon = PlayerController.instance;
        contents += SKillContent;
        this.SkillPower = LCon.Power * (1 + (Damage / 100));
        this.sAttr = SkillAttr.Stun;
        nuckBackForce = 5;
    }


    IEnumerator DamageRoutine(Rigidbody tRigid)
    {
        LivingEntity enemytarget = LCon.target.GetComponent<LivingEntity>();
        yield return new WaitForSeconds(1f);
        enemytarget.OnDamage(this);
        tRigid.AddForce(LCon.target.transform.forward * nuckBackForce * -1, ForceMode.Impulse);
        yield return new WaitForSeconds(0.9f);
        tRigid.velocity = Vector3.zero;
    }

    private void SKillContent()
    {        
        Rigidbody tRigid = LCon.target.GetComponent<Rigidbody>();
        StartCoroutine(DamageRoutine(tRigid));      
    }

}