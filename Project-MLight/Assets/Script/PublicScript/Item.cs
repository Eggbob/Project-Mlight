using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item : MonoBehaviour
{
    
    public ItemData Data { get; private set; }

    public Item(ItemData data) => Data = data;

    public ItemData _data;

    private void Start()
    {
        Data = _data;
    }
}
