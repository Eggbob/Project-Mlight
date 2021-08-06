using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//데이터 테이블
[System.Serializable]
public class DataTable<T> where T : struct
{
    public T[] Rows; //내용물을 저장할 배열

    public int Count { get { return Rows.Length;  } } //배열 길이 반환


    //배열이 존재하는지
    public static implicit operator bool(DataTable<T> table)
    {
        return table.Rows == null ? false : true;
    }

    public static implicit operator T[] (DataTable<T> table) //암시적 사용자 정의 형변환 연산자
    {
        return table.Rows;
    }

    //배열 암시적 형변환
    public static implicit operator DataTable<T>(T[] rows)
    {
        return new DataTable<T> { Rows = rows };
    }

    //구체적인 인덱스 설정
    public T this[int index]
    {
        get
        {
            return Rows[index];
        }
        set
        {
            Rows[index] = value;
        }
    }
}


