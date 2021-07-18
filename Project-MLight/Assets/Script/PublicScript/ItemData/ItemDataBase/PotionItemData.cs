using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion_Item", menuName ="Inventory System/Item Data/Potion", order = 3)]
public class PotionItemData : CountableItemData
{
 
    public float Value => value;
    public float CoolTime => coolTime;

    [SerializeField] private float value;   // 포션 효과량
    [SerializeField] private float coolTime; //포션 쿨타임
   


    public override Item CreateItem()
    {
        return new PotionItem(this);
    }
}
