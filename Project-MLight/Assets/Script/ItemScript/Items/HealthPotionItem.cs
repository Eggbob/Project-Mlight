using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionItem : PotionItem
{
    private HealthPotionItemData edata;

    public HealthPotionItem(HealthPotionItemData data, int amount = 1) : base(data, amount) { }

    public override bool Use(LivingEntity _Lcon)
    {
        base.Use(_Lcon);

        edata = Data as HealthPotionItemData;

        pCon.RestoreHealth((int)edata.Value);
        return true;
    }

    protected override CountableItem Clone(int amount)
    {
        return new HealthPotionItem(CountableData as HealthPotionItemData, amount);
    }
}
