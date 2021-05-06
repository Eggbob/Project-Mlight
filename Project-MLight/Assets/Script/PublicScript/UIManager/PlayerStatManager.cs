using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour
{
    public RectTransform HpBar;
    public Text HpTxt;
    public RectTransform MpBar;
    public Text MpTxt;
    PlayerController pCon;
    Vector2 HBarSize;
    Vector2 MBarSize;

    private void Start()
    {
        pCon = PlayerController.instance;
       
       
    }

    private void Update()
    {
        StatusUpdate();
    }

    void StatusUpdate()
    {
        HBarSize = HpBar.sizeDelta;
        float Rat = pCon.Hp / pCon.MaxHp;
        RectTransform rect = HpBar;
        Vector2 vSize = rect.sizeDelta;
        vSize.x = HBarSize.x * Rat;
        HpBar.sizeDelta = vSize;
        HpTxt.text = string.Format("{0}/{1}", pCon.Hp, pCon.MaxHp);

        MBarSize = MpBar.sizeDelta;
        float fRat = pCon.Mp / pCon.MaxMp;
        Vector2 mSize = MpBar.sizeDelta;
        mSize.x = MBarSize.x * fRat;
        MpBar.sizeDelta = mSize;
        MpTxt.text = string.Format("{0}/{1}", pCon.Mp, pCon.MaxMp);
    }

}
