using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private UIManager instance;

    public GameObject[] InitUIs;

    public UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<UIManager>();
            }
            return Instance;
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
