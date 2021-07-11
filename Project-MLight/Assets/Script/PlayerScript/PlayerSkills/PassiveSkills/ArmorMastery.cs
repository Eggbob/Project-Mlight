using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorMastery : PassiveSkill
{
    private new PlayerController LCon;

    [SerializeField]
    protected int plusWeight;

    public override void PassiveAction()
    {
        LCon.Inven.SetMaxWeight(20);
        LCon.DEF += 12;
     
    }

    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon as PlayerController; 

        PassiveAction();
    }

    protected override void SkillLevelUp()
    {

        plusWeight += 20;
        _skillPower += 12;

        base.SkillLevelUp();
        PassiveAction();
    }
}
