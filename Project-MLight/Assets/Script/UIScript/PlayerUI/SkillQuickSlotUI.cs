using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillQuickSlotUI : MonoBehaviour
{

    [Tooltip("스킬 창 슬롯")]
    [SerializeField]
    private Button skillSlotBtn;
    private Image skillSlotImg;

    [Tooltip("퀵 슬롯")]
    [SerializeField]
    private Button quickSlotBtn;
    private Image quickSlotImg;

    private ActiveSkill skill;
    private PlayerController pCon;
    private CoolDown cool;

    private void Awake()
    {
        skillSlotImg = skillSlotBtn.transform.GetChild(0).GetComponent<Image>();
        quickSlotImg = quickSlotBtn.transform.GetChild(0).GetComponent<Image>();

        quickSlotBtn.onClick.AddListener(UseSkill);
        cool = quickSlotBtn.GetComponent<CoolDown>();
    }

    private void Start()
    {
        pCon = GameManager.Instance.Player;
    }

    private void UseSkill() //스킬 사용
    {
        if(skill != null)
        {
            switch (skill.SAttr)
            {
                case ActiveSkill.SkillAttr.Buff:
                    if (pCon.Mp < skill.MpCost)
                    {
                        Debug.Log("마나가 부족합니다");
                        return;
                    }
                    else
                    {
                        pCon.Mp -= skill.MpCost; //마나감소
                        pCon.SkillUpdate(skill.Skillid); //스킬 모션 실행                      
                        skill.ActiveAction(); //스킬 실행
                        cool.UseSpell(skill.CoolTime); //쿨타임 설정
                    }
                    break;
                default:
                    if (pCon.target == null)
                    {
                        Debug.Log("대상이 없습니다");
                    }
                    else if (pCon.target.layer != 9)
                    {
                        Debug.Log("잘못된 대상입니다");
                    }
                    else if (pCon.Mp < skill.MpCost)
                    {
                        Debug.Log("마나가 부족합니다");
                        return;
                    }
                    else
                    {
                        pCon.Mp -= skill.MpCost; //마나감소
                        pCon.SkillUpdate(skill.Skillid); //스킬 모션 실행                      
                        skill.ActiveAction(); //스킬 실행
                        cool.UseSpell(skill.CoolTime); //쿨타임 설정
                    }
                    break;
            }
        }
    }

    public void SetSkill(ActiveSkill _skill)
    {
        this.skill = _skill;

        skillSlotImg.sprite = _skill.Icon;
        quickSlotImg.sprite = _skill.Icon;
        skillSlotImg.enabled = true;
        quickSlotImg.enabled = true;
    }


   


}
