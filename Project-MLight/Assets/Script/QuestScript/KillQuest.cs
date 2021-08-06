using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 처치 퀘스트
/// </summary>
[CreateAssetMenu(fileName = "Quest", menuName = "QuestSystem/KillQuest")]
public class KillQuest : Quest
{
    public KillObject[] KillObjects => _killObjects;

    [SerializeField]
    private KillObject[] _killObjects;

    public override bool IsComplete()
    {
        foreach (KillObject k in _killObjects)
        {
            if (!k.IsComplete)
                return false;
        }
        return true;
    }
}


[System.Serializable]
public class KillObject
{
    public int TotalKillCount => _totalKillCount;

    public int CurrentKillCount => _currentKillCount;

    public string EnemyType => _enemyType;

    [SerializeField]
    private int _totalKillCount; //총 처치할 적 숫자

    [SerializeField]
    private int _currentKillCount; //현재 처치한 적 숫자

    [SerializeField]
    private string _enemyType; //적 타입


    public bool IsComplete //적을 전부 처치했는지 
    {
        get
        {
            return _currentKillCount >= _totalKillCount;
        }
    }

    public void UpdateKillCount(LivingEntity LCon)// 처치 횟수 업데이트
    {
        if(_enemyType.Equals(LCon.name))
        {
            if(_currentKillCount < _totalKillCount)
            {
                _currentKillCount++;
            }
            QuestUIManager.Instance.CheckComplete();
        }
    }
}

