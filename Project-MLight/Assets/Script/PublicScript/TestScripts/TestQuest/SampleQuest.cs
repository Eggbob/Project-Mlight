using UnityEngine;

[System.Serializable]
public class SampleQuest : MonoBehaviour
{ 
    [SerializeField]
    private string title; //퀘스트 타이틀

    [SerializeField]
    private string description; //퀘스트 설명

    [SerializeField]
    private int level;

    [SerializeField]
    private int xp;

    [SerializeField]
    private CollectObjective[] collectObjectives; //수집할 오브젝트


    [SerializeField]
    private KillObjective[] killObjectives; //처치할 오브젝트

    public QuestScript MyQuestScript { get; set; }

    public QuestGiver MyQuestGiver { get; set; }


    public string MyTitle => title;

    public string MyDescription { get => description; set => description = value; }
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }

    public bool IsComplete
    {
        get 
        {
            foreach(Objective obj in collectObjectives)
            {
                if(!obj.IsComplete)
                {
                    return false;
                }
            }
            foreach(Objective obj in MyKillObjectives)
            {
                if(!obj.IsComplete)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public KillObjective[] MyKillObjectives { get => killObjectives;  }
    public int MyLevel { get => level; set => level = value; }

    public int MyXp { get => xp; set => xp = value; }
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

    public bool IsComplete
    {
        get 
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item) //아이템 수집 갯수 업데이트
    {
        if(MyType.ToLower() == item.Data.Name.ToLower())
        {
            //MyCurrentAmount = PlayerController.instance.Inven.GetItemCount(item.Data.Name);

            if(MyCurrentAmount <= MyAmount)
            {
                FeedManager.Instance.WriteMessage(string.Format("{0}: {1}/{2}",
                    item.Data.Name, MyCurrentAmount, MyAmount));
            }

            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
        }
    }

    public void UpdateItemCount()
    {
       // MyCurrentAmount = PlayerController.instance.Inven.GetItemCount(MyType);

        QuestLog.MyInstance.UpdateSelected();
        QuestLog.MyInstance.CheckCompletion();
    }

    //퀘스트 완료시 아이템 지우기
    public void Complete()
    {
        //Stack<Item> items = Invetory.Instance.GetItems(MyType, MyAmount);

        //foreach(Item item in items)
        //{
        //  items.Remove();
        //}
    }
}


[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(LivingEntity LCon)
    {
        //if (MyType == LCon.myType)
        //{
        //    if(MyCurrentAmount < MyAmount)
        //{
        //    MyCurrentAmount++;

        //    //    FeedManager.Instance.WriteMessage(string.Format("{0}: {1}/{2}",
        //    //          LCon.name, MyCurrentAmount, MyAmount));

        //    //    QuestLog.MyInstance.UpdateSelected();
        //    //    QuestLog.MyInstance.CheckCompletion();
        //}
        //   
        //}
    }
}