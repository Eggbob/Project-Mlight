using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{

    public Button pBtn;
    public Button iBtn;
    public Button dBtn;
    public Text ptxt;
    public Text Itxt;
    public Text Dtxt;
    public Text HPtxt;
    public Text MPtxt;
    public Text Stattxt;
 
    PlayerController pCon;

  

    void OnEnable()
    {
        pCon = PlayerController.instance;
        StatUpdate();
    }

    void StatUpdate()
    {
        pBtn.transform.GetChild(0).GetComponent<Text>().text = "힘 " + pCon.Power;
        iBtn.transform.GetChild(0).GetComponent<Text>().text = "지능 " + pCon.Int;
        dBtn.transform.GetChild(0).GetComponent<Text>().text = "방어도 " + pCon.DEF;
        ptxt.text = "근접 공격력 " + pCon.Power * 20;
        Itxt.text = "스킬 공격력 " + pCon.Int * 100;
        Dtxt.text = "물리 방어력 " + pCon.DEF * 5;
        HPtxt.text = "최대 HP " + pCon.MaxHp;
        MPtxt.text = "최대 MP " + pCon.MaxMp;
        Stattxt.text = "스탯 포인트 " + pCon.StatPoint;
    }
    
    void Update()
    {
        
    }
}
