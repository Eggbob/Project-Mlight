using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Transform targetingTr;
    public Item item;



    private void Awake()
    {
       
    }

    private void Start()
    {
     //   Destroy(this.gameObject, 6f);
    }

    public Item Drop()
    {
        //this.gameObject.SetActive(false);

        return item;
    }

    //아이템 설정
    public void setData(Item _item)
    {
        item = _item;
    }

}
