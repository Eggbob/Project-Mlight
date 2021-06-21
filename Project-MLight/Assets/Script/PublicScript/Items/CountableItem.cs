using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountableItem : Item
{
    public CountableItemData CountableData { get; private set; }

    public int Amount { get; protected set; } //현재 아이템 개수

    //아이템 최대 개수
    public int MaxAmount => CountableData.MaxAmount;

    //아이템이 최대 개수인지 확인
    public bool IsMax => Amount >= CountableData.MaxAmount;

    // 아이템이 비었는지 
    public bool IsEmpty => Amount <= 0;

    public CountableItem(CountableItemData data, int amount = 1) : base(data)
    {
        CountableData = data;
        SetAmount(amount);
    }

    //아이템 개수 지정
    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }

    // 아이템 개수 추가및 최대량 초과시 반환
    public int AddAmountAndGetExcess(int amount)
    {
        int nextAmount = Amount + amount;
        SetAmount(nextAmount);

        return (nextAmount > MaxAmount) ? (nextAmount - MaxAmount) : 0;
    }

    // 아이템 개수 나누고 복제
    public CountableItem SeperateAndClone(int amount)
    {
        //수량이 한개 이하이면 리턴
        if (Amount <= 1) return null;

        if (amount > Amount - 1)
            amount = Amount - 1;

        Amount -= amount;
        return Clone(amount);
    }

    protected abstract CountableItem Clone(int amount);
}
