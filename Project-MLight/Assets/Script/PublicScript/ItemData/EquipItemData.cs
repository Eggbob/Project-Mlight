using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipItemData : ItemData
{ 
    public int Weight => _weight;

    [SerializeField] private int _weight = 100;

}
