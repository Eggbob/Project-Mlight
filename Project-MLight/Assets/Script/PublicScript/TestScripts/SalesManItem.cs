using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SalesManItem 
{
    [SerializeField]
    private ItemData item;

    [SerializeField]
    private int quantity; //수량

    [SerializeField]
    private bool unlimited;

    public ItemData Item { get => item; }
    public int Quantity { get => quantity; set => quantity = value; }
    public bool Unlimited { get => unlimited; }
}
