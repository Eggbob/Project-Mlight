using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Texture Icon; // 스킬 아이콘
    public string SkillName; //스킬 이름
    public string Description; //스킬 설명
    public int Skillid; //스킬 아이디

    public Skill(Skill s)
    {
        Icon = s.Icon;
        name = s.name;
        Description = s.Description;
        Skillid = s.Skillid;
    }

}
