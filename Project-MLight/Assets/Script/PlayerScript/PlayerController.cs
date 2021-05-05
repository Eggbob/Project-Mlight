using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : LivingEntity
{
    private Status status;
    [SerializeField]
    private PlayerMoveController pmanager;
    private Animator anim; //애니메이터 컴포넌트
  

    public enum PlayerState { Idle, Move, Attack, Skill, Drop,  Die }
    public PlayerState pState; //플레이어 상태 변수

    public bool isMove; // 움직임 관련 불값
    public bool isInter; // 오브젝트 상호작용 관련 불값
    public bool isAttack; // 공격 관련 불값

    private void Awake()
    {
        status = this.GetComponent<Status>();
        pmanager = this.GetComponent<PlayerMoveController>();
        anim = this.GetComponent<Animator>();
        pState = PlayerState.Idle;
    }


    private void Start()
    {
     
    }

    void CheckAnimations()
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
                MoveUpdate();
                break;
            case PlayerState.Attack:
                isMove = false;
                isAttack = true;
                isInter = false;            
                break;
            case PlayerState.Skill:
                anim.SetTrigger("isSkill");
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

    void CheckStatus()
    {
        switch (pState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Move:
                MoveUpdate();
                break;
            case PlayerState.Attack:
                break;
            case PlayerState.Skill:
                break;
            case PlayerState.Drop:
                DropUpdate();
                break;
            case PlayerState.Die:
                break;
        }
    }

    public void SkillUpdate(int sId)
    {
       if(pState == PlayerState.Attack && pmanager.target != null)
        {
            switch (sId)
            {
                case 1:
                    anim.SetTrigger("isSkill");
                    anim.SetInteger("Skill", 1);
                    break;
                case 2:
                    anim.SetTrigger("isSkill");
                    anim.SetInteger("Skill", 2);
                    break;
                case 3:
                    anim.SetTrigger("isSkill");
                    anim.SetInteger("Skill", 3);
                    break;
            }
            pState = PlayerState.Skill;
        }
        
    }

    void IdleUpdate()
    {

    }

    void MoveUpdate()
    {
      
    }
    
    void AttackUpdate()
    {
        LivingEntity enemytarget = pmanager.target.GetComponent<LivingEntity>();

        if (enemytarget != null && !enemytarget.dead) { enemytarget.OnDamage(Power); }
        else if(enemytarget.dead) { pState = PlayerState.Idle; }
  
    }
    
    void DropUpdate()
    {      
        StartCoroutine("DropRoutine");      
    }

    IEnumerator DropRoutine()
    {
        yield return new WaitForSeconds(1f);
        isInter = false;
        pState = PlayerState.Idle;
    }

    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
    }

    private void Update()
    {
        CheckStatus();
        CheckAnimations();
        anim.SetBool("isRun",isMove);
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isInter", isInter);

      
    }

    void UsedSkill (int id)
    {
        switch(id)
        {
            case 0:
                print("Used Skill 1");
                break;
            case 1:
                print("Used Skill 2");
                break;
            case 2:
                print("Used Skill 3");
                break;
            case 3:
                print("Used Skill 4");
                break;
            default:
                print("Skill Error");
                break;
        }
    }


  
}
