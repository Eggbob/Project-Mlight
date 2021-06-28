using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Transform targetingTr;


    public void Drop()
    {
        this.gameObject.SetActive(false);
       
    }

    IEnumerator DropRoutine()
    {
        yield return (1f);
      
    }
}
