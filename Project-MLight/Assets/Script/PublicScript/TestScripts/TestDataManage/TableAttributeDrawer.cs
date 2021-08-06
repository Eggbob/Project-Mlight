using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TableAttribute))]
public class TableAttributeDrawer : PropertyDrawer
{
    private SerializedProperty _rows;
    private TableAttribute _attr;


    //배열 갯수 만큼 열 더해주기
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * (property.FindPropertyRelative("Rows").arraySize + 2);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        _attr = attribute as TableAttribute;
        //attribute 가져오기

        var fieldsInfo = _attr.RowType.GetFields();
        //구조체의 모든 필드 정보 가져오기

        _rows = property.FindPropertyRelative("Rows");
        //해당 SerializedProperty 내부의 배열가져오기

        int drawLines = _rows.arraySize + 2; //그려줄 라인 수
        float labelWidth = 50; // row를 그려줄 너비
        float singleHeight = position.height / drawLines; //한라인에 그려줄 픽셀수

        float contentWidth = (position.width - labelWidth) / fieldsInfo.Length; //Colums 폭


        EditorGUI.BeginChangeCheck();

        EditorGUI.LabelField(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, singleHeight), label);

        _rows.arraySize = EditorGUI.DelayedIntField(
                new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, singleHeight),
                new GUIContent("Size"),
                _rows.arraySize);

        // 테이블 헤더 그려주기
        for (int i = 0; i < fieldsInfo.Length; i++)
        {
            Rect headerRect = new Rect(position.x + labelWidth + (contentWidth * i), position.y + singleHeight, contentWidth, singleHeight);
            EditorGUI.LabelField(headerRect, fieldsInfo[i].Name);
        }

        // 테이블 내용 그려주기
        for (int i = 0; i < _rows.arraySize; i++)
        {
            Rect contentRect = new Rect(position.x, position.y + singleHeight * (i + 2), labelWidth, singleHeight);

            EditorGUI.LabelField(contentRect, string.Format("Row {0}", i));
            contentRect.width = contentWidth;

            var contentProperty = _rows.GetArrayElementAtIndex(i);

            for (int j = 0; j < fieldsInfo.Length; j++)
            {
                contentRect.position = new Vector2(position.x + labelWidth + contentWidth * j, contentRect.position.y);
                EditorGUI.PropertyField(contentRect, contentProperty.FindPropertyRelative(fieldsInfo[j].Name), GUIContent.none);
            }
        }

        EditorGUI.EndChangeCheck();

    }



}
