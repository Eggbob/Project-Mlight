using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInputUIManager : MonoBehaviour
{
    public Inventory _inventroy;

    public ItemData[] _itemDataArray;

    public Button _AddHelmet;
    public Button _AddSword;
    public Button _AddPotion;
    public Button _AddPotion50;
    public Button _AddFeather;
    public Button _AddFeather50;
    public Button _AddGold;

    private void Start()
    {
        if(_itemDataArray?.Length > 0)
        {
            for(int i=0; i< _itemDataArray.Length; i++)
            {
                _inventroy.Add(_itemDataArray[i], 2);

                if (_itemDataArray[i] is CountableItemData)
                    _inventroy.Add(_itemDataArray[i], 255);
            }
        }

        _AddHelmet.onClick.AddListener(() => _inventroy.Add(_itemDataArray[0]));
        _AddSword.onClick.AddListener(() => _inventroy.Add(_itemDataArray[4]));

        _AddPotion.onClick.AddListener(() => _inventroy.Add(_itemDataArray[2]));
        _AddPotion50.onClick.AddListener(() => _inventroy.Add(_itemDataArray[2], 50));

        _AddFeather.onClick.AddListener(() => _inventroy.Add(_itemDataArray[3]));
        _AddFeather50.onClick.AddListener(() => _inventroy.Add(_itemDataArray[3],50));

        _AddGold.onClick.AddListener(() => _inventroy.GetGold(1000));
    }
}
