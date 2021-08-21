using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private Dictionary<int, ItemData> itemDic = new Dictionary<int, ItemData>();

    private PlayerController pCon;
    private Inventory inven;
    private SkillBookManager skillBookManager;
    private QuestManager qManager;

    private PlayerData pData;
    private QuickBtnData qData;
    private QuestData qeData;

    private string filePath;

    private void Start()
    {
        pCon = GameManager.Instance.Player;
        inven = GameManager.Instance.Inven;
        skillBookManager = GameManager.Instance.SbookManager;
        qManager = QuestManager.Instance;

        LoadAllItemData();
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }


    //플레이어 저장
    private void SavePlayer()
    {          
        pData = new PlayerData(pCon.Level,
        pCon.Exp, pCon.MaxHp, pCon.MaxMp, pCon.Power,
        pCon.DEF, pCon.Int, pCon.StatPoint, pCon.gameObject.transform.position);

        pData.saveGold = inven.Gold;

        for (int i = 0; i < inven.InvenItems.Length; i++)
        {
            if (inven.InvenItems[i] is CountableItem cItem && inven.InvenItems[i] != null)//수량이 있는 아이템일시
            {
                int val;

                if (pData.saveItems.TryGetValue(cItem.Data.ID, out val)) //키값이 존재한다면
                    pData.saveItems[cItem.Data.ID] = val + cItem.Amount;

                else
                    pData.saveItems.Add(cItem.Data.ID, cItem.Amount);
            }
            else if (inven.InvenItems[i] != null)
            {
                int val;

                if (pData.saveItems.TryGetValue(inven.InvenItems[i].Data.ID, out val)) //키값이 존재한다면
                    pData.saveItems[inven.InvenItems[i].Data.ID] = val + 1;

                else
                    pData.saveItems.Add(inven.InvenItems[i].Data.ID, 1);

            }
        }

        if(inven.EquipManager.hasWeapon)
            pData.equipItems.Add(inven.EquipManager.WITEM.Data.ID);

        if (inven.EquipManager.hasArmor)
            pData.equipItems.Add(inven.EquipManager.AITEM.Data.ID);
       
        for(int i = 1; i< pCon.psCon.PlayerSkills.Count; i++)
        {
            float skillexp = ((pCon.psCon.PlayerSkills[i].SkillLevel * 200) -200) + pCon.psCon.PlayerSkills[i].SkillExp; 
            pData.saveSkills.Add(pCon.psCon.PlayerSkills[i].SkillId, skillexp );         
        }

        pData.DictionaryToJson();

   
        filePath = Application.persistentDataPath + "/" + "PlayerData.json";

        string jsonData = JsonUtility.ToJson(pData, true);

        File.WriteAllText(filePath, jsonData);
     
    }

    //퀵슬롯 세이브
    private void SaveQuick()
    {
        qData = new QuickBtnData();

        foreach(ItemQuickSlotUI qSlot in inven.InvenUI.QuickSlotUIs)
        {
            if(qSlot.HasItem)
             qData.itemQuickSlot.Add(qSlot.Index, qSlot.SlotItem.ID);
        }

    
        foreach(SkillQuickSlotUI sSlot in skillBookManager.qSlotUIList)
        {
            if (sSlot.HasSkill)
                qData.skillQuickSlot.Add(sSlot.Index, sSlot.slotSkill.SkillId);
        }

        qData.DictionaryToJson();

        filePath = Application.persistentDataPath + "/" + "QuickSlotData.json";
        string jsonData = JsonConvert.SerializeObject(qData);
        File.WriteAllText(filePath, jsonData);
       
    }

    //퀘스트 저장
    private void SaveQuest()
    { 
        filePath = Application.persistentDataPath + "/" + "QuestData.txt";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        qeData = new QuestData();
    
        foreach (Quest quest in qManager.Quests)
        {
            qeData.playerQuests.Add(quest);
        }

        var jsonData = JsonUtility.ToJson(qeData, true);
      
        bf.Serialize(file, jsonData);
        file.Close();
 
    }

    //저장한 데이터 로드하기
    private void Load()
    {      
        LoadPlayer();
        LoadQuickSlot();
        LoadQuest();
    }


    //플레이어 정보 불러오기
    private void LoadPlayer()
    {

        filePath = Application.persistentDataPath + "/" + "PlayerData.json";
        if (!File.Exists(filePath))
        {
            SavePlayer();
        }

        string jsonData = File.ReadAllText(filePath);


        pData = JsonUtility.FromJson<PlayerData>(jsonData); 
       

        pCon.statusInit(pData.saveLevel,
        pData.saveExp, pData.saveHp, pData.saveMp,
        pData.savePower, pData.saveInt, pData.saveDef,
        pData.saveStatPoint);


        pCon.gameObject.transform.position = pData.savePos;
        inven.GetGold(pData.saveGold);

        pData.JsonToDictionary();

        foreach (KeyValuePair<int, int> items in pData.saveItems) //key값 대입후 아이템 넣기
        {
            ItemData iData;
            itemDic.TryGetValue(items.Key, out iData);

            if (iData != null)
            {
                inven.Add(iData, items.Value);
            }
        }

        foreach(int id in pData.equipItems)
        {
            ItemData iData;
            itemDic.TryGetValue(id, out iData);

            if(iData != null)
            {
                int index = inven.Add(iData, 1);
                inven.Equip(index);
            }
        }

        for(int i = 1; i< pData.saveSkills.Count; i++)
        {
            Skill skill = pCon.psCon.PlayerSkills[i];
            float exp;

            if(pData.saveSkills.TryGetValue(skill.SkillId, out exp))
            {
               skill.GetExp(exp/100);
            }

        }
   
    }

    //퀵슬롯 불러오기
    private void LoadQuickSlot()
    {
        filePath = Application.persistentDataPath + "/" + "QuickSlotData.json";
        
        if (!File.Exists(filePath))
        {
            SaveQuick();
        }

        string jsonData = File.ReadAllText(filePath);

        qData = JsonUtility.FromJson<QuickBtnData>(jsonData);

        qData.JsonToDictionary();

        foreach (KeyValuePair<int, int> slots in qData.itemQuickSlot) //key값 대입후 아이템 넣기
        {
            int slotIndex, itemID;

            qData.itemQuickSlot.TryGetValue(slots.Key, out itemID);

            (itemID, slotIndex) = inven.GetItemCount(itemID);

            inven.InvenUI.SetQuickSlot(slots.Key, slotIndex);
        }

        foreach (KeyValuePair<int, int> slots in qData.skillQuickSlot) //key값 대입후 아이템 넣기
        {
            skillBookManager.SetSkill(slots.Key, slots.Value);         
        }
    }

    //퀘스트 정보 불러오기
    private void LoadQuest()
    {
  
        filePath = Application.persistentDataPath + "/" + "QuestData.txt";

        if (File.Exists(filePath))
        {
            qeData = new QuestData();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), qeData);

            foreach(Quest quest in qeData.playerQuests)
            {
                qManager.AcceptQuest(quest);
                quest.qGiver.UpdateQuestStatus();
            }

            file.Close();
        }
    }

    //모든 아이템 정보 불러오기
    private void LoadAllItemData()
    {
        ItemData[] datas = Resources.LoadAll<ItemData>("");
        foreach (ItemData data in datas)
        {
            itemDic.Add(data.ID, data);
        }
    }

   
    //파일 저장하기
    public void Save()
    {
        SavePlayer();
        SaveQuick();
        SaveQuest();
    }
}
