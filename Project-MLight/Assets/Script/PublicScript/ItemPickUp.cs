using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Object
{
    [SerializeField]
    private ItemData item;
    [SerializeField]
    private int amount;

    private IEnumerator GetBackRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(false);
        ItemObjectPool.ReturnItem(this.gameObject, item.ID);
    }

    public void Init(ItemData _item, int _amount)
    {
        item = _item;
       

        amount = _amount;
    }

    public (ItemData, int) Drop()
    {
        //this.gameObject.SetActive(false);
        StartCoroutine(GetBackRoutine());

        return (item, amount);
    }

    

}
