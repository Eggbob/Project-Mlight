using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropItem : CountableItem
{
    public PropItem(PropItemData data, int amount = 1) : base(data, amount) { }

    protected override CountableItem Clone(int amount)
    {
        return new PotionItem(CountableData as PotionItemData, amount);
    }
}
