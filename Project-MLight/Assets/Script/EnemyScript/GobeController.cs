using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class GobeController : LivingEntity
{
    public enum GobeState {None, Idle, Patrol, Wait, Move, Attack, Die};

    [Header("기본속성")]
    public GobeState gstate = GobeState.None;
    public float MoveSpeed = 1f; // 이동속도
    public LivingEntity target;
    public Vector3 targetPos;
    public LayerMask targetLayer; // 공격 대상 레이어
    public float AttackRange = 1.5f; // 공격범위
    public float fRange = 10f; // 수색범위

    private Animator anim;
    private NavMeshAgent nav;

    private bool move = false;
    private bool attack = false;

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

    [Header("공격범위 속성")]
    public float angleRange = 45f;
    private bool isCollision = false;
    Color blue = new Color(0f, 0f, 1f, 0.2f);
    Color red = new Color(1f, 0f, 0f, 0.2f);
    float dotValue = 0f;
    Vector3 direction;


    void CheckState()
    {
        switch(gstate)
        {
            case GobeState.Idle:
            case GobeState.Wait:
            
                IdleUpdate();
                break;

            case GobeState.Move:           
            case GobeState.Patrol:
                MoveUpdate();
                break;

            case GobeState.Attack:
                break;

            case GobeState.Die:
                break;
        }
    }

    void IdleUpdate()
    {
        if(target == null)
        {           
            nav.isStopped = true;
            FindNearEnemy(targetLayer);
        }
        else
        {
            gstate = GobeState.Move;
        }
    }

    void FindNearEnemy(LayerMask tlayer)
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

        if (colliderMin != null) //콜라이더가 비어있지 않으면
        {
            PlayerController livingEntity = colliderMin.GetComponent<PlayerController>();


            if (livingEntity != null && !livingEntity.dead) //찾은 리빙엔티티가 죽지않고 null값이 아닐때
            {
                target = livingEntity;
               
                targetPos = target.transform.position;
                gstate = GobeState.Move;
            }
        }
        else //콜라이더가 비어있으면 순찰
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

            gstate = GobeState.Patrol;
        }

    }

    void MoveUpdate()//이동시에 
    {
        move = true; //이동 활성화
        attack = false; // 공격 비활성화
        targetPos.y = this.transform.position.y;
        this.transform.LookAt(targetPos);

        nav.isStopped = false;
        nav.SetDestination(targetPos);
      

        if (Vector3.Distance(this.transform.position, targetPos) <= AttackRange )
        {
            switch (gstate)
            {
                case GobeState.Patrol:
                    StartCoroutine("WaitUpdate");
                    break;

                case GobeState.Move:
                    gstate = GobeState.Attack;
                    break;
            }
        }
    }

    IEnumerator WaitUpdate()
    {
        gstate = GobeState.Wait;
        FindNearEnemy(targetLayer);
        float waitTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(waitTime);
        gstate = GobeState.Idle;
    }


    void AnimationControl()
    {
        switch(gstate)
        {
            case GobeState.Wait:
            case GobeState.Idle:
                attack = false; // 공격 false
                move = false; // 이동 false
                break;

            case GobeState.Patrol:
            case GobeState.Move:
                attack = false; 
                move = true; 
                break;

            case GobeState.Attack:
                attack = true; 
                move = false; 
                break;

            case GobeState.Die:
                attack = false; // 공격 false
                move = false; // 이동 false
                break;
        }
    }

    void AttackUpdate()
    {
      
        if (Vector3.Distance(this.transform.position, targetPos) > AttackRange + 0.5f) //공격범위보다 멀면
        {
            gstate = GobeState.Move; // 추적상태로 변환
            return;
        }
        else
        {
            nav.isStopped = true;
            nav.velocity = Vector3.zero;
        }
    }

    public override void Die()
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
        AnimationControl();
        anim.SetBool("isAttack", attack);
        anim.SetBool("isMove", move);
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
