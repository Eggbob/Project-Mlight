using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public Slider ExpBar;
    public Text ExpTxt;

    PlayerController pCon;

    void Start()
    {
        pCon = PlayerController.instance;
    }

   
    void Update()
    {
        ExpUpdate();
    }

    void ExpUpdate()
    {
        ExpBar.value = (float)pCon.Exp / (float)pCon.MaxExp;
        ExpTxt.text = pCon.Exp + "%"; 
    }

}
