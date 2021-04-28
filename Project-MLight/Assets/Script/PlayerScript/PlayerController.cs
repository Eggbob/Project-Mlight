using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : LivingEntity
{
    private Status status;
    [SerializeField]
    private PlayerMoveController cmanager;
    private Animator anim; //애니메이터 컴포넌트
  

    public enum PlayerState { Idle, Move, Attack, Skill, Drop,  Die }
    public PlayerState pState; //플레이어 상태 변수

    public bool isMove; // 움직임 관련 불값
    public bool isInter; // 오브젝트 상호작용 관련 불값
    public bool isAttack; // 공격 관련 불값

    private void Awake()
    {
        status = this.GetComponent<Status>();
        cmanager = this.GetComponent<PlayerMoveController>();
        anim = this.GetComponentInChildren<Animator>();
        pState = PlayerState.Idle;
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
                AttackUpdate();
                break;
            case PlayerState.Skill:
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
                AttackUpdate();
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

    void IdleUpdate()
    {

    }

    void MoveUpdate()
    {
      
    }
    
    void AttackUpdate()
    {
       
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

   

    private void Update()
    {
        CheckStatus();
        CheckAnimations();
        anim.SetBool("isRun",isMove);
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isInter", isInter);
    }

  
}
