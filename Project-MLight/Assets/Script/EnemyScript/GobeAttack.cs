﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobeAttack : Skill
{  
    void Start()
    {
        _skillPower = LCon.Power;
    }

    protected override void SkillLevelUp()
    {
        throw new System.NotImplementedException();
    }

}
