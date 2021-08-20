using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpUIManager : MonoBehaviour
{
    private IEnumerator SaveRoutine()
    {
        GameManager.Instance.sManager.Save();

        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

   public void GameQuit()
   {
        StartCoroutine(SaveRoutine());
   }

    
}
