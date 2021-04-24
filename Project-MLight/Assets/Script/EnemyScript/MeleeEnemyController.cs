using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyController : LivingEntity
{
    public enum GobeState { None, Idle, Patrol, Wait, Chase, Attack, ReturnPos, Die };

    [Header("기본속성")]
    public GobeState gstate = GobeState.None; // 고블린 상태 체크 변수
    public float MoveSpeed = 1f; // 이동속도
    public LivingEntity target = null; //  타겟
    public Transform targetTransform = null;
    public Vector3 targetPos = Vector3.zero; // 타겟의 위치
    
    public LayerMask targetLayer; // 공격 대상 레이어
    public float AttackRange; // 공격범위
    public float fRange; // 수색범위
    private Vector3 prevPosition; // 귀환할 위치

    private Animator anim; //애니메이터
    private NavMeshAgent nav; // 네비게이션

    [Header("공격범위 속성")]
    public float angleRange = 45f;
    private bool isCollision = false;//공격범위 충돌 확인
    Color blue = new Color(0f, 0f, 1f, 0.2f);
    Color red = new Color(1f, 0f, 0f, 0.2f);
    float dotValue = 0f;
    Vector3 direction;


    private bool move; // 움직임 관련 변수
    private bool attack; // 공격 관련 변수

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


    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        statusInit(); //스테이터스 초기화
        gstate = GobeState.Idle; // 상태를 유휴상태로 변환
        nav.isStopped = true; //네비게이션 멈추기
    }

    void CheckState()
    {
        switch (gstate)
        {
            case GobeState.Idle:
                IdleUpdate(); 
                break;
            case GobeState.Chase:
                ChaseUpdate();
                break;
            case GobeState.Patrol:
                MoveUpdate(); 
                break;
            case GobeState.Attack:
               
                break;
        }
    }


    void IdleUpdate()
    {
        if(hasTarget) //만약 타겟이 있다면
        {
            gstate = GobeState.Chase;
        }
        else
        {
            PatrolCheck(); // 정찰할 지역 지정
            gstate = GobeState.Patrol; // 정찰 상태로 변경
        }
    }

    void PatrolCheck() // 순찰할 지점 지정
    {

        targetPos = new Vector3(transform.position.x + Random.Range(-10f, 10f),
                                   transform.position.y + 1000f,
                                   transform.position.z + Random.Range(-10f, 10f));

        Ray ray = new Ray(targetPos, Vector3.down); //임의의 위치에서 아래방향으로 레이 쏘기
        RaycastHit raycastHit = new RaycastHit(); 

        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true)
        {
            targetPos.y = raycastHit.point.y;

        }
    }


    void MoveUpdate()// 순찰 지점까지 이동시에 
    {

        Vector3 diff = Vector3.zero;
        Vector3 lookAtPosition = Vector3.zero;

        if(targetPos != Vector3.zero)
        {
            diff = targetPos - this.transform.position;

            if(diff.magnitude <= AttackRange)
            {
                StartCoroutine(WaitUpdate());
                return;
            }

            lookAtPosition = new Vector3(targetPos.x, this.transform.position.y, targetPos.z);
        }


        nav.isStopped = false;
        nav.SetDestination(targetPos);
        this.transform.LookAt(lookAtPosition);

      
    }

    IEnumerator WaitUpdate() // 대기 동작
    {
        gstate = GobeState.Wait;
        float waitTime = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(waitTime);
        gstate = GobeState.Idle;
    }


    void ChaseUpdate() // 추적시
    {
        Vector3 diff = Vector3.zero;
        Vector3 lookAtPosition = Vector3.zero;

        if(hasTarget)
        {
            diff = target.transform.position - this.transform.position;

            if(diff.magnitude <= AttackRange)
            {
                gstate = GobeState.Attack;
                return;
            }

            lookAtPosition = new Vector3(targetPos.x, this.transform.position.y, targetPos.z);
        }


        nav.isStopped = false;
        nav.SetDestination(target.transform.position);
        this.transform.LookAt(lookAtPosition);


        if (isCollision) // 적이 공격범위 내에 들어왔을시
        {
            gstate = GobeState.Attack; // 공격 상태로 변환   
            //chaseTime = 0f;
            return;
        }
        else //적이 공격범위 내에 없을때 
        {
           
        }

    }
}

