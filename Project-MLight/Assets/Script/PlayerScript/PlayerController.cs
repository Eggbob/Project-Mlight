using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : LivingEntity
{
   

    public PlayerMoveController pmanager { get; private set; }
    public GameObject levelUpEffect;
    public Animator anim { get; private set; } //애니메이터 컴포넌트
    public static PlayerController instance; //싱글톤을 위한 instance
    public ActiveSkill pAttack;
    public MeleeWeaponTrail trail;
        
    public enum PlayerState { Idle, Move, Attack, Skill, Drop, Die }
    public PlayerState pState; //플레이어 상태 변수


    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed => moveSpeed;

    [SerializeField]
    private Inventory inventory;
    public Inventory Inven => inventory;

    private bool isMove; // 움직임 관련 불값
    private bool isInter; // 오브젝트 상호작용 관련 불값
    private bool isAttack; // 공격하는지
  

    private void Awake()
    {
    
        pmanager = this.GetComponent<PlayerMoveController>();
        anim = this.GetComponent<Animator>();
        pState = PlayerState.Idle;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        statusInit();

        DontDestroyOnLoad(gameObject);
    }

   
    private void Update()
    {
        target = pmanager.target;
        CheckAnimations();
        anim.SetBool("isRun", isMove);
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isInter", isInter);
    }

    //애니메이션 재생
    private void CheckAnimations()
    {
        switch (pState)
        {
            case PlayerState.Idle:
                isMove = false;
                isAttack = false;
                isInter = false;
                break;
            case PlayerState.Move:
                isMove = true;
                isAttack = false;
                isInter = false;      
                break;
            case PlayerState.Attack:
                isMove = false;
                isInter = false;
                isAttack = true;
                break;
            case PlayerState.Drop:
                isMove = false;
                isAttack = false;
                isInter = true;
                DropUpdate();
                break;
            case PlayerState.Die:
                break;
        }
    }

    private void CheckStatus()
    {
        switch (pState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Move:               
                break;
            case PlayerState.Attack:
                break;
            case PlayerState.Skill:
                break;
            case PlayerState.Drop:
              //  DropUpdate();
                break;
            case PlayerState.Die:
                break;
        }
    }

    public void SkillUpdate(int sId)
    {
        pState = PlayerState.Skill;
      
        anim.SetTrigger("isSkill");
        anim.SetInteger("Skill", sId);
        
    }


    void AttackCheck()
    {
        LivingEntity enemytarget = target.GetComponent<LivingEntity>();

        trail.Emit = true;
        if (enemytarget != null)
        {            
            if (enemytarget.dead)
            {            
                pmanager.curtarget = PlayerMoveController.TargetLayer.None;
                pState = PlayerState.Idle;
                pmanager.nav.isStopped = true;
                return;
            }
            else
            {
                StartCoroutine(DamageUpdate(enemytarget));
            }
        }
    }
    

    private IEnumerator DamageUpdate(LivingEntity enemyTarget)
    {       
        yield return new WaitForSeconds(0.7f);
      
        if(pmanager.isCollision)
        {
            pAttack.ActiveAction();
           
        }
        trail.Emit = false;
    }

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

                (ItemData item, int amount) = ipick.Drop();

                inventory.Add(item, amount);
                target = null;
            }
            else
            {
                Debug.Log("습득할수 없는 아이템입니다.");
            }
            isInter = false;
           

            pState = PlayerState.Idle;
        }
      
    }

    //데미지를 받을시
    public override void OnDamage(Skill skill)
    {
        base.OnDamage(skill);
    }


    public override void statusInit(int pHp = 100, int pMp = 100, int pPower = 60, int pInt = 30, int pDef = 30)
    {
        base.statusInit(pHp, pMp, pPower, pInt, pDef);
    }


    //레벨업시
    protected override void LvUp()
    {
        GameObject pre = Instantiate(levelUpEffect, this.transform.position, Quaternion.identity);
        Destroy(pre, 1f);

        base.LvUp();
    }


}
