using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActiveSkill : Skill
{
    public int MpCost; //마나 사용량
    public float coolTime; // 재사용 대기시간
    public event Action active;

    private void Start()
    {
        active += ActiveAction;
    }

    private  void ActiveAction()
    {

    }



}
