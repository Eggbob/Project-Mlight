using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    private static GameManager instance;
    
    public PlayerController Player { get; private set; }
    public Inventory Inven { get; private set; }
    public SkillBookManager SbookManager { get; private set; }
    public SaveManager sManager { get; private set; }

    public GameObject RespawnZone;
  

    public static GameManager Instance 
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        Player = FindObjectOfType<PlayerController>();
        Inven = FindObjectOfType<Inventory>();
        SbookManager = FindObjectOfType<SkillBookManager>();
        sManager = FindObjectOfType<SaveManager>();

        Player.AddDieAction(Respawn);
    }

    private void Start()
    {
       BgmManager.Instance.PlayBgm("Map");
    }

    private void Respawn()
    {
        StartCoroutine(ResapwnRoutine());
    }

    private IEnumerator ResapwnRoutine()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.RespawnUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        Player.transform.position = RespawnZone.transform.position;
        Player.RespawnPlayer();
       

        UIManager.Instance.RespawnUI.gameObject.SetActive(false);
    }
}
