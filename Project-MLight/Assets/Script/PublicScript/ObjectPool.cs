using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool Instance;

    [SerializeField]
    private GameObject wayPoint; //웨이포인트 원본 프리팹
    [SerializeField]
    private GameObject EnemyTargeting; // 적 타겟팅 원본프리팹
    [SerializeField] 
    private GameObject ObjectTargeting; // 오브젝트 타겟팅 원본 프리팹


    private Queue<Spin> WayPointQueue = new Queue<Spin>();
    static GameObject ETargeting; //오브젝트 풀에 담을  적 타겟팅
    static GameObject OTargeting;
    


    private void Awake()
    {
        Instance = this;
        Initialize(20);
   
    }

    private Spin CreateNewObject()
    {
        var newObj = Instantiate(wayPoint, transform).GetComponent<Spin>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }


    void Initialize(int count)
    {
        for(int i=0; i< count; i++)
        {
            WayPointQueue.Enqueue(CreateNewObject());
        }
        ETargeting = Instantiate(EnemyTargeting, transform);
        OTargeting = Instantiate(ObjectTargeting, transform);
        ETargeting.SetActive(false);
        OTargeting.SetActive(false);

    }

    public static GameObject GetTargeting(int idx) // 타겟팅 오브젝트 가져가기
    {
        if(idx == 11)
        {
            OTargeting.transform.SetParent(null);
            OTargeting.SetActive(true);

            return OTargeting;
        }
        else if(idx == 9)
        {
            ETargeting.transform.SetParent(null);
            ETargeting.SetActive(true);

            return ETargeting;
        }

        return null;
    }

  
    public static Spin GetWayPoint()
    {
        if(Instance.WayPointQueue.Count > 0)
        {
            var obj = Instance.WayPointQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(false);
            return newObj;
        }
    }


    public static void ReturnWayPoint(Spin spin)
    {
        spin.gameObject.SetActive(false);
        spin.transform.SetParent(Instance.transform);
        Instance.WayPointQueue.Enqueue(spin);
    }

    public static void ReturnTargeting(GameObject tageting)
    {
        tageting.gameObject.SetActive(false);
        tageting.transform.SetParent(Instance.transform);
    }
}


