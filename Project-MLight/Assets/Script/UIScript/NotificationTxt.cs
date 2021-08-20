using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationTxt : MonoBehaviour
{
    private void GetBackRoutine()
    {
        ObjectPool.ReturnNotifiTxt(this);
    }
}
