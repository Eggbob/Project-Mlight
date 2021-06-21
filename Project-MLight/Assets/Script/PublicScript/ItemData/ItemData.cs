﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemData : ScriptableObject
{
    public int ID => _id;
    public string Name => _name;
    public string Tooltip => _tooltip;
    public Sprite IconSprite => _iconSprite;

    [SerializeField] private int _id; //아이템 아이디
    [SerializeField] private string _name; //아이템 이름
    [Multiline]
    [SerializeField] private string _tooltip; //아이템 설명
    [SerializeField] private Sprite _iconSprite; //아이콘 스프라이트
    [SerializeField] private GameObject _dropItemPrefab; //아이템 게임오브젝트

    public abstract Item CreateItem();
}
