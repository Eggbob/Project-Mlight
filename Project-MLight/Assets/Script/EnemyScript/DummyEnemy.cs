using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : LivingEntity
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnDamage(Skill skill)
    {
        Debug.Log(skill.SkillPower);
       // base.OnDamage(skill);
    }
}
