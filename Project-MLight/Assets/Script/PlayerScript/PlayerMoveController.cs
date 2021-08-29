using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class PlayerMoveController : MonoBehaviour
{
    public enum TargetLayer : int { None, Terrian = 8 , Enemy = 9, Object = 10, NPC = 11,   }
    public enum MoveState {None, Move, Stop }

    [SerializeField]
    private Camera mcamera; //메인카메라 컴포넌트
    private PlayerController pCon; // 플레이어 컨트롤러

    private Transform targetingTr; //타겟팅 오브젝트 트랜스폼
    private Vector3 destination; // 캐릭터가 이동할 목적지 
    private int playerMask; //플레이어 레이어마스크
    private TargetLayer prevtarget; // 이전 타겟 레이어
    private RaycastHit hit; //레이 캐스트 변수

    private GameObject eTargeting; //애너미 타겟팅 프리팹
    private GameObject oTaregeting; // 오브젝트 타겟팅 프리팹
    private GameObject nTargeting; //npc 타겟팅 프리팹

    public TargetLayer curtarget; //현재 타겟 레이어
    public MoveState mstate; //이동 상태

    public GameObject target { get; private set; } //지정된 타겟
    public NavMeshAgent nav { get; private set; } // 네비메쉬 컴포넌트
    public Rigidbody rigid { get; private set; }

    [Header("공격범위 속성")]
    [SerializeField]
    private float attackRange = 3f;
    [SerializeField]
    private float angleRange = 45f;
    public bool isCollision { get; private set; }
    private float dotValue = 0f;
    private Vector3 direction;

    //타겟 유무 확인
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
        playerMask = (-1) - (1 << LayerMask.NameToLayer("Player") ); //플레이어 레이어 번호 연산
   
    }


    private void Update()
    {
        if (hasTarget)
        {
            sectorCheck();
        }
      
        if (Input.GetMouseButtonDown(0)) // 화면 클릭시
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(mcamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity ,playerMask) && !pCon.dead) //카메라에서 클릭한 곳으로 레이 쏘기
                {
                    nav.velocity = Vector3.zero; //네비 속도 0으로 지정
                    CheckTouch();  // 터치한 대상 분석
                }
            }   
        }
        Move(); //움직이기     
    }

    //private bool IsPointerOverUIObject()
    //{
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    //    return results.Count > 0;
    //}

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

            case TargetLayer.NPC: // Npc
                target = hit.collider.gameObject; // 타겟을 오브젝트로 지정       
                SetDestination(target.transform.position);
                StartCoroutine(NpcUpdate());

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

            case TargetLayer.NPC:
                if (nTargeting != null)
                    ObjectPool.ReturnTargeting(nTargeting);
                break;
        }

        targetingTr = null;
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

    }

    private IEnumerator EnemyUpdate() // 적 클릭시 호출
    {
        eTargeting = ObjectPool.GetTargeting((int)curtarget); // 타겟팅 오브젝트 가져오기
        targetingTr = target.GetComponent<Enemy>().targetingTr; // 타겟팅 오브젝트 위치 생성
        eTargeting.transform.position = targetingTr.position;//타겟팅 오브젝트 위치 지정
        eTargeting.transform.parent = target.transform;

        mstate = MoveState.Move;

        yield return new WaitUntil(() => isCollision);
        mstate = MoveState.Stop;

        pCon.pState = PlayerController.PlayerState.Attack; // 플레이어 상태를 공격으로 변환

    }

    private IEnumerator ObjectUpdate() //오브젝트 클릭시
    {
        oTaregeting = ObjectPool.GetTargeting((int)curtarget);
        targetingTr = target.GetComponent<Object>().targetingTr;
        oTaregeting.transform.position = targetingTr.position;//타겟팅 오브젝트 위치 지정
        oTaregeting.transform.parent = target.transform;

        mstate = MoveState.Move;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, target.transform.position) <= attackRange + 0.5f);

        mstate = MoveState.Stop;
        pCon.pState = PlayerController.PlayerState.Drop; //플레이어 상태를 오브젝트 활성화로 변환 

    }

    private IEnumerator NpcUpdate()//Npc클릭시
    {
        NpcController nCon = target.GetComponent<NpcController>();

        nTargeting = ObjectPool.GetTargeting((int)curtarget);
        targetingTr = nCon.targetingTr;
        nTargeting.transform.position = targetingTr.position;//타겟팅 오브젝트 위치 지정
        nTargeting.transform.parent = target.transform;

        mstate = MoveState.Move;

        yield return new WaitUntil(() => isCollision);

        mstate = MoveState.Stop;
        nCon.Interact();
        pCon.pState = PlayerController.PlayerState.Idle; // 플레이어 상태를 대기상태로 변환

        yield return new WaitUntil(() => mstate == MoveState.Move);
        nCon.StopInteract();
    }

    public void SetDestination(Vector3 dest) //목적지 설정
    {
        nav.SetDestination(dest); //네비게이션 목적지 설정
        destination = dest; // 목적지 변수에 저장
        pCon.pState = PlayerController.PlayerState.Move; // 플레이어 상태를 움직임 으로 변환             
    }


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

        if(lookPos != Vector3.zero)
        {
            pCon.transform.forward = lookPos;
        }
       
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

}
