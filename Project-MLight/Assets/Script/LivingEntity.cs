using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
  [SerializeField]
    public float CrHp { get;  set; } //  현재 HP
    public float MaxHp { get; protected set; } // 최대 HP
    public float CrMp { get;  set; } // 현재 MP
    public float MaxMp { get; protected set; } // 최대 MP
    public float MoveSpeed { get; set; } //이동속도
    public bool dead { get; protected set; } // 사망 상태

    // 생명체가 활성화 될때 상태 리셋
    protected virtual void OnEnable()
    {
        dead = false;

        CrHp = MaxHp;
        CrMp = MaxMp;
    }

    public virtual void OnDamage(float damage)
    {
        CrHp -= damage;

      if(CrHp <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        dead = true;
    }
}
