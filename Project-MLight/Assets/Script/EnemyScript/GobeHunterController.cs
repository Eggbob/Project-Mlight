using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using UnityEngine.UI;


public class GobeHunterController : LivingEntity
{
    public enum GobeHState { None, Idle, Patrol, Chase, Wait, Attack, GetBack, Stun, Die };

    [Header("기본속성")]
    public GobeHState ghstate = GobeHState.None; // 고블린 상태 체크 변수
    public float MoveSpeed = 1f; // 이동속도

    public Vector3 targetPos; // 타겟의 위치
    public Vector3 patrolPos;
    public Skill HunterAttack;
    public float AttackRange; // 공격범위
    private Vector3 prevPosition;

    private Animator anim;
    private Rigidbody rigid;
    private NavMeshAgent nav;
    public Image hpBar; //hp바

    [SerializeField]
    private float chaseTime = 0f; // 추적할시간
    private float timer = 0;

    [Header("공격범위 속성")]
    public float angleRange = 45f;
    private bool isCollision = false;//공격범위 충돌 확인
    Color blue = new Color(0f, 0f, 1f, 0.2f);
    Color red = new Color(1f, 0f, 0f, 0.2f);
    float dotValue = 0f;
    Vector3 direction;

    private bool move; // 움직임 관련 변수
    private bool attack; // 공격 관련 변수
    Vector3 lookAtPosition; //캐릭터가 바라볼 위치

