using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ExpPortionItem : PortionItem
{
    private LivingEntity Lcon;
    private ExpPortionItemData edata;
    private bool isApplied;

    public ExpPortionItem(ExpPortionItemData data, int amount = 1) : base(data, amount) { }

    public override bool Use(LivingEntity _Lcon)
    {

        Lcon = _Lcon;
        isApplied = false;

        Thread thread = new Thread(() => PortionRoutine());
        thread.Start();
       
        base.Use(Lcon);
        return true;
    }

    private void PortionRoutine()
    {
      
        edata = Data as ExpPortionItemData;

        Lcon.SetBonusExp(edata.Value);

   
        Thread.Sleep((int)edata.ApperTime);

        Lcon.SetBonusExp( -edata.Value );

    }

    private void ExpUpgrade(int _exp)
    {

        if(!isApplied)
        {
            int Exp = _exp / (int)edata.Value * 100;

            isApplied = true;

            Lcon.ExpGetRoutine(Exp);

            return;
        }
        else
        {
            isApplied = false;
            return;
        }
    
    }

    protected override CountableItem Clone(int amount)
    {
        return new PortionItem(CountableData as PortionItemData, amount);
    }
}
