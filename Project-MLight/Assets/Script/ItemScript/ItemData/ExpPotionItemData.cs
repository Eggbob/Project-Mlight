using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion_Item", menuName = "Inventory System/Item Data/Potion/ExpPotion", order = 3)]
public class ExpPotionItemData : PotionItemData
{

    public float ApperTime => appearTime;

    [SerializeField] private float appearTime;   // 포션 효과시간


    public override Item CreateItem()
    {
        return new ExpPotionItem(this);
    }
}
