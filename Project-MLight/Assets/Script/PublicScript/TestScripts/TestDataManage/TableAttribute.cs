using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//테이블 스타일 표시
public class TableAttribute : PropertyAttribute
{

    //열 타입
    public Type RowType;

    //열 이름
    public string[] RowNames = null;

    //새 인스턴스 초기화
    public TableAttribute(Type rowType)
    {
        RowType = rowType;
    }
}
