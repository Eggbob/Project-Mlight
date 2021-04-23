﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class GobeController : LivingEntity
{
    public enum GobeState {None, Idle, Patrol, Chase, Attack, GetBack, Die};

    [Header("기본속성")]
    public GobeState gstate = GobeState.None; // 고블린 상태 체크 변수
    public float MoveSpeed = 1f; // 이동속도
    public LivingEntity target; //  타겟
    public Vector3 targetPos; // 타겟의 위치
    public LayerMask targetLayer; // 공격 대상 레이어
    public float AttackRange; // 공격범위
    public float fRange; // 수색범위

    private Animator anim;
    private NavMeshAgent nav;
    private float chaseTime = 0f; // 추적할시간

        [Header("공격범위 속성")]
    public float angleRange = 45f;
    private bool isCollision = false;//공격범위 충돌 확인
    Color blue = new Color(0f, 0f, 1f, 0.2f);
    Color red = new Color(1f, 0f, 0f, 0.2f);
    float dotValue = 0f;
    Vector3 direction;



    private bool move // 움직임 관련 변수
    { 
        get
        {
            switch(gstate)
            {                
                case GobeState.Idle:
                    return false;

                case GobeState.Patrol:
                case GobeState.Chase:                   
                    return true;
                    
                case GobeState.Attack:
                    sectorCheck();
                    return false;

                case GobeState.GetBack:
                    return true;

                case GobeState.Die:
                    return false;
                default:
                    return false;
            }
        }
    }
    private bool attack
    { 
        get
        {
            switch (gstate)
            {
                
                case GobeState.Idle:
                    return false;

                case GobeState.Patrol:
                case GobeState.Chase:
                    return false;

                case GobeState.Attack:
                    return true;

                case GobeState.GetBack:
                    return false;

                case GobeState.Die:
                    return false;
               
                default:
                    return false;
            }
        }
    }
    private bool hasTarget
    {
        get
        {
            if (target != null && !target.dead)
            {
                return true;
            }
            return false;
        }
    }

    public void Awake()
    {
     
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    public void OnEnable()
    {
        statusInit(); //스테이터스 초기화
        gstate = GobeState.Idle; // 상태를 유휴상태로 변환
        nav.isStopped = true;           
    }


    void CheckState()
    {
        switch(gstate)
        {
            case GobeState.Idle:
                StartCoroutine(IdleUpdate());
                break;

            case GobeState.Chase:
                sectorCheck();
                ChaseUpdate();            
                break;

            case GobeState.Patrol:
                MoveUpdate();
                break;

            case GobeState.Attack:
                sectorCheck();
                AttackUpdate();
                break;

            case GobeState.GetBack:
                break;

            case GobeState.Die:
                break;
        }
    }

    IEnumerator IdleUpdate() //대기 상태
    {
        if(!hasTarget) //타겟이 현재 비어있으면
        {           
            nav.isStopped = true; //네비 멈추기
            nav.velocity = Vector3.zero; // 네비 속도 0으로 맞추기
            FindNearEnemy(targetLayer); // 근처 적 검색

            if(!hasTarget) // 적 검색후 타겟이 비어있다면.
            {
                float waitTime = Random.Range(1f, 5f); //잠시 대기 했다가
                yield return new WaitForSeconds(waitTime);
                PatrolCheck(); //순찰할 지점 설정
               
            }
            else // 적 검색후 타겟이 있다면
            {
                gstate = GobeState.Chase; // 고블린 상태를 추적으로 변환 
            }

        }
     
        else // 타겟이 있으면
        {
            gstate = GobeState.Chase; // 고블린 상태를 추적으로 변환
        }
    }

    void FindNearEnemy(LayerMask tlayer) // 가장 가까운 대상 찾기
    {

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, fRange, tlayer);//콜라이더 설정하기
        Collider colliderMin = null; // 가장가까운 대상의 콜라이더
        float fPreDist = 99999999.0f; // 가장가까운 대상 거리 float값

        //찾은대상중 가장 가까운 대상을 찾는다.
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];
            float fDist = Vector3.Distance(collider.transform.position, this.transform.position);
            //콜라이더를 통해 찾은 타겟과의 거리를 float값으로 계산

            if (colliderMin == null || fPreDist > fDist) // 조건문으로 가장 가까운 대상 찾기
                colliderMin = collider;
            fPreDist = fDist;

        }

        if (colliderMin != null ) //콜라이더가 비어있지 않으면
        {
            PlayerController livingEntity = colliderMin.GetComponent<PlayerController>();


            if (livingEntity != null && !livingEntity.dead) //찾은 리빙엔티티가 죽지않고 null값이 아닐때
            {
                target = livingEntity;
               
                targetPos = target.transform.position;
               // gstate = GobeState.Chase;
            }
        }
  

    }

    void PatrolCheck() // 순찰할 지점 지정
    {
     
        targetPos = new Vector3(transform.position.x + Random.Range(-10f, 10f),
                                   transform.position.y + 1000f,
                                   transform.position.z + Random.Range(-10f, 10f));

        Ray ray = new Ray(targetPos, Vector3.down);
        RaycastHit raycastHit = new RaycastHit();

        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true)
        {
            targetPos.y = raycastHit.point.y;
        }

        //gstate = GobeState.Patrol;
    }

    void MoveUpdate()// 순찰 지점까지 이동시에 
    { 
        targetPos.y = this.transform.position.y;
        this.transform.LookAt(targetPos);

        nav.isStopped = false;
        nav.SetDestination(targetPos);

        direction = targetPos - this.transform.position;

        FindNearEnemy(targetLayer);

        if(hasTarget)//타겟이 있다면
        {
            gstate = GobeState.Chase; // 추격 상태로 변환
        }
        else //타겟이 없다면
        {
            if (direction.sqrMagnitude <= AttackRange) // 순찰지점까지 도착하면
            {
                gstate = GobeState.Idle; // 일반 상태로 변환
            }
        }       
    }


    void ChaseUpdate() // 추적시
    {
        chaseTime += Time.deltaTime; //추적 시간 갱신

        targetPos.y = this.transform.position.y;
        this.transform.LookAt(targetPos);

        nav.isStopped = false;
        nav.SetDestination(targetPos);

        if(chaseTime >= 3f) //추적 시간이 3초를 넘겼을시
        {
            gstate = GobeState.GetBack; // 복귀 상태로 변환
            return;
        }

        else if(isCollision) // 적이 공격범위 내에 들어왔을시
        {
            gstate = GobeState.Attack; // 공격 상태로 변환
            return;
        }

    }

    void AttackUpdate() // 공격시
    {

        if (!isCollision) //공격범위보다 멀면
        {
            gstate = GobeState.Chase; // 추적상태로 변환
            return;
        }
        else // 공격범위 내에 들어가면
        {
            nav.isStopped = true;
            nav.velocity = Vector3.zero;
        }
    }

    public override void Die() // 사망상태일시
    {
        base.Die();
        Collider[] enemyColliders = GetComponents<Collider>();

        // 콜라이더 다끄기
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }
        nav.isStopped = true; //네비 멈추기
        nav.enabled = false; // 네비 비활성화
        anim.SetTrigger("isDead"); // 트리거 활성화
    }


    private void Update()
    {
        CheckState();
        anim.SetBool("isAttack", attack);
        anim.SetBool("isMove", move);
        anim.SetBool("hasTarget", hasTarget);
    }

    void sectorCheck() // 부챗꼴 범위 충돌
    {

        dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));
        direction = target.transform.position - transform.position;
        if (direction.magnitude < AttackRange)
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

    private void OnDrawGizmos() // 범위 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, fRange);

        Handles.color = isCollision ? red : blue;
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, AttackRange);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, AttackRange);

    }
}
