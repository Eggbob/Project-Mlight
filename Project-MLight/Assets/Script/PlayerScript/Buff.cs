using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Buff : MonoBehaviour
{
    private BuffManager.BuffType buffType; //버프 타입 변수
    private float duration; //지속시간
    private float currentTime; //현재 시간
    private float value; //버프 적용값
    private int index; //버프 인덱스
    private Image buffImg; //버프 이미지
    
    private WaitForSeconds seconds = new WaitForSeconds(0.1f);

    public BuffManager.BuffType BuffType => buffType;
    public float Value => value;
    public int Index => index;

    private void Awake()
    {
        buffImg = GetComponent<Image>();
    }

    //초기화 함수
    public void Init(BuffManager.BuffType bType, float _duration, float _value, int _index, Sprite icon)
    {
        buffType = bType;
        duration = _duration;
        currentTime = _duration;
        value = _value;
        index = _index;
        buffImg.sprite = icon;

        this.gameObject.SetActive(true);

        Excute();
    }

    //지속시간 증가
    public void AddDuration(float addTime)
    {
        currentTime += addTime;
        duration += addTime;
    }

    //버프 실행
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
