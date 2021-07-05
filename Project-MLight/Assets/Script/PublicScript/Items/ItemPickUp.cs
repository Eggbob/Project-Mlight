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

    public void Init(ItemData _item, int _amount)
    {
        item = _item;
       

        amount = _amount;
    }

    public (ItemData, int) Drop()
    {
        this.gameObject.SetActive(false);

        return (item, amount);
    }


}
