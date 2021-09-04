using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcController : MonoBehaviour
{
    public Transform targetingTr; //npc 타겟팅

    public bool IsInteracting { get; set; } //접촉했는지

    //접촉시에
    public virtual void Interact() 
    {
        IsInteracting = true;
        BgmManager.Instance.PlayEffectSound("WindowOpen");
    }

    //접촉종료시에
    public virtual void StopInteract() 
    {
        IsInteracting = false;
        BgmManager.Instance.PlayEffectSound("WindowClose");
    }
}
