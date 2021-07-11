using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Portion_Item", menuName = "Inventory System/Item Data/Portion/ExpPortion", order = 3)]
public class ExpPortionItemData : PortionItemData
{

    public float ApperTime => appearTime;

    [SerializeField] private float appearTime;   // 포션 효과시간


    public override Item CreateItem()
    {
        return new ExpPortionItem(this);
    }
}
