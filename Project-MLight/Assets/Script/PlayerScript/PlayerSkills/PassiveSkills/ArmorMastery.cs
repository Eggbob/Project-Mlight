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

        GameManager.Instance.Inven.SetMaxWeight(20);
        PCon.DEF += 3;  
    }

    public override void Init(LivingEntity _LCon)
    {
        this.LCon = _LCon;

        this._description = "방어력이 " + _skillPower +"증가합니다. \n"
            +"-최대 소지 무게가 " + plusWeight + "증가합니다.";

        PassiveAction();
    }

    protected override void SkillLevelUp()
    {
        plusWeight += 20;
        _skillPower += 3;

        this._description = "방어력이 " + _skillPower + "증가합니다. \n"
            + "-최대 소지 무게가 " + plusWeight + "증가합니다.";

        base.SkillLevelUp();
        PassiveAction();
    }
}
