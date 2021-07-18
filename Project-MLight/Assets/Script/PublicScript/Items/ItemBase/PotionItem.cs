using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : CountableItem,  IUsableItem
{
    protected LivingEntity Lcon;


    public PotionItem(PotionItemData data, int amount = 1) : base(data, amount) { }

    public virtual bool Use(LivingEntity Lcon)
    {
        Amount--;

        return true;
    }

    protected override CountableItem Clone(int amount)
    {
        return new PotionItem(CountableData as PotionItemData, amount);
    }
}
