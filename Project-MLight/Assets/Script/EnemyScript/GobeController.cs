using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class GobeController : LivingEntity
{
    public enum GobeState {None, Idle, Patrol, Chase, Wait, Attack, GetBack, Stun,  Die};

    [Header("기본속성")]
    public GobeState gstate = GobeState.None; // 고블린 상태 체크 변수
    public float MoveSpeed = 1f; // 이동속도
    public Vector3 targetPos; // 타겟의 위치
    public Vector3 patrolPos;
    public Skill GobeAttack; //일반 공격
    public float AttackRange; // 공격범위
    public Image hpBar; //hp바
  
    private Vector3 prevPosition; //귀환시 돌아갈 지점
    private Animator anim;
    private Rigidbody rigid;
    private NavMeshAgent nav;

    [SerializeField]
    private TMP_Text nameTxt;
    [SerializeField]
    private Transform dTxtPos;
    [SerializeField]
    private float chaseTime; // 추적할시간
    [SerializeField]
    private float timer = 0;

    [Header("공격범위 속성")]
    public float angleRange = 45f;
    private bool isCollision = false;//공격범위 충돌 확인
    private Color blue = new Color(0f, 0f, 1f, 0.2f);
    private Color red = new Color(1f, 0f, 0f, 0.2f);
    private float dotValue = 0f;
    private Vector3 direction;

    private bool move; // 움직임 관련 변수
    private bool attack; // 공격 관련 변수
    private Vector3 lookAtPosition; //캐릭터가 바라볼 위치

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

    private void Awake()
    {      
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        GobeAttack.LCon = this;
        nav.updateRotation = false;
        nav.speed = MoveSpeed;
        nameTxt.text = this.name;
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

    public void OnEnable()
    {
        statusInit(); //스테이터스 초기화
        gstate = GobeState.Idle; // 상태를 유휴상태로 변환
        nav.isStopped = true;
        hpBar.rectTransform.localScale = new Vector3(1f, 1f, 1f); //hp바 초기 상태 설정
    }


    private void CheckState() //적 동작 상태 제어
    {
        if (dead) { return; }

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
                StartCoroutine(AttackUpdate());
                
                break;

            case GobeState.GetBack:
                GetBackUpdate();
                break;

            case GobeState.Die:
                
                break;
        }
    }

    private void AnimationState() //애니메이션 상태 제어
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
                move = false;
                attack = false;
                break;
        }
    }

    private void IdleUpdate() //대기 상태
    {
       
        if (hasTarget) //타겟이 있으면
        {
            gstate = GobeState.Chase; // 고블린 상태를 추적으로 변환        
        }
     
        else // 타겟이 없으면
        {
        
            nav.isStopped = true; //네비 멈추기                           
            PatrolCheck(); //정찰할 지점 정하기
             
        }
    }

    private void PatrolCheck() // 순찰할 지점 지정
    {       
        patrolPos = new Vector3(transform.position.x + Random.Range(-30f, 30f),
                                   transform.position.y + 10f,
                                   transform.position.z + Random.Range(-30f, 30f));

        Ray ray = new Ray(patrolPos, Vector3.down);
        RaycastHit raycastHit = new RaycastHit();

        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true)
        {
            if(raycastHit.collider.CompareTag("Props"))
            {
                PatrolCheck();
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 5f);

                chaseTime = Random.Range(6f, 12f);

                patrolPos.y = raycastHit.point.y;

                gstate = GobeState.Patrol;  // 정찰상태로 변환
            }
           
        }
    }

    private void PatrolUpdate()// 순찰 지점까지 이동시에 
    {      
        RaycastHit raycastHit = new RaycastHit();

        timer += Time.deltaTime; //추적 시간 갱신

        if (Vector3.Distance(this.transform.position, patrolPos) <= 2f) // 순찰지점까지 도착하면
        {
            target = null; //타겟을 없애기
            gstate = GobeState.Wait;
            timer = 0;
            return; 
        }

        if (Physics.Raycast(transform.position, transform.forward , out raycastHit, 2f))
        {
            if (raycastHit.collider.CompareTag("Props"))
            {
                target = null; //타겟을 없애기
                gstate = GobeState.Wait;
                timer = 0;
                return;
            }        
        }

        if (timer >= chaseTime) //순찰 시간이 일정 시간 이상이 되었을경우
        {
            target = null; //타겟을 없애기
            gstate = GobeState.Wait;
            timer = 0;
            return;
        }

        nav.isStopped = false; 
        nav.SetDestination(patrolPos);

        lookAtPosition = new Vector3(patrolPos.x, this.transform.position.y, patrolPos.z); //이동시 바라볼 방향체크
        this.transform.LookAt(lookAtPosition); // 이동할 지점 바라보기
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.blue);
    }

    private void WaitUpdate() // 대기 동작
    {
        float waitTime = Random.Range(2f, 5f);
        timer += Time.deltaTime;
     
        if(timer >= waitTime)
        {
            gstate = GobeState.Idle;
            timer = 0;
            return;
        }

        nav.isStopped = true;
        nav.velocity = Vector3.zero;
    }

    private void ChaseUpdate() // 추적시
    {
        if (hasTarget) //타겟이 있다면
        {
            chaseTime = 6f;
            timer += Time.deltaTime; //추적 시간 갱신         
            targetPos = target.transform.position;
           
            if (isCollision) // 적이 공격범위 내에 들어왔을시
            {
                timer = 0;
                gstate = GobeState.Attack; // 공격 상태로 변환                
                return;
            }
            else //적이 공격범위 내에 없을때 
            {
                if (timer >= chaseTime) //추적 시간이 일정 시간을 넘었을시
                {                   
                    target = null; //타겟을 없애기
                    timer = 0;
                    gstate = GobeState.GetBack; // 복귀 상태로 변환                               
                    return;
                }
            }

            lookAtPosition = new Vector3(targetPos.x, this.transform.position.y, targetPos.z); //이동시 바라볼 방향체크

        }
     
        nav.isStopped = false;
        nav.SetDestination(targetPos);
        transform.LookAt(lookAtPosition);
     
    }

    private void OnSetTarget(GameObject _target) //타겟 지정
    {
        if(hasTarget) //귀환 상태일때는 리턴해주기
        {
            return;
        }

        target = _target;      
        prevPosition = this.transform.position; //귀환 지점      
        gstate = GobeState.Chase; //타겟을 향해 이동하는 상태로 전환
    }

    private IEnumerator AttackUpdate() // 공격시
    {
        lookAtPosition = new Vector3(targetPos.x, this.transform.position.y, targetPos.z); //공격시 바라볼 방향
        nav.isStopped = true;
        nav.velocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        transform.LookAt(lookAtPosition);
 
        yield return new WaitForSeconds(1f);

        if (!isCollision) //공격범위보다 멀면
        {
            gstate = GobeState.Chase; // 추적상태로 변환            
        }
    }

    private void AttackCheck() //공격 체크
    {
        LivingEntity enemytarget = target.GetComponent<LivingEntity>();

        if (enemytarget != null)
        {
            if (enemytarget.dead)
            {
                gstate = GobeState.Idle;
                return;
            }
            else
            {                          
                StartCoroutine(Damage(enemytarget));              
            }
        }
    }

    private IEnumerator Damage(LivingEntity enemyTarget)
    {
        yield return new WaitForSeconds(0.7f);
        enemyTarget.OnDamage(GobeAttack);
     
    } //데미지를 입힘

    public void GetBackUpdate() //귀환 상태일때
    {     
          
        if (Vector3.Distance(this.transform.position, prevPosition) <= AttackRange)// 귀환을 완료할시
        {
            gstate = GobeState.Wait; //대기 상태로 변환
            target = null; // 타겟 없애기
            nav.speed = MoveSpeed; //이동 속도 원상태로 복귀
            return;
        }

        nav.speed = 15f; //귀환 속도 지정
        nav.isStopped = false;
        nav.SetDestination(prevPosition);
        transform.LookAt(prevPosition);

    }

    public override void OnDamage(Skill skill) //데미지 처리
    {
        if (dead) return;

        base.OnDamage(skill);

        var dTxt = ObjectPool.GetDTxt();
        dTxt.SetText((int)skill.SkillPower);
        dTxt.transform.position = dTxtPos.position;

        //if(Hp <= 0 && this.gameObject.activeInHierarchy)
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(Die());
        //    Hp = 0;
        //}

        if(skill is ActiveSkill aSkill)
        {
            switch(aSkill.SAttr)
            {
                case ActiveSkill.SkillAttr.Melee:
                    StartCoroutine(NormalDamageRoutine());//일반 공격일시
                    break;
                case ActiveSkill.SkillAttr.Stun:
                    StartCoroutine(StunRoutine(9f));
                    break;
            }
        }
        hpBar.rectTransform.localScale = new Vector3((float)Hp / (float)MaxHp, 1f, 1f);
        //hp바 설정
    }

    private IEnumerator NormalDamageRoutine() //일반 데미지 루틴
    {
        if (gstate != GobeState.Stun || !anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack01"))
        { anim.SetTrigger("isHit");} // 트리거 실행

        float startTime = Time.time; //시간체크

        nav.velocity = Vector3.zero;

        while (Time.time < startTime + 0.8f)
        {
            nav.velocity = Vector3.zero;
            yield return null;
        }
    } 

    private IEnumerator StunRoutine(float nuckTime) //스턴
    {
        nav.velocity = Vector3.zero;

        if (gstate != GobeState.Stun )
        { anim.SetTrigger("isStun"); }

        gstate = GobeState.Stun;
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
            gstate = GobeState.Attack;
        }
        else
        {
            gstate = GobeState.Chase;
        }
    }

    protected override void Die()
    {
        base.Die();
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine() // 사망상태일시
    {
        anim.SetTrigger("Die"); // 트리거 활성화
        gstate = GobeState.Die;
        nav.enabled = false; // 네비 비활성화

        target.GetComponent<LivingEntity>().ExpGetRoutine(50);
        Collider[] enemyColliders = GetComponents<Collider>();

        // 콜라이더 다끄기
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);

    }

    private void sectorCheck() // 부챗꼴 범위 충돌
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

    //private void OnDrawGizmos() // 범위 그리기
    //{
    //    Handles.color = isCollision ? red : blue;
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, AttackRange);
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, AttackRange);

    //}
}
