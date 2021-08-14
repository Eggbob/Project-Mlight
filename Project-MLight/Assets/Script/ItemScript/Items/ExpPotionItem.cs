using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExpPotionItem : PotionItem
{  
    private ExpPotionItemData edata;

    public ExpPotionItem(ExpPotionItemData data, int amount = 1) : base(data, amount) { }

    public override bool Use(LivingEntity _Lcon)
    {

        pCon = _Lcon as PlayerController;

        edata = Data as ExpPotionItemData;

        pCon.buffManager.CreateBuff(BuffManager.BuffType.Exp, edata.ApperTime, edata.Value);


        return base.Use(pCon); ;
    }

    protected override CountableItem Clone(int amount)
    {
        return new ExpPotionItem(CountableData as ExpPotionItemData, amount);
    }
}
