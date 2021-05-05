using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public static PlayerSkillController instance; //싱글톤을 위한 instance

    public List<Skill> PlayerSkills; //플레이어의 모든 스킬

    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

   
    }

    


}
