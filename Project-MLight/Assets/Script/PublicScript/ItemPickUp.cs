using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Object
{
    [SerializeField]
    private ItemData item; //아이템 정보
    [SerializeField]
    private int amount; //아이템 수량


    private void OnEnable()
    {
        isDrop = false;
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(!isDrop && timer > 10f)
        {
            StartCoroutine(ReturnToPull());
        }
    }

    //풀로 프리팹 반환
    private IEnumerator ReturnToPull()
    {
        yield return new WaitForSeconds(0.1f);
        ItemObjectPool.ReturnItem(this.gameObject, item.ID);
    }

    //초기 설정
    public void Init(ItemData _item, int _amount)
    {
        item = _item;       
        amount = _amount;
    }


    //아이템 습득
    public (ItemData, int) Drop()
    {
        isDrop = true;

        if(this.gameObject.activeSelf)
        {
            StartCoroutine(ReturnToPull());
        }
        

        return (item, amount);
    }
    

}
