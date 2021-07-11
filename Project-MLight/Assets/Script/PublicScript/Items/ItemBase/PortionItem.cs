using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionItem : CountableItem,  IUsableItem
{
    public PortionItem(PortionItemData data, int amount = 1) : base(data, amount) { }

    public virtual bool Use(LivingEntity Lcon)
    {
        Amount--;

        return true;
    }

    protected override CountableItem Clone(int amount)
    {
        return new PortionItem(CountableData as PortionItemData, amount);
    }
}
