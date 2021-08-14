using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion_Item", menuName = "Inventory System/Item Data/Potion/ManaPotion", order = 3)]
public class ManaPotionItemData : PotionItemData
{
    public override Item CreateItem()
    {
        return new ManaPotionItem(this);
    }
}
