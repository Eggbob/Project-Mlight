using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;


public class ClickManager : MonoBehaviour
{
    [SerializeField]
    private Camera mcamera; //메인카메라 컴포넌트
    [SerializeField]
    private NavMeshAgent nav; // 네비메쉬 컴포넌트
    private PlayerController pCon; // 플레이어 컨트롤러
    private Rigidbody rigid; //리지드바디 컨트롤러

    public bool isMove; // 움직임 관련 불값
    public bool isInter; // 오브젝트 상호작용 관련 불값
    public bool isAttack; // 공격 관련 불값

    private Vector3 destination; // 캐릭터가 이동할 목적지 
    private int targetlayer; //현재 타겟 레이어
    private int prevtarget; // 이전 타겟 레이어

    RaycastHit hit; //레이 캐스트 변수
    public GameObject target; //지정된 타겟
    private GameObject eTargeting; //애너미 타겟팅 프리팹
    private GameObject oTaregeting; // 오브젝트 타겟팅 프리팹

    [Header("공격범위 속성")]
    public float attackRange = 3f;
    public float angleRange = 45f;
    public bool isCollision = false;
    float dotValue = 0f;
    Vector3 direction;


    private bool hasTarget
    {
        get
        {
            if (target != null )
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

        nav.updateRotation = false; // 네비의회전 기능 비활성화
    }

   
    void Update()
    {
        if(target != null)
        {          
            sectorCheck();
        }

        if(Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭시
        {
            isAttack = false;
            isInter = false;


            if(Physics.Raycast(mcamera.ScreenPointToRay(Input.mousePosition),out hit )) //카메라에서 클릭한 곳으로 레이 쏘기
            {
                nav.velocity = Vector3.zero; //네비 속도 0으로 지정
                CheckTouch();  // 터치한 대상 분석
                SetDestination(hit.point);   // 이동할 목적지 설정  
             }
        }
        Move(); //움직이기
        nav.isStopped = false; //네비 다시 실행
    }

    void CheckTouch() // 터치한 대상 분석
    {
        prevtarget = targetlayer; //이전 대상을 prevtarget에 넣어주기
        targetlayer = hit.collider.gameObject.layer; // targetlayer 에 터치한 대상의 레이어 지정

        ReturnTargeting(); // 타겟팅 오브젝트 오브젝트 풀로 반환하기 

        switch (targetlayer) 
        {
            case 8://지형              
                TerrianUpdate();
              break;
            case 9: //적
                target = hit.collider.gameObject;  // 타겟을 적으로 지정
                EnemyUpdate();
                break;
            case 11: // 오브젝트
                target = hit.collider.gameObject; // 타겟을 오브젝트로 지정
                ObjectUpdate();             
                break;

        }
    }
    
    void ReturnTargeting() // 타겟팅 오브젝트 반환
    {
        if (prevtarget == 9) // 적
        {
            if (eTargeting != null)
                ObjectPool.ReturnTargeting(eTargeting); 
        }
        else if (prevtarget == 11) // 오브젝트
        {
            if (oTaregeting != null)
                ObjectPool.ReturnTargeting(oTaregeting);
        }
    }


    void TerrianUpdate() //지형 클릭시 호출
    {
        var waypoint = ObjectPool.GetWayPoint(); //웨이포인트 가져오기
        Vector3 waytr = hit.point; //웨이포인트 생성할 위치 생성
        waytr.y += 3f; 
        waypoint.transform.position = waytr; //웨이포인트 생성할 위치 지정 
    }

    void EnemyUpdate() // 적 클릭시 호출
    {      
        eTargeting = ObjectPool.GetTargeting(targetlayer); // 타겟팅 오브젝트 가져오기
        Transform waytr = hit.collider.gameObject.GetComponent<Enemy>().targetingTr; // 타겟팅 오브젝트 위치 생성
        eTargeting.transform.position = waytr.position;//타겟팅 오브젝트 위치 지정
    }

    void ObjectUpdate() //오브젝트 클릭시
    {       
        oTaregeting = ObjectPool.GetTargeting(targetlayer);
        Transform waytr = hit.collider.gameObject.GetComponent<Object>().targetingTr;
        oTaregeting.transform.position = waytr.position;
    }

    void SetDestination(Vector3 dest) //목적지 설정
    {
        nav.SetDestination(dest); //네비게이션 목적지 설정
        destination = dest; // 목적지 변수에 저장
        isMove = true; //움직임 활성화
        pCon.pState = PlayerController.PlayerState.Move; // 플레이어 상태를 움직임 으로 변환             
    }

    
    private void Move() //직접 움직이기
    {
        if(isMove)
        {
            switch (targetlayer)
            {
                case 8: //지형
                    if (Vector3.Distance(transform.position, destination) <= 0.1f) //이동할 지점까지 다다르면
                    {
                        navStop();
                        pCon.pState = PlayerController.PlayerState.Idle; //플레이어 상태를 유휴상태로 변환
                        return;
                    }
                    break;

                case 9: //적
                    if (isCollision) // 타겟과 나의 거리가 공격 범위에 충돌했을시
                    {
                        navStop();
                        isAttack = true;
                        pCon.pState = PlayerController.PlayerState.Attack; // 플레이어 상태를 공격으로 변환
                        return;
                    }
                    break;

                case 11: //오브젝트
                    if (Vector3.Distance(transform.position, target.transform.position) <= attackRange + 0.5f) // 타겟과 나의 거리가 사거리 이하일 경우
                    {
                        navStop();
                        isInter = true;
                        pCon.pState = PlayerController.PlayerState.Drop; //플레이어 상태를 오브젝트 활성화로 변환 
                        return;
                    }
                    break;

            }        
            this.transform.forward = new Vector3(nav.steeringTarget.x, transform.position.y, nav.steeringTarget.z)
                - transform.position;
        }  
    }

    void navStop() //네비 멈추기
    {
        nav.isStopped = true;         // 네비게이션 멈추기     
        nav.velocity = Vector3.zero;   // 네비게이션 속도 0으로 지정               
        rigid.velocity = Vector3.zero;
        isMove = false;    // 움직임 비활성화
    }

    void sectorCheck() // 부챗꼴 범위 충돌
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
