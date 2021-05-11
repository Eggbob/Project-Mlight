using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust : Skill
{
    public int nuckBackForce;
    public int Damage;

    public override void ActiveAction()
    {
       if(pCon.Mp < this.MpCost)
        {
            Debug.Log("마나가 부족합니다");
        }
       else
        {
            pCon.Mp -= this.MpCost;
            base.ActiveAction();
        }

    }


    IEnumerator DamageRoutine(float second)
    {
        LivingEntity enemytarget = pCon.target.GetComponent<LivingEntity>();
        yield return second;
        enemytarget.OnDamage(pCon.Power * (1+(Damage/100)));
    }

    private void SKillContent()
    {
        Rigidbody tRigid = pCon.target.GetComponent<Rigidbody>();
        tRigid.AddForce(pCon.target.transform.forward * nuckBackForce * -1, ForceMode.Impulse);
    }

}