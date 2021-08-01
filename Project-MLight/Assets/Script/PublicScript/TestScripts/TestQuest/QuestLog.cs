using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab; //퀘스트 프리팹

    [SerializeField]
    private Transform questParent; //퀘스트가 생성될 지역

    private Quest selected;

    [SerializeField]
    private Text questDescription;


    private static QuestLog instance;

    public static QuestLog MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }
            return instance;
        }
    }

    public void AcceptQuest(Quest quest) //퀘스트 수락
    {
        foreach(CollectObjective obj in quest.MyCollectObjectives)
        {
          //  InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(obj.UpdateItemCount);
        }

        GameObject go = Instantiate(questPrefab, questParent);

        QuestScript qs = go.GetComponent<QuestScript>();
        quest.MyQuestScript = qs;
        qs.MyQuest = quest;


        go.GetComponentInChildren<Text>().text = quest.MyTitle; 
    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest) //퀘스트 보여주기
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.DeSelect();
            }

            string objectives = string.Empty;

            selected = quest;

            string title = quest.MyTitle;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            //퀘스트 설명 
            questDescription.text = string.Format("{0}\n{1}\nObjectives\n{2}", title, quest.MyDescription, objectives);
        }
    }
}
