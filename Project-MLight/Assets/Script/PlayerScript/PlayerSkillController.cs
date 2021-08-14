using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSkillController : MonoBehaviour
{ 
    public GameObject[] PlayerSkillObject; //생성할 플레이어의 스킬 오브젝트
    public List<Skill> PlayerSkills; //플레이어의 모든 스킬

    private PlayerController pCon;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Init()
    {
        for(int i = 0; i< PlayerSkillObject.Length; i++)
        {
           var pSkill = Instantiate(PlayerSkillObject[i], this.transform.GetChild(1)).GetComponent<Skill>();
           PlayerSkills.Add(pSkill);
        }
    }

    private void Start()
    {
        pCon = GameManager.Instance.Player; 

        for(int i= 0; i< PlayerSkills.Count; i++)
        {
            PlayerSkills[i].Init(pCon);
        }

        pCon.pAttack = PlayerSkills[0] as ActiveSkill;
     
    }

    public ActiveSkill GetSkill(int slotNum)
    {
        return PlayerSkills[slotNum] as ActiveSkill;
    }
}
