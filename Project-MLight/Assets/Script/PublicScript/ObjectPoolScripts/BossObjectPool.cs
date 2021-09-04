using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObjectPool : MonoBehaviour
{
    private static BossObjectPool instance; //싱글톤 변수

    [SerializeField]
    private BossController bCon;

    [SerializeField]
    private GameObject fireballPrefab;

    [SerializeField]
    private GameObject dangerCirclePrefab;

    [SerializeField]
    private GameObject powerAttackPrefab;

    private Queue<PowerAttack> powerAttackQueue = new Queue<PowerAttack>();
    private Queue<FireBall> fireBallQueue = new Queue<FireBall>();
    private Queue<GameObject> dangerCircleQueue = new Queue<GameObject>();
  

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init(10);
    }

    private void Init(int count)
    {
        for(int i=0; i< count; i++)
        {
            fireBallQueue.Enqueue(CreateFireball());
            dangerCircleQueue.Enqueue(CreateDangerCircle());
            powerAttackQueue.Enqueue(CreatePowerBird());
        }
    }

    //화염구 생성
    private FireBall CreateFireball()
    {
        var newObj = Instantiate(fireballPrefab, transform).GetComponent<FireBall>();
        newObj.Init(bCon);
        newObj.gameObject.SetActive(false);
        return newObj;
    }
    
    //공격 범위 생성
    private GameObject CreateDangerCircle()
    {
        var newObj = Instantiate(dangerCirclePrefab, transform);
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private PowerAttack CreatePowerBird()
    {
        var newObj = Instantiate(powerAttackPrefab, transform).GetComponent<PowerAttack>();
        newObj.Init(bCon);
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    public static FireBall GetFireBall()
    {
        if(instance.fireBallQueue.Count >0)
        {
            var obj = instance.fireBallQueue.Dequeue();
            obj.transform.SetParent(null);
           // obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreateFireball();
            newObj.transform.SetParent(null);
            //newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public static void ReturnFireball(FireBall fireBall)
    {
        fireBall.gameObject.SetActive(false);
        fireBall.transform.SetParent(instance.transform);
        instance.fireBallQueue.Enqueue(fireBall);
    }

    public static GameObject GetDangerCircle()
    {
        if (instance.dangerCircleQueue.Count > 0)
        {
            var obj = instance.dangerCircleQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreateDangerCircle();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(false);
            return newObj;
        }
    }

    public static void ReturnDangerCircle(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        instance.dangerCircleQueue.Enqueue(obj);
    }

    public static PowerAttack GetPowerAttack()
    {
        if (instance.powerAttackQueue.Count > 0)
        {
            var obj = instance.powerAttackQueue.Dequeue();
            obj.transform.SetParent(null);
           // obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreatePowerBird();
            newObj.transform.SetParent(null);
           // newObj.gameObject.SetActive(false);
            return newObj;
        }
    }

    public static void ReturnPowerAttack(PowerAttack pBird)
    {
        pBird.gameObject.SetActive(false);
        pBird.transform.SetParent(instance.transform);
        instance.powerAttackQueue.Enqueue(pBird);
    }

}
