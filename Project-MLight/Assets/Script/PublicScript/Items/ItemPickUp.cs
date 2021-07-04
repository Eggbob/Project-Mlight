using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Object
{
    [SerializeField]
    private ItemData item;

    public void Drop()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator DropRoutine()
    {
        yield return (1f);
    }
}
