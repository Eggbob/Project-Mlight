using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ClickManager : MonoBehaviour
{
    [SerializeField]
    private Camera mcamera; //메인카메라 컴포넌트
    [SerializeField]
    private Animator anim; //애니메이터 컴포넌트
    private NavMeshAgent nav; // 네비메쉬 컴포넌트

    private bool isMove; // 움직임 관련 불값
    private Vector3 destination; // 캐릭터가 이동할 목적지

    private int target; //현재 타겟
    private int prevtarget; // 이전 타겟

    
    private GameObject eTargeting; //애너미 타겟팅 프리팹
    private GameObject oTaregeting; // 오브젝트 타겟팅 프리팹

    private void Awake()
    {
        mcamera = Camera.main;
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
    }

   
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {           
            RaycastHit hit;
            if(Physics.Raycast(mcamera.ScreenPointToRay(Input.mousePosition),out hit ))
            {
                nav.velocity = Vector3.zero;
                CheckTouch(hit);             
                SetDestination(hit.point);
               
             }
        }
        Move();
    }

    void CheckTouch(RaycastHit hit)
    {
        prevtarget = target;
        target = hit.collider.gameObject.layer;

        ReturnTargeting();

        switch (target)
        {
            case 8://지형
                TerrianUpdate(hit);
              break;
            case 9: //적
                EnemyUpdate(hit);
                break;
            case 11: // 오브젝트
                ObjectUpdate(hit);             
                break;

        }
    }
    
    void ReturnTargeting()
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


    void TerrianUpdate(RaycastHit hit)
    {

        var waypoint = ObjectPool.GetWayPoint();
        Vector3 waytr = hit.point;
        waytr.y += 3f;
        waypoint.transform.position = waytr;
    }

    void EnemyUpdate(RaycastHit hit)
    {
       
        eTargeting = ObjectPool.GetTargeting(target);
        Transform waytr = hit.collider.gameObject.GetComponent<Enemy>().targetingTr;
        eTargeting.transform.position = waytr.position;
       
    }

    void ObjectUpdate(RaycastHit hit)
    {
        
        oTaregeting = ObjectPool.GetTargeting(target);
        Transform waytr = hit.collider.gameObject.GetComponent<Object>().targetingTr;
        oTaregeting.transform.position = waytr.position;
    }

    void SetDestination(Vector3 dest) //목적지 설정
    {
        nav.SetDestination(dest);
        destination = dest;
        isMove = true;
        

        anim.SetBool("isRun", isMove);
        
    }

    private void Move() //직접 움직이기
    {
        if(isMove)
        {
            if(target == 9 || target == 11)
            {
                if (Vector3.Distance(transform.position, destination) <= 5f)
                {
                    Debug.Log("aaaaa");
                    isMove = false;
                    nav.velocity = Vector3.zero;
                    anim.SetBool("isRun", isMove);
                    return;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, destination) <= 0.1f)
                {
                    Debug.Log("bbb");
                    isMove = false;
                    anim.SetBool("isRun", isMove);
                    return;
                }
            }

           

            //var dir = new Vector3(nav.steeringTarget.x, transform.position.y, nav.steeringTarget.z)
              //  - transform.position;
      
           anim.transform.forward = new Vector3(nav.steeringTarget.x, transform.position.y, nav.steeringTarget.z)
                - transform.position; 
            
        }  
    }
}
