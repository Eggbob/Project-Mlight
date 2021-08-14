using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField]
    protected int _level; // 현재 레벨
    [SerializeField]
    protected int _exp; // 현재 경험치
    [SerializeField]
    protected int _maxExp; // 최대 경험치
    [SerializeField]
    protected int _hp; // 현재 Hp
    [SerializeField]
    protected int _maxHP; // 최대 Hp
    [SerializeField]
    protected int _mp; // 현재 Mp
    [SerializeField]
    protected int _maxMP; // 최대 Mp
    [SerializeField]
    protected int _power; // 공격
    [SerializeField]
    protected int _int; // 지능
    [SerializeField]
    protected int _def; // 방어도
    [SerializeField]
    protected int _statPoint; //스탯 포인트


    public int BonusPower { get; private set; } = 0;
    public float BonusInt { get; private set; } = 0;
    public float BonusDef { get; private set; } = 0;
    public float BonusHp { get; private set; } = 0;
    public float BonusMp { get; private set; } = 0;
    public float BonusExp { get; private set; } = 0;

    public int Level { get { return _level; } set { _level = value; } }
    public int Exp { get { return _exp; } set { _exp = value; } }
    public int MaxExp {  get { return _maxExp; } set { _maxExp = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp {  get { return _maxHP; } set { _maxHP = value; } }
    public int Mp {  get { return _mp; } set { _mp = value; } }
    public int MaxMp {  get { return _maxMP; } set { _maxMP = value; } }
    public int Power { get { return _power; } set { _power = value; } }
    public int Int { get { return _int; } set { _int = value; } }
    public int DEF { get { return _def; } set { _def = value; } }
    public int StatPoint { get { return _statPoint;  } set { _statPoint = value; } }
    public bool dead { get; protected set; } // 사망 상태

   
    //초기상태 설정
    public virtual void statusInit(int pLevel =1 , int pExp = 0, 
        int pHp =100, int pMp = 100, int pPower = 10, int pInt = 10, int pDef = 10 ,int pStatPoint = 0)
        
    {
        _level = pLevel;
        _exp = pExp;
        _maxExp = _level * 200;
   
        _maxHP = (100 * _level) + 100;
        _hp = pHp;

        _maxMP = (100 * _level) + 100;
        _mp = pMp;

        _power = pPower;
        _int = pInt;
        _def = pDef;
        _statPoint = pStatPoint;
    }

    public void SetBonusPower(int _power){ BonusPower += _power; }
    public void SetBonusInt(float _Int){ BonusInt = _Int; }
    public void SetBonusDef(float _def){ BonusDef = _def; }
    public void SetBonusHp(float _hp){ BonusHp = _hp; }
    public void SetBonusMp(float _mp){ BonusMp = _mp; }
    public void SetBonusExp(float _exp){ BonusExp += _exp/100 ; }

  

}
