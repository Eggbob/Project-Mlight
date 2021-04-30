using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : Status, IDamageable
{
 
    public virtual void OnDamage(int damage)
    {
        Hp -= damage;

      if(Hp <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        dead = true;
    }
}
