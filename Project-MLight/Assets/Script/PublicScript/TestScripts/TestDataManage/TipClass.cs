using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MysCustomTable : DataTable<int> { }

public class TipClass : MonoBehaviour
{
    public int[] IntArray;
    public MysCustomTable IntTable;
}
