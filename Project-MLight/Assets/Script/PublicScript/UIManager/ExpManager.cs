using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public Slider ExpBar;
    public Text ExpTxt;


    
    private PlayerController pCon;

    void Start()
    {
        pCon = PlayerController.instance;
        pCon.ExpGet += ExpUpdate;

        ExpUpdate();
    }

   
    void Update()
    {
      
    }

    void ExpUpdate()
    {
        float value = (float)pCon.Exp / (float)pCon.MaxExp;
        int expValue = Mathf.RoundToInt(((float)pCon.Exp / (float)pCon.MaxExp) * 100);
        ExpBar.value = value;
        ExpTxt.text = expValue.ToString() + "%";

    }



}
