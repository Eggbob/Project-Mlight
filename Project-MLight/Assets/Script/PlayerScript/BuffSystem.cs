using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffSystem : MonoBehaviour
{
    public string type;
    public float percentage; //얼마나 적용할지

    public float duration; //지속시간
    public float currentTime;//현재 시간
    public Image icon;

    WaitForSeconds seconds = new WaitForSeconds(0.1f);

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void Init(string _type, float _per, float _dur)
    {
        type = _type;
        percentage = _per;
        duration = _dur;
        currentTime = _dur;
        icon.fillAmount = 1;
        Excute();
    }

    public void Excute()
    {
        BuffManager.instance.onBuff.Add(this);
        BuffManager.instance.ChooseBuff(type);

        StartCoroutine(Activation());
    }

    IEnumerator Activation()
    {
        while(currentTime > 0)
        {
            currentTime -= 0.1f;
            icon.fillAmount = currentTime / duration;
            yield return seconds;
        }

        icon.fillAmount = 0;
        currentTime = 0;

        DeActivation();
    }

    public void DeActivation()
    {
        BuffManager.instance.onBuff.Remove(this);
        BuffManager.instance.ChooseBuff(type);

        Destroy(gameObject);
    }
}
