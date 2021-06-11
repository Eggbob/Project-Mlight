using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{

    public Button pBtn;
    public Button iBtn;
    public Button aBtn;
    public Button dBtn;
    public 

    PlayerController pCon;

    void Start()
    {
        pCon = PlayerController.instance;
        pBtn.transform.GetChild(0).GetComponent<Text>().text = "힘 " + pCon.Power;
        iBtn.transform.GetChild(0).GetComponent<Text>().text = "지능 " + pCon.Int;
        aBtn.transform.GetChild(0).GetComponent<Text>().text = "민첩 " + pCon.AG;
        dBtn.transform.GetChild(0).GetComponent<Text>().text = "방어도 " + pCon.DEF;

    }

    
    void Update()
    {
        
    }
}
