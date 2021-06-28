using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Prop_Item", menuName = "Inventory System/Item Data/PropItem", order = 4)]
public class PropItemData : CountableItemData
{
    public override Item CreateItem()
    {
        return new PropItem(this);
    }
}
