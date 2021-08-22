using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFireBall : ActiveSkill
{

    private FireBall[] fireball = new FireBall[7];

    private IEnumerator SpellRoutine()
    {
        for (int i = 0; i < 7; i++)
        {
            Vector3 spawnPos = Random.insideUnitCircle * 10f;
            spawnPos.x += this.transform.position.x;
            spawnPos.z = spawnPos.y + this.transform.position.z;
            spawnPos.y = this.transform.position.y + 3f;

            fireball[i].transform.position = spawnPos;
            fireball[i].CreaeteFire(spawnPos);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public override void ActiveAction()
    {
        StartCoroutine(SpellRoutine());
    }

  
    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;
        _skillPower = (LCon.Int * Damage) / 100;

        this.sAttr = SkillAttr.Melee;

        for(int i = 0; i< 7; i++)
        {
            fireball[i] = Instantiate(EffectPrefab, this.transform).GetComponent<FireBall>();
            fireball[i].gameObject.SetActive(false); 
        }
    }
}
