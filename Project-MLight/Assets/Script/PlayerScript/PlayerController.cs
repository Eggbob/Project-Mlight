using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class PlayerController : LivingEntity
{
    private float atkSpeed = 1f; //공격 속도
    private bool isRun; // 움직임 관련 불값
    private bool isInter; // 오브젝트 상호작용 관련 불값
    private bool isAttack; // 공격하는지

    public PlayerMoveController pmanager { get; private set; }
    public PlayerSkillController psCon { get; private set; }

    public BuffManager buffManager;
    public ActiveSkill pAttack; //플레이어 스킬
    public MeleeWeaponTrail trail; //무기 궤적
    public AudioClip footSound;
    public AudioClip lvUpSound;
    public Transform weaponPos;
    public Action<Enemy> killAction; //적 처치시 액션
        
    public enum PlayerState { Idle, Move, Attack, Skill, Drop, Die }
    public PlayerState pState; //플레이어 상태 변수
   
    public float AtkSpeed => atkSpeed;

    private void Awake()
    {    
        pmanager = this.GetComponent<PlayerMoveController>();
        psCon = this.GetComponent<PlayerSkillController>();
        anim = this.GetComponent<Animator>();
        pState = PlayerState.Idle;

        statusInit();

        DontDestroyOnLoad(gameObject);
    }

   
    private void Update()
    {
        miniMapIcon.transform.eulerAngles = new Vector3(-90, 0, 0);

        if (dead) return;

        target = pmanager.target;
    
        CheckAnimations();
        
        anim.SetBool("isRun", isRun);
        anim.SetFloat("Attack", atkSpeed);
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isInter", isInter);
      
    }

    //애니메이션 재생
    private void CheckAnimations()
    {
        switch (pState)
        {
            case PlayerState.Idle:
                isRun = false;
                isAttack = false;
                isInter = false;
                break;
            case PlayerState.Move:            
                isRun = true;
                isAttack = false;
                isInter = false;      
                break;
            case PlayerState.Attack:
                isRun = false;
                isInter = false;
                isAttack = true;
                break;
            case PlayerState.Drop:
                isRun = false;
                isAttack = false;
                isInter = true;
                DropUpdate();
                break;
            case PlayerState.Die:
                break;
        }
    }


  
    //공격시
    private void AttackCheck()
    {
        Enemy enemytarget = target.GetComponent<Enemy>();

        trail.Emit = true;
        if (enemytarget != null)
        {            
            if (enemytarget.dead)
            {
                if (killAction != null) { killAction(enemytarget); }

                pmanager.curtarget = PlayerMoveController.TargetLayer.None;
                pState = PlayerState.Idle;
                pmanager.mstate = PlayerMoveController.MoveState.Stop;
               
                return;
            }
            else
            {
                StartCoroutine(DamageRoutine(enemytarget));
            }
        }
    }
    
    //공격 루틴
    private IEnumerator DamageRoutine(LivingEntity enemyTarget)
    {       
        yield return new WaitForSeconds(0.7f);
      
        if(pmanager.isCollision)
        {
            pAttack.ActiveAction();
           
        }
        trail.Emit = false;
    }

    //아이템 습득시
    private void DropUpdate()
    {
        target = pmanager.target;
        StartCoroutine("DropRoutine");      
    }

    //아이템 습득루틴
    private IEnumerator DropRoutine()
    {
        yield return new WaitForSeconds(0.3f);

        if (isInter)
        {
            Object obj = target.GetComponent<Object>();

            if (obj is ItemPickUp ipick)
            {
                ipick.Drop();

                target = null;
            }
            else if(obj is CoinPickUp cpick)
            {
                cpick.Drop();
                target = null;
            }
            else 
            {
                NotificationUI.Instance.GenerateTxt("습득할수 없는 아이템입니다.");
            }
            isInter = false;
           
            pState = PlayerState.Idle;
        }
      
    }
  
    //레벨업시
    protected override void LvUp()
    {
        GameObject obj = ObjectPool.GetLvEffect();

        Vector3 tr = this.transform.position;
        tr.y += 5f;

        obj.transform.position = tr;

        //audioSource.PlayOneShot(lvUpSound);
        BgmManager.Instance.PlayCharacterSound(lvUpSound);

        base.LvUp();

    }

    protected override void Die()
    {
        base.Die();
        anim.SetTrigger("Dead");
        anim.SetBool("isDead", dead);
    }

    //스킬 애니메이션 업데이트
    public void SkillUpdate(int sId)
    {
        pState = PlayerState.Skill;

        anim.SetTrigger("isSkill");
        anim.SetInteger("Skill", sId);
    }


    public void PlayFootSound()
    {
        //audioSource.PlayOneShot(footSound, 0.1f);
        BgmManager.Instance.PlayCharacterSound(footSound);
    }

    //데미지를 받을시
    public override void OnDamage(Skill skill)
    {
        if (dead) return;

        base.OnDamage(skill);

        var dTxt = ObjectPool.GetDTxt();
        dTxt.SetText((int)skill.SkillPower);
        dTxt.transform.position = this.transform.position;
    }

    //공격 속도 설정
    public void SetAtkSpeed(float speed)
    {
        atkSpeed += speed;
    }

    public void RespawnPlayer()
    {
        this.Hp = MaxHp;
        this.Mp = MaxMp;

        dead = false;
        anim.SetBool("isDead", dead);
        pState = PlayerState.Idle;
    }

   

}
