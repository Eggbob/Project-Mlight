using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillController : MonoBehaviour
{
    public static PlayerSkillController instance; //싱글톤을 위한 instance  
    public List<Skill> PlayerSkills; //플레이어의 모든 스킬
    public List<Button> SkillButtons; //플레이어 스킬 버튼 리스트
    public List<Skill> QuickSkill;
    private PlayerController pCon;


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

    private void Start()
    {
        pCon = PlayerController.instance;

        for (int i=0; i<SkillButtons.Count; i++) //온클릭 리스너 등록
        {
            int num = int.Parse(SkillButtons[i].gameObject.name.Substring(SkillButtons[i].gameObject.name.IndexOf("_") + 1));
            SkillCoolDown coolTime = SkillButtons[i].gameObject.GetComponent<SkillCoolDown>();
            SkillButtons[i].onClick.AddListener(()=> SkillAction(num, coolTime));
        }

        for(int i= 0; i< PlayerSkills.Count; i++)
        {
            PlayerSkills[i].Init(pCon);
        }

        QuickSkill.Add(PlayerSkills[1]);
        QuickSkill.Add(PlayerSkills[0]);
       
    }

    public void SkillAction(int SlotNum, SkillCoolDown coolTime)
    {
        if (pCon.target.layer.Equals("Enemy"))
        {
            Debug.Log("대상이 없습니다");
            return;
        }
        else if (pCon.Mp <  QuickSkill[SlotNum].MpCost)
        {
            Debug.Log("마나가 부족합니다");
            return;
        }
        else
        {
            pCon.Mp -= QuickSkill[SlotNum].MpCost; //마나감소
            pCon.SkillUpdate(QuickSkill[SlotNum].Skillid, QuickSkill[SlotNum].ActTime); //스킬 모션 실행                      
            QuickSkill[SlotNum].ActiveAction(); //스킬 실행
            coolTime.UseSpell(QuickSkill[SlotNum].CoolTime); //쿨타임 설정
        }

       
    }

    public void SkillChange(int SlotNum)
    {
      
    }

}
