using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpUIManager : MonoBehaviour
{
    public Slider ExpBar;
    public Text ExpTxt;

    private PlayerController pCon;

    private void Start()
    {
        pCon = GameManager.Instance.Player;
        pCon.AddExpAction(ExpUpdate);
        ExpUpdate();
    }

    private void ExpUpdate()
    {
        float value = (float)pCon.Exp / (float)pCon.MaxExp;
        int expValue = Mathf.RoundToInt(((float)pCon.Exp / (float)pCon.MaxExp) * 100);
        ExpBar.value = value;
        ExpTxt.text = expValue.ToString() + "%";
    }



}
