using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPortionItem : PortionItem
{
    private LivingEntity Lcon;
    private HealthPortionItemData edata;

    public HealthPortionItem(HealthPortionItemData data, int amount = 1) : base(data, amount) { }

    public override bool Use(LivingEntity _Lcon)
    {
        Lcon = _Lcon;

        edata = Data as HealthPortionItemData;

        Lcon.RestoreHealth((int)edata.Efficacy);

        base.Use(Lcon);

        return true;
    }

    protected override CountableItem Clone(int amount)
    {
        return new HealthPortionItem(CountableData as HealthPortionItemData, amount);
    }
}
