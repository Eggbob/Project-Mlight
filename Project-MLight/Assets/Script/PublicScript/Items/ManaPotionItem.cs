using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotionItem : PotionItem
{
    private ManaPotionItemData mdata;

    public ManaPotionItem(ManaPotionItemData data, int amount = 1) : base(data, amount) { }

    public override bool Use(LivingEntity _Lcon)
    {
        Lcon = _Lcon;

        mdata = Data as ManaPotionItemData;

        Lcon.RestoreMana((int)mdata.Value);

        return base.Use(Lcon);
    }


    protected override CountableItem Clone(int amount)
    {
        return base.Clone(amount);
    }
}
