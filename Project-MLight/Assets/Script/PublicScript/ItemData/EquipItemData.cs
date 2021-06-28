using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipItemData : ItemData
{ 
    public int Weight => weight;

    //장비 무게
    [SerializeField] private int weight = 10;

}
