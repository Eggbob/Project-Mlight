using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMoveController : MonoBehaviour
{
    public enum TargetLayer : int { None, Terrian = 8 , Enemy = 9, Object = 10,  }
    public enum MoveState {None, Move, Stop }

    [SerializeField]
    private Camera mcamera; //메인카메라 컴포넌트
    [SerializeField]
    public NavMeshAgent nav; // 네비메쉬 컴포넌트
    private PlayerController pCon; // 플레이어 컨트롤러
    private Rigidbody rigid;

    private Vector3 destination; // 캐릭터가 이동할 목적지 
    private float range; // 사거리
    public TargetLayer curtarget; //현재 타겟 레이어
    private TargetLayer prevtarget; // 이전 타겟 레이어
    public MoveState mstate; 

    RaycastHit hit; //레이 캐스트 변수
    public GameObject target; //지정된 타겟
    private GameObject eTargeting; //애너미 타겟팅 프리팹
    private GameObject oTaregeting; // 오브젝트 타겟팅 프리팹

    [Header("공격범위 속성")]
    public float attackRange = 3f;
    public float angleRange = 45f;
    public bool isCollision = false;
    Color blue = new Color(0f, 0f, 1f, 0.2f);
    Color red = new Color(1f, 0f, 0f, 0.2f);
    float dotValue = 0f;
    Vector3 direction;


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

    private void Start()
    {
        mcamera = Camera.main; //메인카메라 가져오기
        nav = this.GetComponent<NavMeshAgent>(); // 네비 가져오기
        pCon = this.GetComponent<PlayerController>(); //플레이어 컨트롤러 가져오기
        rigid = this.GetComponent<Rigidbody>();
        mstate = MoveState.None;
        nav.updateRotation = false; // 네비의회전 기능 비활성화
        range = 7f;
    }


    private void Update()
    {
        if (hasTarget)
        {
            sectorCheck();
        }
       

        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭시
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {

                if (Physics.Raycast(mcamera.ScreenPointToRay(Input.mousePosition), out hit)) //카메라에서 클릭한 곳으로 레이 쏘기
                {
                    nav.velocity = Vector3.zero; //네비 속도 0으로 지정
                    CheckTouch();  // 터치한 대상 분석
                }
            }   
        }
        Move(); //움직이기     
    }

    private void CheckTouch() // 터치한 대상 분석
    {
        prevtarget = curtarget; //이전 대상을 prevtarget에 넣어주기
        curtarget = (TargetLayer)hit.collider.gameObject.layer; // targetlayer 에 터치한 대상의 레이어 지정

        ReturnTargeting(); // 타겟팅 오브젝트 오브젝트 풀로 반환하기 

        switch (curtarget)
        {
            case TargetLayer.Terrian://지형                           
                SetDestination(hit.point);
                StartCoroutine(TerrianUpdate());
                        
                break;
            case TargetLayer.Enemy: //적
               
                target = hit.collider.gameObject;  // 타겟을 적으로 지정
                SetDestination(target.transform.position);
                StartCoroutine(EnemyUpdate());
               
                break;
            case TargetLayer.Object: // 오브젝트

                target = hit.collider.gameObject; // 타겟을 오브젝트로 지정        
                SetDestination(target.transform.position);
                StartCoroutine(ObjectUpdate());
             
                break;

        }
    }

    private void ReturnTargeting() // 타겟팅 오브젝트 반환
    {
        switch(prevtarget)
        {
            case TargetLayer.Enemy:
                if (eTargeting != null)
                    ObjectPool.ReturnTargeting(eTargeting);
                break;
            case TargetLayer.Object:
                if (oTaregeting != null)
                    ObjectPool.ReturnTargeting(oTaregeting);
                break;
        }
    }


    private IEnumerator TerrianUpdate() //지형 클릭시 호출
    {
        var waypoint = ObjectPool.GetWayPoint(); //웨이포인트 가져오기
        Vector3 waytr = hit.point; //웨이포인트 생성할 위치 생성
        waytr.y += 3f;
        waypoint.transform.position = waytr; //웨이포인트 생성할 위치 지정 


        mstate = MoveState.Move;

        yield return new WaitUntil(() => Vector3.Distance(transform.position, destination) <= 0.1f);

        mstate = MoveState.Stop;
        pCon.pState = PlayerController.PlayerState.Idle; //플레이어 상태를 유휴상태로 변환

        //if (Vector3.Distance(transform.position, destination) <= 0.1f) //이동할 지점까지 다다르면
        //{
        //    mstate = MoveState.Stop;
        //    pCon.pState = PlayerController.PlayerState.Idle; //플레이어 상태를 유휴상태로 변환

        //    return;
        //}

        
    }

    private IEnumerator EnemyUpdate() // 적 클릭시 호출
    {

        eTargeting = ObjectPool.GetTargeting((int)curtarget); // 타겟팅 오브젝트 가져오기
        Transform waytr = target.GetComponent<Enemy>().targetingTr; // 타겟팅 오브젝트 위치 생성
        eTargeting.transform.position = waytr.position;//타겟팅 오브젝트 위치 지정
        eTargeting.transform.parent = target.transform;

        mstate = MoveState.Move;

        yield return new WaitUntil(() => isCollision);
        mstate = MoveState.Stop;

        pCon.pState = PlayerController.PlayerState.Attack; // 플레이어 상태를 공격으로 변환


        //if (isCollision) // 타겟과 나의 거리가 공격 범위에 충돌했을시
        //{
        //    mstate = MoveState.Stop;

        //    pCon.pState = PlayerController.PlayerState.Attack; // 플레이어 상태를 공격으로 변환
        //    return;
        //}

    }

    private IEnumerator ObjectUpdate() //오브젝트 클릭시
    {

        oTaregeting = ObjectPool.GetTargeting((int)curtarget);
        Transform waytr = target.GetComponent<Object>().targetingTr;
        oTaregeting.transform.position = waytr.position;//타겟팅 오브젝트 위치 지정
        oTaregeting.transform.parent = target.transform;

        mstate = MoveState.Move;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, target.transform.position) <= attackRange + 0.5f);

        mstate = MoveState.Stop;
        pCon.pState = PlayerController.PlayerState.Drop; //플레이어 상태를 오브젝트 활성화로 변환 

        //if (Vector3.Distance(transform.position, target.transform.position) <= attackRange + 0.5f) // 타겟과 나의 거리가 사거리 이하일 경우
        //{
        //    mstate = MoveState.Stop;
        //    pCon.pState = PlayerController.PlayerState.Drop; //플레이어 상태를 오브젝트 활성화로 변환 
        //    return;
        //}
    }

    private void SetDestination(Vector3 dest) //목적지 설정
    {
        nav.SetDestination(dest); //네비게이션 목적지 설정
        destination = dest; // 목적지 변수에 저장
        pCon.pState = PlayerController.PlayerState.Move; // 플레이어 상태를 움직임 으로 변환             
    }


    /*private void Move() 
    {
      
            switch (curtarget)
            {
                case TargetLayer.Terrian: //지형
                    if (Vector3.Distance(transform.position, destination) <= 0.1f) //이동할 지점까지 다다르면
                    {
                        navStop();
                        pCon.pState = PlayerController.PlayerState.Idle; //플레이어 상태를 유휴상태로 변환
                        
                        return;
                    }
                    break;

                case TargetLayer.Enemy: //적
                    if (isCollision) // 타겟과 나의 거리가 공격 범위에 충돌했을시
                    {
                        navStop();
               
                        pCon.pState = PlayerController.PlayerState.Attack; // 플레이어 상태를 공격으로 변환
                        return;
                    }
                    else
                    {
                       pCon.pState = PlayerController.PlayerState.Move;
                    }
                    break;

                case TargetLayer.Object: //오브젝트
                    if (Vector3.Distance(transform.position, target.transform.position) <= attackRange + 0.5f) // 타겟과 나의 거리가 사거리 이하일 경우
                    {
                        navStop();                   
                        pCon.pState = PlayerController.PlayerState.Drop; //플레이어 상태를 오브젝트 활성화로 변환 
                        return;
                    }
                    break;

                 case TargetLayer.None:
                    navStop();                         
                    break;

        }

        Vector3 lookPos = new Vector3(nav.steeringTarget.x, transform.position.y, nav.steeringTarget.z)
            - transform.position;

        pCon.anim.transform.forward = lookPos;
        nav.isStopped = false; //네비 다시 실행
    }*/

    private void Move()
    {
        switch(mstate)
        {
            case MoveState.Move:
                navStart();
                break;

            case MoveState.Stop:
                navStop();
                break;
        }
    }

    private void navStart() //네비 움직이기
    {

        Vector3 lookPos = new Vector3(nav.steeringTarget.x, transform.position.y, nav.steeringTarget.z)
            - transform.position;

        pCon.anim.transform.forward = lookPos;
        nav.isStopped = false; //네비 다시 실행
    }

    private void navStop() //네비 멈추기
    {
        nav.isStopped = true;         // 네비게이션 멈추기     
        nav.velocity = Vector3.zero;   // 네비게이션 속도 0으로 지정               
        rigid.velocity = Vector3.zero;   
    }

    private void sectorCheck() // 부챗꼴 범위 충돌
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

    public void SetSpeed(float speed)
    {
        nav.speed += speed;
    }


    //private void OnDrawGizmos() // 범위 그리기
    //{
    //    Handles.color = isCollision ? red : blue;
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, attackRange);
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, attackRange);

    //}
}
