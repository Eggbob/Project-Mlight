using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Potion_Item", menuName = "Inventory System/Item Data/Potion/HealthPotion", order = 3)]
public class HealthPotionItemData : PotionItemData
{

    public override Item CreateItem()
    {
        return new HealthPotionItem(this);
    }

}
