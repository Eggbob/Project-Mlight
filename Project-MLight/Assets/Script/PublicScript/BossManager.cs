using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossManager : MonoBehaviour
{
    public GameObject UIManager; 
    public Camera roomCam;  
    public GameObject wall;

    public CameraFollow camfollow;
    public BossController boss;
    public Quest bossQuest;

    [SerializeField]
    private float shakeForce = 0f;
    [SerializeField]
    private Vector3 shakeOffset = Vector3.zero;


    private Animator wallAnim;
    private bool isEnter;
    private PlayerController pCon;

    private void Start()
    {
        pCon = GameManager.Instance.Player;
        isEnter = false;
        wallAnim = wall.GetComponent<Animator>();
    }


    //보스룸 오픈
    private IEnumerator RoomOpen()
    {
        isEnter = true;
        roomCam.gameObject.SetActive(true);      
        UIManager.gameObject.SetActive(false);
               
        StartCoroutine(Shake());

        yield return new WaitForSeconds(1.5f);
        StopCoroutine(Shake());
       
        wall.SetActive(false);
        roomCam.gameObject.SetActive(false);
        UIManager.gameObject.SetActive(true);
        
    }
    //카메라 흔들림 효과
    private IEnumerator Shake()
    {
        Vector3 originEuler = roomCam.transform.eulerAngles;

        while(true)
        {
            float t_rotX = Random.Range(-shakeOffset.x, shakeOffset.x);
            float t_rotY = Random.Range(-shakeOffset.y, shakeOffset.y);
            float t_rotZ = Random.Range(-shakeOffset.z, shakeOffset.z);

            Vector3 t_randomRot = originEuler + new Vector3(t_rotX, t_rotY, t_rotZ);
            Quaternion t_rot = Quaternion.Euler(t_randomRot);

            while(Quaternion.Angle(roomCam.transform.rotation, t_rot)> 0.1f)
            {
                roomCam.transform.rotation = Quaternion.RotateTowards(roomCam.transform.rotation, t_rot, shakeForce * Time.deltaTime);
                yield return null;
            }

            wall.transform.Translate(Vector3.down * 70f * Time.deltaTime);
            yield return null;
        } 
    }

    private IEnumerator OperateBoss()
    {
        camfollow.target = boss.gameObject.transform;
        boss.gameObject.SetActive(true);
        UIManager.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);

        camfollow.target = pCon.gameObject.transform;
        UIManager.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") )
        {
            if (!isEnter && QuestManager.Instance.HasQuest(bossQuest))
                StartCoroutine(RoomOpen());
            else if (isEnter && !boss.gameObject.activeSelf)
                StartCoroutine(OperateBoss());

        }
    }

   
}
