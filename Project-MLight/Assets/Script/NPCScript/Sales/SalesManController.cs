using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SalesManController : NpcController
{
    [SerializeField]
    private ItemData[] salesItems;

    [SerializeField]
    private SalesWindowManager sManager;

    //접촉시에
    public override void Interact()
    {
      if(!IsInteracting)
        {
            base.Interact();
        
            sManager.AddItem(salesItems);
            sManager.Open();
        }

    }

    //접촉종료시에
    public override void StopInteract()
    {
        if (IsInteracting)
        {
            base.StopInteract();

            sManager.Close();
        }
    }

}
