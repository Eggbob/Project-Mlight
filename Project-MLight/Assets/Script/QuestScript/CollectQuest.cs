using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수집 퀘스트
/// </summary>
[CreateAssetMenu(fileName = "Quest", menuName = "QuestSystem/CollectQuest")]
public class CollectQuest : Quest
{
    public ColletObject[] ColletObjects => _collectObjects;

    [SerializeField]
    private ColletObject[] _collectObjects;

    public override bool IsComplete() //퀘스트를 완료했는지
    {
        foreach (ColletObject coll in _collectObjects)
        {
            if(!coll.IsComplete)
                return false;
          
            else
                return true;             
       
        }
        return false;
    }
}

[System.Serializable]
public class ColletObject 
{
    [SerializeField]
    private ItemData _collectItem; //수집할 아이템

    [SerializeField]
    private int _totalAmount; //총 수집할 아이템 개수

    [SerializeField]
    private int _currentAmount; //현재 수집한 아이템 개수

    private int _itemIndex; //인벤토리에서의 아이템 인덱스

    public ItemData CollectItem => _collectItem;

    public int TotalAmount => _totalAmount;

    public int CurAmount => _currentAmount;

    public bool IsComplete //아이템을 전부 수집했는지
    {
        get
        {         
            return _currentAmount >= _totalAmount;
        }
    }

    public void UpdateItemAmount(ItemData data) //아이템 개수 업데이트
    {
        if(data.ID.Equals(_collectItem.ID))
        {
            (_currentAmount, _itemIndex) = GameManager.Instance.Inven.GetItemCount(_collectItem.ID);
            //이 아래쪽에 UI업데이트 호출

            if(_currentAmount < _totalAmount)
            {
                NotificationUI.Instance.GenerateTxt(_collectItem.ItemName + " : " + _currentAmount + "/" + _totalAmount);

                if (_currentAmount == _totalAmount)
                {
                    BgmManager.Instance.PlayEffectSound("Clear");
                    NotificationUI.Instance.GenerateTxt(_collectItem.ItemName + " (수집 완료)");  
                } 
            }
        
            QuestManager.Instance.CheckComplete(); 
        }   
    }

    public void CompleteQuest() //수집한 아이템 지우기
    {
        GameManager.Instance.Inven.Remove(_itemIndex, _totalAmount);
    }
}
