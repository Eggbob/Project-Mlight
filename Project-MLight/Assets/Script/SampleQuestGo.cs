using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SampleQuestGo 
{
    public GoalType goalType;
    public int requireAmount;
    public int currentAmount;

    public bool IsReached()
    {
        return (currentAmount >= requireAmount);
    }

    public void EnemyKilled()
    {
        if(goalType == GoalType.Kill)
         currentAmount++;
    }

}

public enum GoalType
{
    Kill,
    Gathering
}
