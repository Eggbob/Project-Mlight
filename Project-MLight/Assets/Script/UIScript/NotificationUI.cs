using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour
{
    private static NotificationUI instance;

    public static NotificationUI Instance
    {
        get
        {
            if(instance.Equals(null))
            {
                instance = FindObjectOfType<NotificationUI>();
            }
            return instance;
        }     
    }

    private void Awake()
    {
        instance = this;
    }

    public void GenerateTxt(string message)
    {
        NotificationTxt nTxt = ObjectPool.GetNotifiTxt();
        Text messageTxt = nTxt.GetComponent<Text>();
        messageTxt.transform.SetParent(this.transform);
        messageTxt.transform.SetAsLastSibling();

        messageTxt.text = message;
    }
}
