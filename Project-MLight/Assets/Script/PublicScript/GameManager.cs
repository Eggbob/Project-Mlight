using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void KillConfirmed(LivingEntity LCon);

public class GameManager : MonoBehaviour
{
   
    private static GameManager instance;

    public GameObject inventory;
    public GameObject shopUI;
    public GameObject questUI;

    public static GameManager Instance 
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    
    }

    private void Start()
    {
        StartCoroutine(StartRoutine());
    }


    IEnumerator StartRoutine()
    {
        inventory.SetActive(true);
        shopUI.SetActive(true);
        questUI.SetActive(true);
        yield return null;

        inventory.SetActive(false);
        shopUI.SetActive(false);
        questUI.SetActive(false);
    }

}
