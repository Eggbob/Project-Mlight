using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    public List<BuffSystem> onBuff = new List<BuffSystem>();

    public GameObject buffPrefab;

    public float BuffChange(string type, float origin)
    {
        if (onBuff.Count > 0)
        {
            float temp = 0;
            for (int i = 0; i < onBuff.Count; i++)
            {
                if (onBuff[i].type.Equals(type))
                {
                    temp += origin * onBuff[i].percentage;
                }
            }
            return origin + temp; ;
        }
        else
        {
            return origin;
        }
    }

    public void ChooseBuff(string type)
    {
        switch (type)
        {
            case "Atk":
                BuffChange(type, 1);
                break;
        }
    }

    public void CreateBuff(string type, float per, float du, Sprite icon)
    {
        GameObject go = Instantiate(buffPrefab, transform);
        go.GetComponent<BuffSystem>().Init(type, per, du);
        go.GetComponent<Image>().sprite = icon;
    }
}
