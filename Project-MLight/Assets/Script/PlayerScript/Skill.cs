using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string SkillName; //스킬 이름
    public int SkillId; // 스킬 식별아이디
    public int MpCost; // 스킬 마나 소모량
    public float CoolTime; // 스킬 쿨타임
    public float SkillDamage; // 스킬 데미지

   public virtual void InitSkill(string _name, int _id, int _cost, float _coolTime, float _damage)
    {
        SkillName = _name;
        SkillId = _id;
        MpCost = _cost;

    }
}
