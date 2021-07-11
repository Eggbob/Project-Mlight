using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountableItemData : ItemData
{
    public int MaxAmount => _maxAmount;
    [SerializeField] private int _maxAmount = 99; //총 가질수 있는 아이템 갯수
}
