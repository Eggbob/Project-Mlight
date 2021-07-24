using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : Status
{
    public GameObject target;
  
    public Action ExpGetAction;

    public Action DieAction;

    public BuffManager buffManager;

    public override void statusInit(int pHp = 100, int pMp = 100, int pPower = 10, int pInt = 10, int pDef = 10)
    {
        base.statusInit(pHp, pMp, pPower, pInt, pDef);
    }

    public virtual void OnDamage(Skill skill)//데미지를 받을시 호출될 함수
    {
        Hp -= (int)skill.SkillPower;  // 스킬의 위력만큼 HP 감소

       if(Hp <= 0 && !dead)
       {
            Hp = 0;
            StopAllCoroutines();
            
            Die();
            
       }
    }

    //체력 회복
    public virtual void RestoreHealth(int amount)
    {
        if(amount + this.Hp > this.MaxHp)
        {
            this.Hp = this.MaxHp;
        }
        else
        {
            this.Hp += amount;
        }
    }

    //마나 회복
    public virtual void RestoreMana(int amount)
    {
        if (amount + this.Mp > this.MaxMp)
        {
            this.Mp = this.MaxMp;
        }
        else
        {
            this.Mp += amount;
        }
    }

    //죽었을시
    protected virtual void Die()
    {
        dead = true;
        DieAction();
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

        while (Exp >= MaxExp)
        {
            LvUp();
        }

        ExpGetAction();
    }
}
