using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private Dictionary<int, ItemData> itemDic = new Dictionary<int, ItemData>();

    private PlayerController pCon;
    private Inventory inven;

    private PlayerData pData;

    private void Start()
    {
        pCon = GameManager.Instance.Player;
        inven = GameManager.Instance.Inven;

        LoadAllItemData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Load();
        }
    }

    //파일 저장하기
    private void Save()
    {
        SavePlayer();
    }
    //플레이어 저장
    private void SavePlayer()
    {          
        pData = new PlayerData(pCon.Level,
        pCon.Exp, pCon.Hp, pCon.Mp, pCon.Power,
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
            float skillexp = (pCon.psCon.PlayerSkills[i].MaxSkillExp + pCon.psCon.PlayerSkills[i].SkillExp) - pCon.psCon.PlayerSkills[i].MaxSkillExp;
            //pData.saveSKillList.Add(pCon.psCon.PlayerSkills[i]);
            pData.saveSkills.Add(pCon.psCon.PlayerSkills[i].Skillid, skillexp );         
        }
        
       // string jsonData = JsonUtility.ToJson(pData, true);

        string jsonData = JsonConvert.SerializeObject(pData);
        File.WriteAllText(Application.persistentDataPath + "/" + "playerData.json", jsonData);
        Debug.Log(Application.persistentDataPath);
    }

    //저장한 데이터 로드하기
    private void Load()
    {      
        LoadPlayer();
    }


    //플레이어 정보 불러오기
    private void LoadPlayer()
    {
        if (!File.Exists(Application.persistentDataPath + "/" + "playerData.json"))
        {
            SavePlayer();
        }

        string jsonData = File.ReadAllText(Application.persistentDataPath + "/" + "playerData.json");


        pData = JsonConvert.DeserializeObject<PlayerData>(jsonData);
        //JsonUtility.FromJson<PlayerData>(jsonData);

        pCon.statusInit(pData.saveLevel,
        pData.saveExp, pData.saveHp, pData.saveMp,
        pData.savePower, pData.saveInt, pData.saveDef,
        pData.saveStatPoint);


        pCon.gameObject.transform.position = pData.savePos;
        inven.GetGold(pData.saveGold);

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

            if(pData.saveSkills.TryGetValue(skill.Skillid, out exp))
            {
               skill.GetExp(exp/100);
            }

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
}
