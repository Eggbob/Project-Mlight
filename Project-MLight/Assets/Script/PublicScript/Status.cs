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
    protected int _ag; // 민첩
    [SerializeField]
    protected int _def; // 방어도


    public int Level { get { return _level; } set { _level = value; } }
    public int Exp { get { return _exp; } set { _exp = value; } }
    public int MaxExp {  get { return _maxExp; } set { _maxExp = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp {  get { return _maxHP; } set { _maxHP = value; } }
    public int Mp {  get { return _mp; } set { _mp = value; } }
    public int MaxMp {  get { return _maxMP; } set { _maxMP = value; } }
    public int Power { get { return _power; } set { _power = value; } }
    public int Int { get { return _int; } set { _int = value; } }
    public int AG {  get { return _ag; } set { _ag = value; } }
    public int DEF { get { return _def; } set { _def = value; } }
    public bool dead { get; protected set; } // 사망 상태

   
    //초기상태 설정
    public void statusInit(int pHp =100, int pMp = 100, int pPower = 10, int pInt = 10, int pAg = 10, int pDef = 10 )
    {
        _level = 1;
        _maxExp = 0;
        _exp = _maxExp;
        _maxHP = pHp;
        _hp = _maxHP;
        _maxMP = pMp;
        _mp = _maxMP;
        _power = pPower;
        _int = pInt;
        _ag = pAg;
        _def = pDef;
    }

    public void LvUp()
    {
        _level++;
        int leftexp = _exp - _maxExp;
        _maxExp += 1000;
        _exp = leftexp;
        
    }
}
