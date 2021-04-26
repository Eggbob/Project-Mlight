using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : LivingEntity
{
    private Status status;
    [SerializeField]
    private ClickManager cmanager;
    private Animator anim; //애니메이터 컴포넌트
  

    public enum PlayerState { Idle, Move, Attack, Skill, Drop,  Die }
    public PlayerState pState;

    

    private void Awake()
    {
        status = this.GetComponent<Status>();
        cmanager = this.GetComponent<ClickManager>();
        anim = this.GetComponentInChildren<Animator>();
        pState = PlayerState.Idle;
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
       // LookTarget();
    }
    
    void DropUpdate()
    {
      //  LookTarget();
        StartCoroutine("DropRoutine");
        
    }

    IEnumerator DropRoutine()
    {
        yield return new WaitForSeconds(2.7f);
        cmanager.isInter = false;
        pState = PlayerState.Idle;
    }


    void LookTarget() //대상 바라보기
    {
        
        Vector3 temp = cmanager.target.transform.position;
        temp.y = this.transform.position.y;
        Quaternion.LookRotation(temp);
    }


   

    private void Update()
    {
        CheckStatus();
        anim.SetBool("isRun", cmanager.isMove);
        anim.SetBool("isAttack", cmanager.isAttack);
        anim.SetBool("isInter", cmanager.isInter);
    }

  
}
