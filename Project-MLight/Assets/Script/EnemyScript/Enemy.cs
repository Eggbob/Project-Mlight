using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public enum EnemyState
{
    None = 0, 
    Idle, 
    Patrol, 
    Chase, 
    Wait, 
    Attack, 
    GetBack, 
    Stun, 
    Die
}

public class Enemy : LivingEntity
{
    [Header("기본속성")]
    private EnemyState eState = EnemyState.None; //적 상태

    [SerializeField]
    protected int enemyId; //적 ID

    [SerializeField]
    protected string enemyName; // 적 개채 명

    [SerializeField]
    protected int enemyExp; //적이 가지고 있는 경험치

    [SerializeField]
    protected float attackRange; //공격범위

    [SerializeField]
    protected Skill EnemyNormalAttack; //기본공격

    protected Vector3 targetPos; // 타겟의 위치
    protected Vector3 patrolPos; // 정찰할 위치
    protected Vector3 prevPos; // 귀환시 돌아갈 위치
    protected Vector3 lookAtPos; //캐릭터가 바라볼 위치

    protected Rigidbody rigid;

    [SerializeField]
    protected TMP_Text nameTxt; //이름 txt
    [SerializeField]
    protected Transform dTxtPos; //데미지 txt
    [SerializeField]
    private float chaseTime; // 추적할시간

    private float timer = 0; //타이머

    protected bool move; // 움직임 관련 변수
    protected bool attack; // 공격 관련 변수

    public Image hpBar; //hp바
    public Transform targetingTr;

    public int EnemyID => enemyId;
    public string EnemyName => enemyName;

    [Header("공격범위 속성")]
    [SerializeField]
    public float angleRange = 45f;
    protected bool isCollision = false;//공격범위 충돌 확인
    private Color blue = new Color(0f, 0f, 1f, 0.2f);
    private Color red = new Color(1f, 0f, 0f, 0.2f);   
    protected float dotValue = 0f;
    protected Vector3 direction;

    protected bool hasTarget
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

