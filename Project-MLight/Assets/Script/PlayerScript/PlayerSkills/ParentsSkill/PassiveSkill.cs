using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : Skill
{
    public virtual void PassiveAction() // 패시브 스킬일 경우
    {

    }

    protected override void SkillLevelUp()
    {
        float remainSkillExp = SkillExp - MaxSkillExp;
        _skillExp = remainSkillExp;
        _skillLevel++;
        _maxSkillExp *= 2;

    }
}
