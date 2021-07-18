using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExpPotionItem : PotionItem
{
   
    private ExpPotionItemData edata;

    public ExpPotionItem(ExpPotionItemData data, int amount = 1) : base(data, amount) { }

    public override bool Use(LivingEntity _Lcon)
    {

        Lcon = _Lcon;

        edata = Data as ExpPotionItemData;

        Lcon.buffManager.CreateBuff(BuffManager.BuffType.Exp, edata.ApperTime, edata.Value);


        return base.Use(Lcon); ;
    }

    //private void PortionRoutine()
    //{
      
    //    edata = Data as ExpPortionItemData;

    //    Lcon.SetBonusExp(edata.Value);

   
    //    Thread.Sleep((int)edata.ApperTime);

    //    Lcon.SetBonusExp( -edata.Value );

    //}



    protected override CountableItem Clone(int amount)
    {
        return new ExpPotionItem(CountableData as ExpPotionItemData, amount);
    }
}
