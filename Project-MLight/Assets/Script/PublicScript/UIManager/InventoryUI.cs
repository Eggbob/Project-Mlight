using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Options")]
    [Range(0, 10)]
    [SerializeField] private int horSlotCount = 8; // 가로 슬롯 개수
    [Range(0, 10)]
    [SerializeField] private int verSlotCount = 8; // 세로 슬롯 개수
    [SerializeField] private float slotMargin = 8f; //슬롯상하좌우 여백
    [SerializeField] private float contentPadding = 20f; //인벤토리 내부 여백
    [Range(32, 64)]
    [SerializeField] private float slotSize = 64f; //각 슬롯 크기

    [SerializeField] private RectTransform contentArea; //슬롯이 위치할 영역
    [SerializeField] private GameObject slotPrefab; //슬롯 프리팹
    List<ItemSlotUI> slotUiList;

    private void slotInit() //슬롯 동적 생성
    {
        //슬롯 프리팹 설정
        slotPrefab.TryGetComponent(out RectTransform slotRect);
        slotRect.sizeDelta = new Vector2(slotSize, slotSize);

        slotPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
            slotPrefab.AddComponent<ItemSlotUI>();

        slotPrefab.SetActive(false);

        Vector2 beginPos = new Vector2(contentPadding, -contentPadding);
        Vector2 curPos = beginPos;

        slotUiList = new List<ItemSlotUI>(verSlotCount * horSlotCount);

        for(int i = 0; i< verSlotCount; i++)
        {
            for(int j = 0; j< horSlotCount; j++)
            {
                int slotIndex = (horSlotCount * i) + j;

                var slotRT = CloneSlot();
                slotRT.pivot = new Vector2(0f, 1f);
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

                var slotUI = slotRT.GetComponent<ItemSlotUI>();
              
                //slotUI.SetSlotIndex(slotIndex);
                slotUiList.Add(slotUI);


                curPos.x += (slotMargin + slotSize);
            }

            curPos.x = beginPos.x;
            curPos.y -= (slotMargin + slotSize);
        }

        // 프리팹이 아닌경우 파괴
        if(slotPrefab.scene.rootCount != 0)
        {
            Destroy(slotPrefab);
        }

        RectTransform CloneSlot()
        {
            GameObject slotGo = Instantiate(slotPrefab);
            RectTransform rt = slotGo.GetComponent<RectTransform>();
            rt.SetParent(contentArea);
            return rt;
        }
    }



}
