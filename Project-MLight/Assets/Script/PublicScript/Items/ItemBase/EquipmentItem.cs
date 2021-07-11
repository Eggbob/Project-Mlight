using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//장비 아이템
public abstract class EquipmentItem : Item
{
    private int propweight;

    public EquipItemData eItemData { get; private set; }

    //아이템 무게
    public int PropWeight
    {
        get => propweight;
        set
        {
            if (value < 0) value = 0;
            if (value > eItemData.Weight)
                value = eItemData.Weight;

            propweight = value;
        }
            
    }

    public EquipmentItem(EquipItemData data) : base(data)
    {
        eItemData = data;
        PropWeight = data.Weight;
    }
}
