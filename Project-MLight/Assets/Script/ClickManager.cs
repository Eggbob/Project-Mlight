using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ClickManager : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Animator anim;
    private NavMeshAgent nav;

    private bool isMove;
    private Vector3 destination;

    private void Awake()
    {
        camera = Camera.main;
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition),out hit ))
            {
                CheckTouch(hit);
                SetDestination(hit.point);
                var waypoint = ObjectPool.GetObject();
                waypoint.transform.position = transform.position + hit.point.normalized;

            }
        }
        Move();
    }

    void CheckTouch(RaycastHit hit)
    {
        int target = hit.collider.gameObject.layer;
        switch(target)
        {
            case 8://지형
                Debug.Log("지형클릭");
              break;
            case 9: //적
                Debug.Log("적클릭");
                break;
            case 11: // 오브젝트
                Debug.Log("오브젝트클릭");
                break;

        }
    }

    void SetDestination(Vector3 dest)
    {
        nav.SetDestination(dest);
        destination = dest;
        isMove = true;
       
        anim.SetBool("isRun", isMove);
        Debug.Log("달리는중");
    }

    private void Move()
    {
        if(isMove)
        {
            if(nav.velocity.magnitude == 0f)
            {
                isMove = false;
                anim.SetBool("isRun", isMove);
                return;
            }

            var dir = new Vector3(nav.steeringTarget.x, transform.position.y, nav.steeringTarget.z)
                - transform.position;
       

           anim.transform.forward = dir;
            //this.transform.LookAt(dir);
        }

       
    }
}
