using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup canvasGroup; //캔버스그룹

    private NpcController nCon;//npc 컨트롤러

    public virtual void Open(NpcController npc)
    {
        this.nCon = npc;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void Close()
    {
        nCon.IsInteracting = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        nCon = null;
    }
}
