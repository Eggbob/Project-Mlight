using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : Status
{
    public GameObject target;

    public virtual void OnDamage(Skill skill)//데미지를 받을시 호출될 함수
    {
        Hp -= (int)skill.SkillPower;  // 스킬의 위력만큼 HP 감소

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
