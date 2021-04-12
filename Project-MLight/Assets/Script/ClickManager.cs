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
                SetDestination(hit.point);
            }
        }
        Move();
    }

    void SetDestination(Vector3 dest)
    {
        nav.SetDestination(dest);
        destination = dest;
        isMove = true;
        anim.SetBool("isRun", true);
       
    }

    private void Move()
    {
        if(isMove)
        {
            if(nav.velocity.magnitude == 0f)
            {
                isMove = false;
                anim.SetBool("isRun", false);
                return;
            }

            var dir = new Vector3(nav.steeringTarget.x, transform.position.y, nav.steeringTarget.z)
                - transform.position;

            anim.transform.forward = dir;
        
        }

       
    }
}
