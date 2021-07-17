using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ExpPortionItem : PortionItem
{
    private LivingEntity Lcon;
    private ExpPortionItemData edata;

    public ExpPortionItem(ExpPortionItemData data, int amount = 1) : base(data, amount) { }

    public override bool Use(LivingEntity _Lcon)
    {

        Lcon = _Lcon;

        edata = Data as ExpPortionItemData;

        Lcon.buffManager.CreateBuff(BuffManager.BuffType.Exp, edata.ApperTime, edata.Value);

       // Thread thread = new Thread(() => PortionRoutine());
        //thread.Start();
       
        base.Use(_Lcon);
        return true;
    }

    private void PortionRoutine()
    {
      
        edata = Data as ExpPortionItemData;

        Lcon.SetBonusExp(edata.Value);

   
        Thread.Sleep((int)edata.ApperTime);

        Lcon.SetBonusExp( -edata.Value );

    }



    protected override CountableItem Clone(int amount)
    {
        return new PortionItem(CountableData as PortionItemData, amount);
    }
}
