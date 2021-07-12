using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : Status
{
    public GameObject target;
    //public delegate void GetExp(int amount);
    public Action ExpGet;

    public override void statusInit(int pHp = 100, int pMp = 100, int pPower = 10, int pInt = 10, int pDef = 10)
    {
       // ExpGet += ExpGetRoutine;

        base.statusInit(pHp, pMp, pPower, pInt, pDef);
    }

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

    //레벨업 
    protected virtual void LvUp()
    {
        Level++;
        int leftexp = Exp - _maxExp;
        _maxExp += 200;
        _exp = leftexp;
        _statPoint += 3;
    }

    //경험치 획득 
    public void ExpGetRoutine(float mount)
    {
        mount *= BonusExp;

        _exp += (int)mount;

        Debug.Log(mount);
      
        while (Exp >= MaxExp)
        {
            LvUp();
        }

        ExpGet();
    }
}
