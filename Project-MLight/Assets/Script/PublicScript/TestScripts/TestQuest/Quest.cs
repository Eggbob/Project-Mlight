using UnityEngine;

[System.Serializable]
public class Quest
{ 
    [SerializeField]
    private string title; //퀘스트 타이틀

    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;

    public QuestScript MyQuestScript { get; set; }

    public string MyTitle => title;

    public string MyDescription { get => description; set => description = value; }
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int MyAmount { get => amount; set => amount = value; }
    public int MyCurrentAmount { get => currentAmount; set => currentAmount = value; }
    public string MyType { get => type;  }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if(MyType.ToLower() == item.Data.Name.ToLower())
        {
            MyCurrentAmount = PlayerController.instance.Inven.GetItemCount(item.Data.Name);
            QuestLog.MyInstance.UpdateSelected();
        }
    }
}
