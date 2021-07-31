using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesMan : NpcController
{
    [SerializeField]
    private SalesManItem[] items;

    [SerializeField]
    private ShopManager sManager;

    public bool IsOpen { get; set; }

    //접촉시에
    public override void Interact()
    {
        if(!IsOpen)
        {
            IsOpen = true;
            sManager.CreatePages(items);
            sManager.Open(this);
        }
        
    }

    //접촉종료시에
    public override void StopInteract()
    {
        if(IsOpen)
        {
            IsOpen = false;
        
            sManager.Close();
        }
    }
}
