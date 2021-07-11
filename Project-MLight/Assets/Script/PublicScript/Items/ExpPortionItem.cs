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

        Lcon.ExpGet += ExpUpgrade;

        float timer = 0;

        Debug.Log(edata.ApperTime);
        while (timer  >= edata.ApperTime)
        {
            timer += Time.deltaTime;

            
        }
        Debug.Log(timer);
        //Thread.Sleep((int)edata.ApperTime);

        Lcon.ExpGet -= ExpUpgrade;
        Debug.Log("Out");
    }

    private void ExpUpgrade(int _exp)
    {

        if(!isApplied)
        {
            int Exp = _exp / (int)edata.Value * 100;

            Lcon.ExpGetRoutine(Exp);

            isApplied = true;
        }
        else
        {
            isApplied = false;
        }
     

    }

    protected override CountableItem Clone(int amount)
    {
        return new PortionItem(CountableData as PortionItemData, amount);
    }
}
