using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedPotionItem : PotionItem
{
    private AttackSpeedPotionItemData adata;

    public AttackSpeedPotionItem(AttackSpeedPotionItemData data , int amount = 1) : base(data, amount) { }

    public override bool Use(LivingEntity _Lcon)
    {
        PlayerController pCon = _Lcon as PlayerController;

        adata = Data as AttackSpeedPotionItemData;

        float moveValue = pCon.AtkSpeed * adata.Value / 100;

        pCon.buffManager.CreateBuff(BuffManager.BuffType.AtkSpeed, adata.ApeearTime, moveValue);

        return base.Use(base.pCon);
    }

    protected override CountableItem Clone(int amount)
    {
        return new AttackSpeedPotionItem(CountableData as AttackSpeedPotionItemData, amount);
    }
}
