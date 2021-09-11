using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : ActiveSkill
{
    private GameObject dangerCircle;

    private void OnTriggerEnter(Collider other)
    {
        BgmManager.Instance.PlayCharacterSound(effectSound);

        if (other.gameObject.CompareTag("Player"))
        {
            LivingEntity target = other.gameObject.GetComponent<LivingEntity>();

            target.OnDamage(this);
            effectPrefab.SetActive(true);
            Invoke("GetBakcRoutine", 0.5f);       
        }
        else if(other.gameObject.CompareTag("Terrian"))
        {
            effectPrefab.SetActive(true);
            Invoke("GetBakcRoutine", 0.5f);
        }
       
    }

    private void GetBakcRoutine()
    {
        BossObjectPool.ReturnDangerCircle(dangerCircle);
        BossObjectPool.ReturnFireball(this);
    }

    public override void Init(LivingEntity _Lcon)
    {
        LCon = _Lcon;
        base.Init(LCon);
    }
   
    public void CreaeteFire(Vector3 pos)
    {
        effectPrefab.SetActive(false);
        this.transform.position = pos;
        this.gameObject.SetActive(true);

        dangerCircle = BossObjectPool.GetDangerCircle();

        pos.y = 1;

        dangerCircle.transform.position = pos;
    }

   
}
