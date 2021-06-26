﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Weapon_", menuName = "Inventory System/Item Data/Weapon", order = 1)]
public class WeaponItemData : EquipItemData
{

    public int Damage => _damage;

    [SerializeField] private int _damage = 1;

    public override Item CreateItem()
    {
        return new WeaponItem(this);
    }
}
