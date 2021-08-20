using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public Text lvlTxt;

    [Header("스탯 텍스트")]
    public Text pStat;
    public Text iStat;
    public Text dStat;

    [Header("보너스 스탯 텍스트")]
    public Text bPowerTxt;
    public Text bIntTxt;
    public Text bDefTxt;

    [Header("상세정보 텍스트")]
    public Text Ptxt;
    public Text Itxt;
    public Text Dtxt;
    public Text HPtxt;
    public Text MPtxt;
    public Text Stattxt;

    [Header("버튼")]
    public Button[] UpBtn;
    public Button[] DownBtn;
    public Button conBtn;

    PlayerController pCon;
    int statPoint; //스탯포인트
    int pAddPoint; //투자한 포인트
    int iAddPoint; //투자한 포인트
    int dAddPoint; //투자한 포인트

    private void Awake()
    {
        pAddPoint = 0;
        pCon = GameManager.Instance.Player;
    }

    void OnEnable()
    {     
        statInit();
    }

    void statInit()
    {

        lvlTxt.text = pCon.Level.ToString();
        statPoint = pCon.StatPoint;   
        pStat.text = "힘 " + pCon.Power;
        iStat.text = "지능 " + pCon.Int;
        dStat.text = "방어도 " + pCon.DEF;
        Ptxt.text = "근접 공격력 " + pCon.Power * 20;
        Itxt.text = "스킬 공격력 " + pCon.Int * 100;
        Dtxt.text = "물리 방어력 " + pCon.DEF * 5;
        HPtxt.text = "최대 HP " + pCon.MaxHp;
        MPtxt.text = "최대 MP " + pCon.MaxMp;
        Stattxt.text = "스탯 포인트 " + statPoint;

        if (pCon.BonusPower > 0) { bPowerTxt.text = "+" + pCon.BonusPower; bPowerTxt.gameObject.SetActive(true); }
        else bPowerTxt.gameObject.SetActive(false);

        if (pCon.BonusInt > 0) { bIntTxt.text = "+" + pCon.BonusInt; bIntTxt.gameObject.SetActive(true); }
        else bIntTxt.gameObject.SetActive(false);

        if (pCon.BonusDef > 0) { bDefTxt.text = "+" + pCon.BonusDef; bDefTxt.gameObject.SetActive(true); }
        else bDefTxt.gameObject.SetActive(false);

        UpBtn[0].onClick.AddListener(() => pUpBtn());
        DownBtn[0].onClick.AddListener(() => pDownBtn());
        UpBtn[1].onClick.AddListener(() => iUpBtn());
        DownBtn[1].onClick.AddListener(() => iDownBtn());
        UpBtn[2].onClick.AddListener(() => dUpBtn());
        DownBtn[2].onClick.AddListener(() => dDownBtn());
        conBtn.onClick.AddListener(() => conBtnUpdate());
    }
    
    void Update()
    {
        if(pCon.StatPoint >0)
        {
            activeBtn();
        }
        else
        {
            inactiveBtn();
        }
    }

    void activeBtn() //버튼활성화
    {
        for(int i= 0; i< 3; i++)
        {
            UpBtn[i].gameObject.SetActive(true);
            DownBtn[i].gameObject.SetActive(true);
        }
    }
    void inactiveBtn() //버튼 비활성화
    {
        for (int i = 0; i < 3; i++)
        {
            UpBtn[i].gameObject.SetActive(false);
            DownBtn[i].gameObject.SetActive(false);
        }
    }

    void pUpBtn() //파워업버튼
    {
       if(statPoint > 0)
        {
            statPoint--;
            pAddPoint++;

            StringBuilder statTxt = new StringBuilder();
            statTxt.Append("힘");
            statTxt.Append(pCon.Power);
            statTxt.Append("( +");
            statTxt.Append(pAddPoint);
            statTxt.Append(")");
           

            pStat.text = statTxt.ToString();
            Stattxt.text = "스탯 포인트 " + statPoint;
        }
    }

    void pDownBtn()//파워다운 버튼
    {
        if (pAddPoint > 0)
        {
            statPoint++;
            pAddPoint--;

            StringBuilder statTxt = new StringBuilder();
            statTxt.Append("힘");
            statTxt.Append(pCon.Power);
            statTxt.Append("( +");
            statTxt.Append(pAddPoint);
            statTxt.Append(")");


            pStat.text = statTxt.ToString();
            Stattxt.text = "스탯 포인트 " + statPoint;
        }
    }

    void iUpBtn()
    {
        if (statPoint > 0)
        {
            statPoint--;
            iAddPoint++;
            StringBuilder statTxt = new StringBuilder();
            statTxt.Append("지능");
            statTxt.Append(pCon.Int);
            statTxt.Append("( +");
            statTxt.Append(iAddPoint);
            statTxt.Append(")");

            iStat.text = statTxt.ToString();
            Stattxt.text = "스탯 포인트 " + statPoint;
        }
    } //인트업버튼

    void iDownBtn()
    {
        if (iAddPoint > 0)
        {
            statPoint++;
            iAddPoint--;

            StringBuilder statTxt = new StringBuilder();
            statTxt.Append("지능");
            statTxt.Append(pCon.Int);
            statTxt.Append("( +");
            statTxt.Append(iAddPoint);
            statTxt.Append(")");

            iStat.text = statTxt.ToString();
            Stattxt.text = "스탯 포인트 " + statPoint;
        }
    }//인트다운 버튼

    void dUpBtn()
    {
        if (statPoint > 0)
        {
            statPoint--;
            dAddPoint++;

            StringBuilder statTxt = new StringBuilder();
            statTxt.Append("방어도");
            statTxt.Append(pCon.DEF);
            statTxt.Append("( +");
            statTxt.Append(dAddPoint);
            statTxt.Append(")");

            dStat.text = statTxt.ToString();
            Stattxt.text = "스탯 포인트 " + statPoint;
        }
    }//데프업버튼

    void dDownBtn()
    {
        if (dAddPoint > 0)
        {
            statPoint++;
            dAddPoint--;

            StringBuilder statTxt = new StringBuilder();
            statTxt.Append("방어도");
            statTxt.Append(pCon.DEF);
            statTxt.Append("( +");
            statTxt.Append(dAddPoint);
            statTxt.Append(")");

            dStat.text = statTxt.ToString();
            Stattxt.text = "스탯 포인트 " + statPoint;
        }
    }//데프다운버튼

    void conBtnUpdate()
    {
        pCon.Power += pAddPoint;
        pCon.Int += iAddPoint;
        pCon.DEF += dAddPoint;
        pCon.StatPoint = statPoint;
        statInit();
    } //확인 버튼

}
