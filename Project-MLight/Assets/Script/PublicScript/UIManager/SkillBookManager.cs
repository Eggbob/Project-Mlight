using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkillBookManager : MonoBehaviour
{
    public GameObject sButtonTemp; //스킬 버튼 템플릿
    public GameObject ActiveskillList;
    public GameObject PassiveSkillList;
    PlayerSkillController psCon; //플레이어 스킬 컨트롤러
    

    private void Start()
    {
        psCon = PlayerSkillController.instance;
        SkillButtonInit();
    }

    private void SkillButtonInit()
    {
        GameObject sButton;
        for(int i = 0; i< psCon.PlayerSkills.Count; i++)
        {
            switch (psCon.PlayerSkills[i].sType)
            {
                case Skill.SkillType.Active:
                    sButton = Instantiate(sButtonTemp, ActiveskillList.transform);
                    sButton.transform.GetChild(0).GetComponent<Image>().sprite = psCon.PlayerSkills[i].Icon;
                    sButton.transform.GetChild(1).GetComponent<Text>().text = psCon.PlayerSkills[i].SkillName;
                    sButton.transform.GetChild(2).GetComponent<Text>().text = psCon.PlayerSkills[i].SkillMastery;
                    
                    break;

                case Skill.SkillType.Passive:
                    sButton = Instantiate(sButtonTemp, PassiveSkillList.transform);
                    sButton.transform.GetChild(0).GetComponent<Image>().sprite = psCon.PlayerSkills[i].Icon;
                    sButton.transform.GetChild(1).GetComponent<Text>().text = psCon.PlayerSkills[i].SkillName;
                    sButton.transform.GetChild(2).GetComponent<Text>().text = psCon.PlayerSkills[i].SkillMastery;

                    break;
            }
        }

        PassiveSkillList.SetActive(false);
    }


    
}
