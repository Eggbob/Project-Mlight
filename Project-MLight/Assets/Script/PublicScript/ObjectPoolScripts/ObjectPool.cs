using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance; //싱글톤 변수

    [SerializeField]
    private GameObject wayPoint; //웨이포인트 원본 프리팹
    [SerializeField]
    private GameObject enemyTargeting; // 적 타겟팅 원본프리팹
    [SerializeField]
    private GameObject objectTargeting; // 오브젝트 타겟팅 원본 프리팹
    [SerializeField]
    private GameObject npcTargeting; // NPC 타겟팅 원본 프리팹

    [SerializeField]
    private GameObject lvUpEffectPrefab; //레벨업 이펙트

    [SerializeField]
    private GameObject damageTxt; //데미지 표시 텍스트 
    [SerializeField]
    private GameObject questSlotBtn; //퀘스트 슬롯
    [SerializeField]
    private GameObject notifiTxt;//알림 텍스트

    private Queue<Spin> wayPointQueue = new Queue<Spin>(); //웨이포인트 큐
    private Queue<DamageTextManager> dTextQueue = new Queue<DamageTextManager>(); //데미지 텍스트 큐
    private Queue<QuestSlotUI> qSlotQueue = new Queue<QuestSlotUI>(); //퀘스트 슬롯 큐
    private Queue<NotificationTxt> nTxtQueue = new Queue<NotificationTxt>();
    private GameObject eTargeting; //오브젝트 풀에 담을 적 타겟팅
    private GameObject oTargeting; //오브젝트 풀에 담을 사물 타겟팅
    private GameObject nTargeting; //오브젝트 풀에 담을 npc 타겟팅
    private GameObject lvEffect;

    private void Awake()
    {
        instance = this;
        Initialize(30);  
    }

    //초기설정
    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            wayPointQueue.Enqueue(CreateNewObject());
            dTextQueue.Enqueue(CreateNewTxt());
            qSlotQueue.Enqueue(CreateNewQSlot());
            nTxtQueue.Enqueue(CreateNewNTxt());
        }

        eTargeting = Instantiate(enemyTargeting, transform);
        oTargeting = Instantiate(objectTargeting, transform);
        nTargeting = Instantiate(npcTargeting, transform);
        lvEffect = Instantiate(lvUpEffectPrefab, transform);

        eTargeting.SetActive(false);
        oTargeting.SetActive(false);
        nTargeting.SetActive(false);
        lvEffect.SetActive(false);
    }


    //웨이포인트 생성
    private Spin CreateNewObject()
    {
        var newObj = Instantiate(wayPoint, transform).GetComponent<Spin>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    //데미지 텍스트 생성
    private DamageTextManager CreateNewTxt()
    {
        var newObj = Instantiate(damageTxt, transform).GetComponent<DamageTextManager>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    //퀘스트 슬롯 생성
    private QuestSlotUI CreateNewQSlot()
    {
        var newObj = Instantiate(questSlotBtn, transform).GetComponent<QuestSlotUI>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private NotificationTxt CreateNewNTxt()
    {
        var newObj = Instantiate(notifiTxt, transform).GetComponent<NotificationTxt>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    //웨이포인트 가져가기
    public static Spin GetWayPoint()
    {
        if (instance.wayPointQueue.Count > 0)
        {
            var obj = instance.wayPointQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(false);
            return newObj;
        }
    }

    //웨이포인트 회수하기
    public static void ReturnWayPoint(Spin spin)
    {
        spin.gameObject.SetActive(false);
        spin.transform.SetParent(instance.transform);
        instance.wayPointQueue.Enqueue(spin);
    }

    // 타겟팅 오브젝트 가져가기
    public static GameObject GetTargeting(int idx) 
    {
        if(idx == 10) //오브젝트 레이어 일시
        {
            instance.oTargeting.transform.SetParent(null);
            instance.oTargeting.SetActive(true);

            return instance.oTargeting;
        }
        else if(idx == 9) //적 레이어 일시
        {
            instance.eTargeting.transform.SetParent(null);
            instance.eTargeting.SetActive(true);

            return instance.eTargeting;
        }
        else if(idx == 11)
        {
            instance.nTargeting.transform.SetParent(null);
            instance.nTargeting.SetActive(true);

            return instance.nTargeting;
        }
        return null;
    }

    //타겟팅 오브젝트 회수하기
    public static void ReturnTargeting(GameObject tageting) 
    {
        tageting.gameObject.SetActive(false);
        tageting.transform.SetParent(instance.transform);
    }

    //데미지 텍스트 가져가기
    public static DamageTextManager GetDTxt()
    {
        if (instance.wayPointQueue.Count > 0)
        {
            var obj = instance.dTextQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreateNewTxt();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(false);
            return newObj;
        }
    }

    //데미지 텍스트 회수
    public static void ReturnDTxt(DamageTextManager text)
    {
        text.gameObject.SetActive(false);
        text.transform.SetParent(instance.transform);
        instance.dTextQueue.Enqueue(text);
    }

    //퀘스트 슬롯 가져가기
    public static QuestSlotUI GetQuSlot()
    {
        if (instance.qSlotQueue.Count > 0)
        {
            var obj = instance.qSlotQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreateNewQSlot();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(false);
            return newObj;
        }
    }

    //퀘스트 슬롯 회수
    public static void ReturnQuSlot(QuestSlotUI qSlot)
    {
        qSlot.gameObject.SetActive(false);
        qSlot.transform.SetParent(instance.transform);
        instance.qSlotQueue.Enqueue(qSlot);
    }
 
    //레벨업 이펙트 가져가기
    public static GameObject GetLvEffect()
    {
        instance.lvEffect.transform.SetParent(null);
        instance.lvEffect.gameObject.SetActive(true);

        return instance.lvEffect;
    }

    //레벨업 이펙트 회수하기
    public static void ReturnLvEffect(GameObject effect)
    {
        effect.gameObject.SetActive(false);
        effect.transform.SetParent(instance.transform);
    }

    public static NotificationTxt GetNotifiTxt()
    {
        if (instance.nTxtQueue.Count > 0)
        {
            var obj = instance.nTxtQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreateNewNTxt();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(false);
            return newObj;
        }
    }

    public static void ReturnNotifiTxt(NotificationTxt nTxt)
    {
        nTxt.gameObject.SetActive(false);
        nTxt.transform.SetParent(instance.transform);
        instance.nTxtQueue.Enqueue(nTxt);
    }
}


