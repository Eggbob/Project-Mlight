using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : Object
{
    [SerializeField]
    private int coinAmount;

    private void OnEnable()
    {
        isDrop = false;
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (!isDrop && timer > 5f)
        {
            StartCoroutine(ReturnToPull());
        }
    }

    //풀로 프리팹 반환
    private IEnumerator ReturnToPull()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(false);
        ItemObjectPool.ReturnCoin(this);      
    }

    //초기 설정
    public void Init(int _amount)
    {
        coinAmount = _amount;
    }

    public int Drop()
    {
        isDrop = true;
        StartCoroutine(ReturnToPull());
        return coinAmount;
    }

}
