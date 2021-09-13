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

    public int EnemyID => _enemyID;

    [SerializeField]
    private int _totalKillCount; //총 처치할 적 숫자

    [SerializeField]
    private int _currentKillCount; //현재 처치한 적 숫자

    [SerializeField]
    private int _enemyID;

    public bool IsComplete //적을 전부 처치했는지 
    {
        get
        {
            return _currentKillCount >= _totalKillCount;
        }
    }

    public void UpdateKillCount(Enemy enemy)// 처치 횟수 업데이트
    {

        if(_enemyID.Equals(enemy.EnemyID))
        {
            if(_currentKillCount < _totalKillCount)
            {
                _currentKillCount++;
                NotificationUI.Instance.GenerateTxt(enemy.EnemyName + " : " +  _currentKillCount + "/" + _totalKillCount);

                if (_currentKillCount == _totalKillCount)
                {
                    BgmManager.Instance.PlayEffectSound("Clear");
                    NotificationUI.Instance.GenerateTxt(enemy.EnemyName + " (처치 완료)");
                }
            }         
            QuestManager.Instance.CheckComplete();
        }
    }
}

