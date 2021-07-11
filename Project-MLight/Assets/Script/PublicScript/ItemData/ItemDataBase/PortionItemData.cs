using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portion_Item", menuName ="Inventory System/Item Data/Portion", order = 3)]
public class PortionItemData : CountableItemData
{
 
    public float Value => value;
    public float CoolTime => coolTime;

    [SerializeField] private float value;   // 포션 효과량
    [SerializeField] private float coolTime; //포션 쿨타임
   


    public override Item CreateItem()
    {
        return new PortionItem(this);
    }
}
