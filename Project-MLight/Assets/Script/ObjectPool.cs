using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool Instance;

    [SerializeField]
    private GameObject wayPoint;

    private Queue<Spin> WayPointQueue = new Queue<Spin>();

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
    }

    public static Spin GetObject()
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


    public static void ReturnObject(Spin spin)
    {
        spin.gameObject.SetActive(false);
        spin.transform.SetParent(Instance.transform);
        Instance.WayPointQueue.Enqueue(spin);
    }
}