    private bool hasTarget
    {
        get
        {
            if (target != null)
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
        HunterAttack.LCon = this;
        nav.updateRotation = false;
        nav.speed = MoveSpeed;
    }

    public void OnEnable()
    {
        statusInit(); //스테이터스 초기화
        ghstate = GobeHState.Idle; // 상태를 유휴상태로 변환
        nav.isStopped = true;
        hpBar.rectTransform.localScale = new Vector3(1f, 1f, 1f); //hp바 초기 상태 설정
    }

    void CheckState()
    {
        if (dead) { return; }


        switch (ghstate)
        {
            case GobeHState.Idle:
                IdleUpdate();
                break;

            case GobeHState.Wait:
                WaitUpdate();
                break;

            case GobeHState.Chase:
                ChaseUpdate();
                break;

            case GobeHState.Patrol:
                PatrolUpdate();
                break;

            case GobeHState.Attack:

                AttackUpdate();
                break;

            case GobeHState.GetBack:
                GetBackUpdate();
                break;

            case GobeHState.Die:

                break;
        }
    }

    void AnimationState()
    {
        switch (ghstate)
        {
            case GobeHState.Idle:
            case GobeHState.Wait:
                move = false;
                attack = false;
                break;

            case GobeHState.Chase:
            case GobeHState.Patrol:
            case GobeHState.GetBack:
                move = true;
                attack = false;
                break;

            case GobeHState.Attack:
                move = false;
                attack = true;
                break;

            case GobeHState.Die:
                move = false;
                attack = false;
                break;
        }
    }

    void IdleUpdate() //대기 상태
    {

        if (hasTarget) //타겟이 있으면
        {
            ghstate = GobeHState.Chase; // 고블린 상태를 추적으로 변환        
        }

        else // 타겟이 없으면
        {

            nav.isStopped = true; //네비 멈추기        

            PatrolCheck(); //정찰할 지점 정하기

            ghstate = GobeHState.Patrol;  // 정찰상태로 변환

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

        chaseTime += Time.deltaTime; //추적 시간 갱신

        if (Vector3.Distance(this.transform.position, patrolPos) <= AttackRange) // 순찰지점까지 도착하면
        {
            ghstate = GobeHState.Wait;
            chaseTime = 0;
        }

        nav.isStopped = false;
        nav.SetDestination(patrolPos);


        lookAtPosition = new Vector3(patrolPos.x, this.transform.position.y, patrolPos.z); //이동시 바라볼 방향체크
        this.transform.LookAt(lookAtPosition);


        if (chaseTime >= 5f) //추적 시간이 8초를 넘겼을시
        {
            chaseTime = 0f;
            target = null; //타겟을 없애기
            ghstate = GobeHState.Idle;
            return;
        }

    }

    void WaitUpdate() // 대기 동작
    {
        float waitTime = Random.Range(0.01f, 1f);

        anim.SetFloat("waitTime", waitTime);

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
            anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            ghstate = GobeHState.Idle;
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

            if (isCollision) // 적이 공격범위 내에 들어왔을시
            {

                chaseTime = 0f;
                ghstate = GobeHState.Attack; // 공격 상태로 변환                
                return;
            }
            else //적이 공격범위 내에 없을때 
            {
                if (chaseTime >= 8f) //추적 시간이 8초를 넘겼을시
                {
                    chaseTime = 0f;
                    target = null; //타겟을 없애기
                    ghstate = GobeHState.GetBack; // 복귀 상태로 변환                               
                    return;
                }
            }

            lookAtPosition = new Vector3(targetPos.x, this.transform.position.y, targetPos.z); //이동시 바라볼 방향체크

        }

        nav.isStopped = false;
        nav.SetDestination(lookAtPosition);

    }

    void OnSetTarget(GameObject _target) //타겟 지정
    {
        if (ghstate == GobeHState.GetBack || hasTarget) //귀환 상태일때는 리턴해주기
        {
            return;
        }

        prevPosition = this.transform.position;
        target = _target;
        targetPos = target.transform.position;
        //타겟을 향해 이동하는 상태로 전환
        ghstate = GobeHState.Chase;
    }




    void AttackUpdate() // 공격시
    {

        if (!isCollision) //공격범위보다 멀면
        {
            ghstate = GobeHState.Chase; // 추적상태로 변환            
        }
        else // 공격범위 내에 들어가면
        {
            nav.isStopped = true;
            nav.velocity = Vector3.zero;
            rigid.velocity = Vector3.zero;
            transform.LookAt(target.transform);
        }

    }

    public void GetBackUpdate() //귀환 상태일때
    {

        if (Vector3.Distance(this.transform.position, prevPosition) <= AttackRange)// 귀환을 완료할시
        {
            ghstate = GobeHState.Wait; //대기 상태로 변환
            target = null; // 타겟 없애기
            nav.speed = MoveSpeed; //이동 속도 원상태로 복귀
            return;
        }

        nav.speed = 15f; //귀환 속도 지정
        nav.isStopped = false;
        nav.SetDestination(prevPosition);
        transform.LookAt(prevPosition);


    }



    public override void OnDamage(Skill skill)
    {
        if (dead) return;

        base.OnDamage(skill);

        if (Hp <= 0 && this.gameObject.activeInHierarchy)
        {
            StartCoroutine(Die());
            Hp = 0;
        }

        else
        {
            switch (skill.sAttr)
            {
                case Skill.SkillAttr.Melee:
                    StartCoroutine(NormalDamageRoutine());//일반 공격일시
                    break;
                case Skill.SkillAttr.Stun:
                    StartCoroutine(StunRoutine(9f));
                    break;
            }
        }
        hpBar.rectTransform.localScale = new Vector3((float)Hp / (float)MaxHp, 1f, 1f);
        //hp바 설정


    }

    IEnumerator NormalDamageRoutine()
    {

        if (ghstate != GobeHState.Stun || !anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack"))
        { anim.SetTrigger("isHit"); } // 트리거 실행



        float startTime = Time.time; //시간체크

        nav.velocity = Vector3.zero;

        while (Time.time < startTime + 0.8f)
        {
            nav.velocity = Vector3.zero;
            yield return null;
        }
    }


    IEnumerator StunRoutine(float nuckTime) //스턴
    {
        nav.velocity = Vector3.zero;


        if (ghstate != GobeHState.Stun)
        { anim.SetTrigger("isStun"); }

        ghstate = GobeHState.Stun;
        float startTime = Time.time;

        while (Time.time < startTime + nuckTime)
        {
            nav.isStopped = true;
            rigid.angularVelocity = Vector3.zero;
            yield return null;
        }


        anim.SetTrigger("wakeUp");

        yield return new WaitForSeconds(0.2f);

        if (isCollision)
        {
            ghstate = GobeHState.Attack;
        }
        else
        {
            ghstate = GobeHState.Chase;
        }
    }


    IEnumerator Die() // 사망상태일시
    {

        anim.SetTrigger("Die"); // 트리거 활성화
        ghstate = GobeHState.Die;


        nav.enabled = false; // 네비 비활성화

        Collider[] enemyColliders = GetComponents<Collider>();

        // 콜라이더 다끄기
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (hasTarget && !dead)
        {
            sectorCheck();
            targetPos = target.transform.position; //타겟위치 업데이트
        }

        CheckState();
        AnimationState();


        anim.SetBool("isAttack", attack);
        anim.SetBool("isMove", move);
        anim.SetBool("hasTarget", hasTarget);
        anim.SetBool("isDead", dead);


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
