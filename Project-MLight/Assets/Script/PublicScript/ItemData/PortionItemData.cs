using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portion_Item", menuName ="Inventory System/Item Data/Portion", order = 3)]
public class PortionItemData : CountableItemData
{
    // 회복량
    public float Value => _value;
    public float CoolTime => _coolTime;

    [SerializeField] private float _value;
    [SerializeField] private float _coolTime;

    public override Item CreateItem()
    {
        return new PortionItem(this);
    }
}
