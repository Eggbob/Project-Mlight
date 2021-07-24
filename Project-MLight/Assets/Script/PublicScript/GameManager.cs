using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject inventory;

    private void Start()
    {
        StartCoroutine(StartRoutine());
    }


    IEnumerator StartRoutine()
    {
        inventory.SetActive(true);

        yield return null;

        inventory.SetActive(false);
    }
}