        nav.updateRotation = false;
        nav.speed = moveSpeed;
        nameTxt.text = enemyName;
    }

    private void Start()
    {
        EnemyNormalAttack.Init(this);
    }

    private void OnEnable()
    {
        statusInit(); //스테이터스 초기화
        eState = EnemyState.Idle; // 상태를 유휴상태로 변환
        nav.isStopped = true;
        hpBar.rectTransform.localScale = new Vector3(1f, 1f, 1f); //hp바 초기 상태 설정
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

        miniMapIcon.transform.eulerAngles = new Vector3(90, 0, 0);

        anim.SetBool("isAttack", attack);
        anim.SetBool("isMove", move);
        anim.SetBool("hasTarget", hasTarget);
        anim.SetBool("isDead", dead);
    }

    private void CheckState() //적 동작 상태 제어
    {
        if (dead) { return; }

        switch (eState)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;

            case EnemyState.Wait:
                WaitUpdate();
                break;

            case EnemyState.Chase:
                ChaseUpdate();
                break;

            case EnemyState.Patrol:
                PatrolUpdate();
                break;

            case EnemyState.Attack:
                StartCoroutine(AttackUpdate());
                break;

            case EnemyState.GetBack:
                GetBackUpdate();
                break;

            case EnemyState.Die:
                break;
        }
    }


    private void AnimationState() //애니메이션 상태 제어
    {
        switch (eState)
        {
            case EnemyState.Idle:
            case EnemyState.Wait:
                move = false;
                attack = false;
                break;

            case EnemyState.Chase:
            case EnemyState.Patrol:
            case EnemyState.GetBack:
                move = true;
                attack = false;
                break;

            case EnemyState.Attack:
                move = false;
                attack = true;
                break;

            case EnemyState.Die:
                move = false;
                attack = false;
                break;
        }
    }

    private void IdleUpdate() //대기 상태
    {
        if (hasTarget) //타겟이 있으면
        {
            eState = EnemyState.Chase; // 고블린 상태를 추적으로 변환        
        }

        else // 타겟이 없으면
        {

            nav.isStopped = true; //네비 멈추기                           
            PatrolCheck(); //정찰할 지점 정하기

        }
    }

    private void PatrolCheck() // 순찰할 지점 지정
    {
        patrolPos = new Vector3(transform.position.x + Random.Range(-20f, 20f),
                                   transform.position.y + 10f,
                                   transform.position.z + Random.Range(-20f, 20f));

        Ray ray = new Ray(patrolPos, Vector3.down);
        RaycastHit raycastHit = new RaycastHit();

        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true)
        {
            if (raycastHit.collider.CompareTag("Plant"))
            {           
                PatrolCheck();
            }
            else
            {
                chaseTime = Random.Range(6f, 12f); //정찰할 시간 정하기

                patrolPos.y = raycastHit.point.y;

                eState = EnemyState.Patrol;  // 정찰상태로 변환
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
            eState = EnemyState.Wait;
            timer = 0;
            return;
        }

        Debug.DrawRay(transform.position, transform.forward * 7, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, 7f))
        {
            if (raycastHit.collider.CompareTag("Plant"))
            {
                target = null; //타겟을 없애기
                eState = EnemyState.Wait;
                timer = 0;
                return;
            }
        }

        if (timer >= chaseTime) //순찰 시간이 일정 시간 이상이 되었을경우
        {
            target = null; //타겟을 없애기
            eState = EnemyState.Wait;
            timer = 0;
            return;
        }

        nav.isStopped = false;
        nav.SetDestination(patrolPos);

        lookAtPos = new Vector3(patrolPos.x, this.transform.position.y, patrolPos.z); //이동시 바라볼 방향체크
        this.transform.LookAt(lookAtPos); // 이동할 지점 바라보기      
    }

    private void WaitUpdate() // 대기 동작
    {
        float waitTime = Random.Range(5f, 9f);
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            eState = EnemyState.Idle;
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
                eState = EnemyState.Attack; // 공격 상태로 변환                
                return;
            }
            else //적이 공격범위 내에 없을때 
            {
                if (timer >= chaseTime) //추적 시간이 일정 시간을 넘었을시
                {
                    target = null; //타겟을 없애기
                    timer = 0;
                    eState = EnemyState.GetBack; // 복귀 상태로 변환                               
                    return;
                }
            }

            lookAtPos = new Vector3(targetPos.x, this.transform.position.y, targetPos.z); //이동시 바라볼 방향체크
        }

        nav.isStopped = false;
        nav.SetDestination(targetPos);
        transform.LookAt(lookAtPos);
    }

    private void OnSetTarget(GameObject _target) //타겟 지정
    {
        if (hasTarget || _target.GetComponent<LivingEntity>().dead) //타겟이 있다면
        {
            return;
        }

        target = _target;
        prevPos = this.transform.position; //귀환 지점      
        eState = EnemyState.Chase; //타겟을 향해 이동하는 상태로 전환
    }

    private void AttackCheck() //공격 체크
    {   
        if (target != null)
        {
            LivingEntity enemytarget = target.GetComponent<LivingEntity>();

            if (enemytarget.dead)
            {
                target = null;
                return;
            }
            else
            {
                StartCoroutine(Damage(enemytarget));
            }
        }
    }

    private IEnumerator AttackUpdate() // 공격시
    {
        if (target == null)
        {
            eState = EnemyState.GetBack;
            StopAllCoroutines();
        }

        lookAtPos = new Vector3(targetPos.x, this.transform.position.y, targetPos.z); //공격시 바라볼 방향
        nav.isStopped = true;
        nav.velocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        transform.LookAt(lookAtPos);

        yield return new WaitForSeconds(1f);

        if (!isCollision) //공격범위보다 멀면
        {
            eState = EnemyState.Chase; // 추적상태로 변환            
        }
    }

    private IEnumerator Damage(LivingEntity enemyTarget) //데미지를 입힘
    {
        yield return new WaitForSeconds(0.7f);
        enemyTarget.OnDamage(EnemyNormalAttack);

    }

    private void GetBackUpdate() //귀환 상태일때
    {
        if (Vector3.Distance(this.transform.position, prevPos) <= attackRange)// 귀환을 완료할시
        {
            eState = EnemyState.Wait; //대기 상태로 변환
            target = null; // 타겟 없애기
            nav.speed = moveSpeed; //이동 속도 원상태로 복귀
            return;
        }

        nav.speed = 15f; //귀환 속도 지정
        nav.isStopped = false;
        nav.SetDestination(prevPos);
        transform.LookAt(prevPos);
    }

    public override void OnDamage(Skill skill) //데미지 처리
    {
        if (dead) return;

        base.OnDamage(skill);

        var dTxt = ObjectPool.GetDTxt();
        dTxt.SetText((int)skill.SkillPower);
        dTxt.transform.position = dTxtPos.position;

        if (skill is ActiveSkill aSkill && !dead)
        {
            switch (aSkill.SAttr)
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
        if (eState != EnemyState.Stun || !anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack01"))
        { anim.SetTrigger("isHit"); } // 트리거 실행

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

        if (eState != EnemyState.Stun)
        { anim.SetTrigger("isStun"); }

        eState = EnemyState.Stun;
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
            eState = EnemyState.Attack;
        }
        else
        {
            eState = EnemyState.Chase;
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
        eState = EnemyState.Die;
        nav.enabled = false; // 네비 비활성화

        target.GetComponent<LivingEntity>().ExpGetRoutine(enemyExp);
        Collider[] enemyColliders = GetComponents<Collider>();

        // 콜라이더 다끄기
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }

    protected virtual void sectorCheck() // 부챗꼴 범위 충돌
    {
        dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));
        direction = target.transform.position - transform.position;
        if (direction.magnitude < attackRange)
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
}
