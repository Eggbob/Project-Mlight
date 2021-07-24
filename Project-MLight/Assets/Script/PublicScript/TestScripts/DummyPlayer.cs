using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : LivingEntity
{
    public int damage;

    private void Update()
    {
        if (Hp <= 0) // health가 0이하일경우 추가 입력 방지
            return;


        if (Input.GetMouseButtonDown(0)) //마우스클릭시 데미지 입히기(테스트용)
        {
          
            LivingEntity enemytarget = target.GetComponent<LivingEntity>();

       
          

        }
        if (Input.GetMouseButtonDown(2)) //마우스클릭시 데미지 입히기(테스트용)
        {



        }
    }


    public override void OnDamage(Skill skill)
    {
        //base.OnDamage(damage, mtype);
    }
}
