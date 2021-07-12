using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Buff : MonoBehaviour
{
   public enum BuffType
   {
        None,
        Atk,
        Move,
        Exp,
        Done
   }

    private BuffType type; //버프 타입
    private float duration; //지속시간
    private float currentTime; //현재 시간
    private Image buffImg; //버프 이미지
    private WaitForSeconds seconds = new WaitForSeconds(0.1f);

    private void Awake()
    {
        buffImg = GetComponent<Image>();
    }

    public void Init(BuffType bType, float dur, float curTime)
    {
        type = bType;
        duration = dur;
        currentTime = curTime;
        Excute();
    }

    private void Excute()
    {
        StartCoroutine(ActiveBuff());
    }

    IEnumerator ActiveBuff()
    {
        while(currentTime > 0)
        {
            currentTime -= 0.1f;
            buffImg.fillAmount = currentTime / duration;
            yield return seconds;
        }

        buffImg.fillAmount = 0;
        currentTime = 0;

        DeActiveBuff();
    }

    private void DeActiveBuff()
    {
        Destroy(this.gameObject);

    }
}
