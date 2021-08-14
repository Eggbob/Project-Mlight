using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion_Item", menuName = "Inventory System/Item Data/Potion/MovePotion", order = 3)]
public class MoveSpeedPotionItemData : PotionItemData
{
    public float ApeearTime => appearTime;

    [SerializeField] private float appearTime;

    public override Item CreateItem()
    {
        return new MoveSpeedPotionItem(this);
    }
}
