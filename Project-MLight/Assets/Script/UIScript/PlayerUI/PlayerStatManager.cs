using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour
{
    public Slider HpBar;
    public Text HpTxt;

    public Slider MpBar;
    public Text MpTxt;

    public Text LvTxt;
    PlayerController pCon;

    private void Start()
    {
        pCon = GameManager.Instance.Player;
}

    private void Update()
    {
        StatusBarUpdate();
        LvTxtUpdate();
    }

    void StatusBarUpdate()
    {

        HpBar.value = (float)pCon.Hp / (float)pCon.MaxHp; ; //Hp바 수정
        HpTxt.text = string.Format("{0}/{1}", pCon.Hp, pCon.MaxHp); //HP 텍스트 수정

        MpBar.value = (float)pCon.Mp / (float)pCon.MaxMp; ; // Mp바 값 수정
        MpTxt.text = string.Format("{0}/{1}", pCon.Mp, pCon.MaxMp); // MP 텍스트 수정
    }

    void LvTxtUpdate()
    {
        if (pCon.Level < 10)
        {
            LvTxt.text = "0" + pCon.Level;
        }
        else
        {
            LvTxt.text = pCon.Level.ToString();
        }
    }
}
