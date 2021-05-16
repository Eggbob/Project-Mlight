using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust : Skill
{
    public int nuckBackForce;
    public int Damage;

    public override void ActiveAction()
    {
        Debug.Log(MpCost);
        Debug.Log(pCon.Mp);
       
        

       if(pCon.Mp < this.MpCost)
        {
            Debug.Log("마나가 부족합니다");
        }
       else
        {
            pCon.Mp -= this.MpCost;
            pCon.SkillUpdate(Skillid);
            base.ActiveAction();
        }

    }


    private void Start()
    {
        contents += SKillContent;
        pCon = PlayerController.instance;
    }


    IEnumerator DamageRoutine(Rigidbody tRigid)
    {
        LivingEntity enemytarget = pCon.target.GetComponent<LivingEntity>();
        yield return new WaitForSeconds(1f);
        enemytarget.OnDamage(pCon.Power * (1+(Damage/100)), sType);
         tRigid.AddForce(pCon.target.transform.forward * nuckBackForce * -1, ForceMode.Impulse);
    }

    private void SKillContent()
    {
        
        Rigidbody tRigid = pCon.target.GetComponent<Rigidbody>();
        StartCoroutine(DamageRoutine(tRigid));
       
    }

}