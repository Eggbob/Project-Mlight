using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    //버프 타입
    public enum BuffType
    {
        None,
        Atk,
        Move,
        Exp,
        Done
    }

    //현재 적용중인 버프
    [SerializeField]
    private Buff[] onBuff = new Buff[(int)BuffType.Done];

    //플레이어 컨트롤러
    private LivingEntity LCon;

    //생성할 버프 프리팹
    [SerializeField]
    private GameObject buffSlotPrefab;

    [SerializeField]
    private RectTransform BuffGroup;

    [Header("생성할 버프 이미지")]
    [SerializeField]
    private Sprite atkIcon;
    [SerializeField]
    private Sprite moveIcon;
    [SerializeField]
    private Sprite expIcon;


    //버프 생성
    public void CreateBuff(BuffType type,  float duration, float value)
    {
       int index = FindAdditionalBuff(type);

        if(index != -1) //추가 가능한 버프가 있다면
        {
            onBuff[index].AddDuration(duration);
            return;
        }
        else
        {
            index = FindEmptyBuffIndex();
            SetBuff(type, duration, value, index);
         
            return;
        }
    }

    //버프 적용
    private void ApplyBuff(int index)
    {
        switch(onBuff[index].BuffType)
        {
            case BuffType.Atk:            
                LCon.SetBonusPower((int)onBuff[index].Value); 
                break;
            case BuffType.Exp:
                LCon.SetBonusExp(onBuff[index].Value);
                break;
            case BuffType.Move:
                PlayerController pCon = LCon as PlayerController;
                pCon.pmanager.SetSpeed(onBuff[index].Value);
                break;
            default:
                break;
        }
    }

    //버프 해제 시키기
    public void RemoveBuff(int index)
    {
        switch (onBuff[index].BuffType)
        {
            case BuffType.Atk:             
                LCon.SetBonusPower((int)-onBuff[index].Value);
                break;
            case BuffType.Exp:
                LCon.SetBonusExp(-onBuff[index].Value);
                break;
            case BuffType.Move:
                PlayerController pCon = LCon as PlayerController;
                pCon.pmanager.SetSpeed(-onBuff[index].Value);
                break;
            default:
                break;
        }
    }


    //버프 선택후 집어넣기
    private void SetBuff(BuffType type,  float duration, float value, int index)
    {
        switch(type)
        {
            case BuffType.Atk:
                value = Mathf.RoundToInt(LCon.Power * (value / 100));
                onBuff[index].Init(type, duration, value, index, atkIcon);              
                break;
            case BuffType.Exp:
                onBuff[index].Init(type, duration, value, index, expIcon);
                break;
            case BuffType.Move:
                onBuff[index].Init(type, duration, value, index, moveIcon);
                break;
            default:
                break;           
        }

        ApplyBuff(index);
    }

    //시간 추가 가능한 버프 찾기
    private int FindAdditionalBuff(BuffType type)
    {
        for(int i = 0; i< onBuff.Length; i++ )
        {
            if (onBuff[i] == null)
                continue;

            if (onBuff[i].BuffType.Equals(type))
                return i;
        }

        return -1;
    }

    //비어있는 버프 슬롯 찾기
    private int FindEmptyBuffIndex()
    {
        for(int i = 0; i< onBuff.Length; i++)
        {
            if (!onBuff[i].gameObject.activeSelf)
                return i;
        }
        return -1;
    }

    private void Start()
    {
        LCon = PlayerController.instance;

        for(int i = 0; i< (int)BuffType.Done; i++)
        {
            var obj = Instantiate(buffSlotPrefab);
            obj.transform.SetParent(BuffGroup);
            onBuff[i] = obj.GetComponent<Buff>();
            obj.SetActive(false);
        }
       
    }

    
}
