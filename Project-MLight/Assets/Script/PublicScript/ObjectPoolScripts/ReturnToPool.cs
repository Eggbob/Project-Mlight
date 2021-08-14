using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Return());
    }


    private IEnumerator Return()
    {
        yield return new WaitForSeconds(2f);
        ObjectPool.ReturnLvEffect(this.gameObject);
    }
}
