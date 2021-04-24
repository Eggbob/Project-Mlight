using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class GobeController : LivingEntity
{
    public enum GobeState {None, Idle, Patrol, Chase, Wait, Attack, GetBack, Die};

    [Header("기본속성")]
    public GobeState gstate = GobeState.None; // 고블린 상태 체크 변수
    public float MoveSpeed = 1f; // 이동속도
    public PlayerController target; //  타겟
    public Vector3 targetPos; // 타겟의 위치
    public Vector3 patrolPos;
    public float AttackRange; // 공격범위
    private Vector3 prevPosition; 

    private Animator anim;
    private Rigidbody rigid;
    private NavMeshAgent nav;
    [SerializeField]
    private float chaseTime = 0f; // 추적할시간

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

    public void Awake()
    {
        rigid = GetComponent<Rigidbody>();
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
                IdleUpdate();             
                break;

            case GobeState.Wait:
                WaitUpdate();
                break;

            case GobeState.Chase:               
                ChaseUpdate();            
                break;

            case GobeState.Patrol:
                PatrolUpdate();
                break;

            case GobeState.Attack:
               
                AttackUpdate();
                break;

            case GobeState.GetBack:
                GetBackUpdate();
                break;

            case GobeState.Die:
                break;
        }
    }

    void AnimationState()
    {
        switch (gstate)
        {
            case GobeState.Idle:
            case GobeState.Wait:           
                move = false;
                attack = false;
                break;

            case GobeState.Chase:
            case GobeState.Patrol:
            case GobeState.GetBack:
                move = true;
                attack = false;
                break;

            case GobeState.Attack:
                move = false;
                attack = true;
                break;

            case GobeState.Die:
                break;
        }
    }

    void IdleUpdate() //대기 상태
    {
       
        if (hasTarget) //타겟이 있으면
        {
            gstate = GobeState.Chase; // 고블린 상태를 추적으로 변환        
        }
     
        else // 타겟이 없으면
        {
          
            nav.isStopped = true; //네비 멈추기        
                               
            PatrolCheck(); //정찰할 지점 정하기

            gstate = GobeState.Patrol;  // 정찰상태로 변환
             
        }
    }
 
   

    void PatrolCheck() // 순찰할 지점 지정
    {     
   
        patrolPos = new Vector3(transform.position.x + Random.Range(-10f, 20f),
                                   transform.position.y + 1000f,
                                   transform.position.z + Random.Range(-10f, 20f));

        Ray ray = new Ray(patrolPos, Vector3.down);
        RaycastHit raycastHit = new RaycastHit();

        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true)
        {
            patrolPos.y = raycastHit.point.y;

            
        }
    }


    void PatrolUpdate()// 순찰 지점까지 이동시에 
    {
        Vector3 lookAtPosition = Vector3.zero;

        if (Vector3.Distance(this.transform.position, patrolPos) <= AttackRange) // 순찰지점까지 도착하면
        {
            gstate = GobeState.Wait;
        }

        nav.isStopped = false; 
        nav.SetDestination(patrolPos);


        lookAtPosition = new Vector3(patrolPos.x, this.transform.position.y, patrolPos.z); //이동시 바라볼 방향체크
        this.transform.LookAt(lookAtPosition);

    }

    void WaitUpdate() // 대기 동작
    {
        float waitTime = Random.Range(0.01f, 1f);

        anim.SetFloat("waitTime", waitTime);

        if( anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Idle01"))
        {
            gstate = GobeState.Idle;
            anim.SetFloat("waitTime", 1f);
            return;
        }

        nav.isStopped = true;
        nav.velocity = Vector3.zero;
    }


    void ChaseUpdate() // 추적시
    {
        Vector3 lookAtPosition = Vector3.zero;

        if (hasTarget)
        {
            chaseTime += Time.deltaTime; //추적 시간 갱신
            targetPos = target.transform.position;
            direction = targetPos - this.transform.position;

            if (direction.sqrMagnitude <= AttackRange + 0.5f) // 적이 공격범위 내에 들어왔을시
            {
               
                chaseTime = 0f;
                gstate = GobeState.Attack; // 공격 상태로 변환                
                return;
            }
            else //적이 공격범위 내에 없을때 
            {
                if (chaseTime >= 8f) //추적 시간이 3초를 넘겼을시
                {
                    chaseTime = 0f;
                    target = null; //타겟을 없애기
                    gstate = GobeState.GetBack; // 복귀 상태로 변환                               
                    return;
                }
            }

            lookAtPosition = new Vector3(targetPos.x, this.transform.position.y, targetPos.z); //이동시 바라볼 방향체크

        }
     
        nav.isStopped = false;
        nav.SetDestination(lookAtPosition);
     
    }


    void OnSetTarget(PlayerController _target)
    {
        if(gstate == GobeState.GetBack || hasTarget) //귀환 상태일때는 리턴해주기
        {
            return;
        }

        prevPosition = this.transform.position;
        target = _target;
        targetPos = target.transform.position;
        //타겟을 향해 이동하는 상태로 전환
        gstate = GobeState.Chase;
    }


    void AttackUpdate() // 공격시
    {       
        if (!isCollision) //공격범위보다 멀면
        {
            gstate = GobeState.Chase; // 추적상태로 변환            
        }
        else // 공격범위 내에 들어가면
        {
            
            nav.isStopped = true;
            nav.velocity = Vector3.zero;
            rigid.velocity = Vector3.zero;
        }
     
    }

    public void GetBackUpdate() //귀환 상태일때
    {     
          
        if (Vector3.Distance(this.transform.position, prevPosition) <= AttackRange)// 귀환을 완료할시
        {
            gstate = GobeState.Idle; // 일반 상태로 변환
            target = null;
            
            return;
        }

        nav.isStopped = false;
        nav.SetDestination(prevPosition);

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
        attack = false;
        move = false;
        anim.SetTrigger("isDead"); // 트리거 활성화
    }




    private void Update()
    {
        if (hasTarget)
        {
            sectorCheck();
        }

        CheckState();
        AnimationState();

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
        Handles.color = isCollision ? red : blue;
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, AttackRange);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, AttackRange);

    }
}
