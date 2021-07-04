using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillController : MonoBehaviour
{
    public static PlayerSkillController instance; //싱글톤을 위한 instance  
    public GameObject[] PlayerSkillObject; //생성할 플레이어의 스킬 오브젝트
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
        pCon = PlayerController.instance;
    

        for (int i=0; i<SkillButtons.Count; i++) //온클릭 리스너 등록
        {
            int num = int.Parse(SkillButtons[i].gameObject.name.Substring(SkillButtons[i].gameObject.name.IndexOf("_") + 1));
            CoolDown coolTime = SkillButtons[i].gameObject.GetComponent<CoolDown>();
            SkillButtons[i].onClick.AddListener(()=> SkillAction(num, coolTime));
        }

        for(int i= 0; i< PlayerSkills.Count; i++)
        {
            PlayerSkills[i].Init(pCon);
        }

        pCon.pAttack = PlayerSkills[0];
        //QuickSkill.Add(PlayerSkills[6]);
        //QuickSkill.Add(PlayerSkills[2]);
        //QuickSkill.Add(PlayerSkills[3]);
        //QuickSkill.Add(PlayerSkills[1]);
    }

    public void SkillAction(int SlotNum, CoolDown coolTime)  //스킬 사용시 호출
    {
        if (QuickSkill[SlotNum] == null)
        { return; }


        switch (QuickSkill[SlotNum].sAttr)
        {
            case Skill.SkillAttr.Buff:
                if (pCon.Mp < QuickSkill[SlotNum].MpCost)
                {
                    Debug.Log("마나가 부족합니다");
                    return;
                }
                else
                {
                    pCon.Mp -= QuickSkill[SlotNum].MpCost; //마나감소
                    pCon.SkillUpdate(QuickSkill[SlotNum].Skillid); //스킬 모션 실행                      
                    QuickSkill[SlotNum].ActiveAction(); //스킬 실행
                    coolTime.UseSpell(QuickSkill[SlotNum].CoolTime); //쿨타임 설정
                }
                break;
            default:
                if ( pCon.target == null)
                {
                    Debug.Log("대상이 없습니다");
                }
                else if(pCon.target.layer != 9)
                {
                    Debug.Log("잘못된 대상입니다");
                }
                else if (pCon.Mp < QuickSkill[SlotNum].MpCost)
                {
                    Debug.Log("마나가 부족합니다");
                    return;
                }
                else
                {
                    pCon.Mp -= QuickSkill[SlotNum].MpCost; //마나감소
                    pCon.SkillUpdate(QuickSkill[SlotNum].Skillid); //스킬 모션 실행                      
                    QuickSkill[SlotNum].ActiveAction(); //스킬 실행
                    coolTime.UseSpell(QuickSkill[SlotNum].CoolTime); //쿨타임 설정
                }
                break;
        }     
    }

    public void SkillChange(int SlotNum, int Qbtn) // 스킬 슬롯 변경시
    {
        QuickSkill[Qbtn] = PlayerSkills[SlotNum];
        Image sImage = SkillButtons[Qbtn].transform.GetChild(0).GetComponent<Image>();
        sImage.sprite = PlayerSkills[SlotNum].Icon;
        sImage.enabled = true;
    }

}
