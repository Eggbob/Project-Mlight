using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private static UIManager instance;

    public GameObject[] InitUIs;

    public GameObject RespawnUI;

    public static UIManager Instance
    {
        get
        {
            if(instance.Equals(null))
            {
                instance = GameObject.FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;

        StartCoroutine(StartRoutine());
    }

    IEnumerator StartRoutine()
    {
        foreach(GameObject obj in InitUIs)
        {
            obj.SetActive(true);
        }

        yield return null;

        foreach (GameObject obj in InitUIs)
        {
            obj.SetActive(false);
        }
    }
}
