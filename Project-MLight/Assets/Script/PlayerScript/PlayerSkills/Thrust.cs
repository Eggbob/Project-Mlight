using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust : Skill
{
    public int nuckBackForce;

    private void ActiveAction()
    {
       Rigidbody tRigid =  pCon.target.GetComponent<Rigidbody>();
        tRigid.AddForce(pCon.target.transform.forward * nuckBackForce, ForceMode.Impulse);
    }
}