using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcController : MonoBehaviour
{
    public Transform targetingTr;

    //접촉시에
    public abstract void Interact();

    //접촉종료시에
    public abstract void StopInteract();
}
