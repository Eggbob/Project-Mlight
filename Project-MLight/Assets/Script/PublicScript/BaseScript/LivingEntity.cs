using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LivingEntity : Status
{
    #region
    [SerializeField]
    protected float moveSpeed;

    protected Action ExpGetAction;

    protected Action DieAction;

    public GameObject target;

    public GameObject miniMapIcon;
    
    public Animator anim { get; protected set; } //애니메이터 컴포넌트

    public NavMeshAgent nav { get; protected set; } //네브 메쉬 컴포넌트

    public float MoveSpeed => moveSpeed;

    #endregion


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
        _maxHP += 100;
        _maxMP += 100;

        _exp = leftexp;
        _statPoint += 3;
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

    //경험치 획득 액션 설정
    public void AddExpAction(Action _ExpAciton)
    {
        ExpGetAction += _ExpAciton;
    }

    public void AddDieAction(Action _DieAciton)
    {
        DieAction += _DieAciton;
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
