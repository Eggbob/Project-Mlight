using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlayerAttack : Skill
{
    void Start()
    {
        this.SkillPower = LCon.Power;
    }
}
