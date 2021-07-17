using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Portion_Item", menuName = "Inventory System/Item Data/Portion/HealthPortion", order = 3)]
public class HealthPortionItemData : PortionItemData
{

    public float Efficacy => efficacy;

    [SerializeField] private int efficacy;

}
