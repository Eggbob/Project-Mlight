using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorMastery : PassiveSkill
{
  

    [SerializeField]
    protected int plusWeight;

    public override void PassiveAction()
    {
        PlayerController PCon = LCon as PlayerController;

        PCon.Inven.SetMaxWeight(20);
        PCon.DEF += 12;
     
    }

    public override void Init(LivingEntity _LCon)
    {       
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
