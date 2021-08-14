using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedPotionItem : PotionItem
{
    private MoveSpeedPotionItemData mdata;

    public MoveSpeedPotionItem(MoveSpeedPotionItemData data, int amount = 1) : base(data, amount) { }

    public override bool Use(LivingEntity _Lcon)
    {
        PlayerController pCon = _Lcon as PlayerController;

        mdata = Data as MoveSpeedPotionItemData;

        float moveValue = pCon.MoveSpeed * mdata.Value / 100;

        pCon.buffManager.CreateBuff(BuffManager.BuffType.Move, mdata.ApeearTime, moveValue);


        return base.Use(base.pCon);
    }

    protected override CountableItem Clone(int amount)
    {
        return new MoveSpeedPotionItem(CountableData as MoveSpeedPotionItemData, amount);
    }
}
