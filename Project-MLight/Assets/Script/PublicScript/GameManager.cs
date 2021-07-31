using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject inventory;
    public GameObject shopUI;

    private void Start()
    {
        StartCoroutine(StartRoutine());
    }


    IEnumerator StartRoutine()
    {
        inventory.SetActive(true);
        shopUI.SetActive(true);
        yield return null;

        inventory.SetActive(false);
        shopUI.SetActive(false);
    }
}
