using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPowerAttack : ActiveSkill
{
    private GameObject effect;
    private PowerAttack pBird;

    public PowerAttack pBirdPrefab;

    private IEnumerator AttackRoutine()
    {
        //effect.gameObject.SetActive(true);

        pBird.gameObject.transform.position = LCon.transform.position;
      
       // pBird.Activation(LCon.transform.forward);
 
        yield return new WaitForSeconds(1f);

      // effect.gameObject.SetActive(false);
       // pBird.gameObject.SetActive(false);
    }


    public override void ActiveAction()
    {
        StartCoroutine(AttackRoutine());
    }

    public override void Init(LivingEntity _Lcon)
    {
        LCon = _Lcon;

        _skillPower = (LCon.Int * Damage) / 100;

        this.sAttr = SkillAttr.Melee;

        effect = Instantiate(EffectPrefab, this.transform);
        effect.SetActive(false);

        pBird = Instantiate(pBirdPrefab, this.transform).GetComponent<PowerAttack>();
       // pBird.Init(this);
        pBird.gameObject.SetActive(false);
    }

}
