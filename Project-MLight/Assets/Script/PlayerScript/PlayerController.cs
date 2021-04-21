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

    [Header("공격범위 속성")]
    public float angleRange = 45f;
    private bool isCollision = false;
    Color blue = new Color(0f, 0f, 1f, 0.2f);
    Color red = new Color(1f, 0f, 0f, 0.2f);
    float dotValue = 0f;
    Vector3 direction;

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


    void sectorCheck() // 부챗꼴 범위 충돌
    {
        dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));
        direction = cmanager.target.transform.position - transform.position;
        if (direction.magnitude < 2f)
        {
            if (Vector3.Dot(direction.normalized, transform.forward) > dotValue)
            {
                isCollision = true;
            }
            else
                isCollision = false;
        }
        else
            isCollision = false;
    }

    private void Update()
    {
        CheckStatus();
        anim.SetBool("isRun", cmanager.isMove);
        anim.SetBool("isAttack", cmanager.isAttack);
        anim.SetBool("isInter", cmanager.isInter);
    }

}
