using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//장비 아이템
public abstract class EquipmentItem : Item
{
    private int _durability;


    public EquipItemData eItemData { get; private set; }

    public int PropWeight
    {
        get => _durability;
        set
        {
            if (value < 0) value = 0;
            if (value > eItemData.Weight)
                value = eItemData.Weight;

            _durability = value;
        }
            
    }

    public EquipmentItem(EquipItemData data) : base(data)
    {
        eItemData = data;
        PropWeight = data.Weight;
    }
}
