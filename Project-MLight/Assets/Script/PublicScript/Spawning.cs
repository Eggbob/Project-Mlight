﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [SerializeField]
    private GameObject gobeHunter;

    [SerializeField]
    private GameObject goblin;

    private Queue<Enemy> gobeHunterQueue = new Queue<Enemy>();
    private Queue<Enemy> gobeQueue = new Queue<Enemy>();
    
    private Vector3 spawnPos;

    private int enemyCount;

    private void Start()
    {
        Init(6);

        for(int i = 0; i<2; i++)
        {
            spawnPos = Random.insideUnitCircle * 30f; ;
            spawnPos.x += this.transform.position.x;
            spawnPos.z = spawnPos.y + this.transform.position.z;
            spawnPos.y = this.transform.position.y;

            var newObj = gobeQueue.Dequeue();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            newObj.transform.position = spawnPos;

            spawnPos = Random.insideUnitCircle * 30f; ;
            spawnPos.x += this.transform.position.x;
            spawnPos.z = spawnPos.y + this.transform.position.z;
            spawnPos.y = this.transform.position.y;

            newObj = gobeHunterQueue.Dequeue();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            newObj.transform.position = spawnPos;
        }
    }

    private void Init(int count)
    {
        for(int i = 0; i< count; i++)
        {
            gobeQueue.Enqueue(CreateGoblin());
            gobeHunterQueue.Enqueue(CreateGobHunter());
        }
    }

    //고블린 생성
    private Enemy CreateGoblin() 
    {
        var newObj = Instantiate(goblin, transform).GetComponent<Enemy>();
        newObj.AddDieAction(() => GobeDieRoutine(newObj));
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    //고블린 헌터 생성
    private Enemy CreateGobHunter()
    {
        var newObj = Instantiate(gobeHunter, transform).GetComponent<Enemy>();
        newObj.AddDieAction(() => GobeHunterDieRoutine(newObj));
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private void Update()
    {

        //if(Input.GetKeyDown(KeyCode.M))
        //{
        //    Vector3 spawnPos = this.transform.position;
        //    spawnPos.y += 1f;
        //    Instantiate(gobeHunter, spawnPos, Quaternion.identity);
        //}
        //if(Input.GetKeyDown(KeyCode.N))
        //{
        //    Vector3 spawnPos = this.transform.position;
        //    spawnPos.y += 1f;
        //    Instantiate(goblin, spawnPos, Quaternion.identity);
        //}

    }
    
    //고블린 사망시 동작
    private void GobeDieRoutine(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        enemy.transform.SetParent(transform);

        gobeQueue.Enqueue(enemy);

        enemy = gobeQueue.Dequeue();
        enemy.transform.SetParent(null);
        enemy.gameObject.SetActive(true);
        enemy.RestoreHealth(enemy.MaxHp);

        spawnPos = Random.insideUnitCircle * 10f; ;
        spawnPos.x += this.transform.position.x;
        spawnPos.z = spawnPos.y + this.transform.position.z;
        spawnPos.y = this.transform.position.y;


        enemy.transform.position = spawnPos;
    }
    
    //고블린헌터 사망시 동작
    private void GobeHunterDieRoutine(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        enemy.transform.SetParent(transform);

        gobeHunterQueue.Enqueue(enemy);

        enemy = gobeQueue.Dequeue();
        enemy.transform.SetParent(null);
        enemy.gameObject.SetActive(true);
        enemy.RestoreHealth(enemy.MaxHp);

        spawnPos = Random.insideUnitCircle * 6f; ;
        spawnPos.x += this.transform.position.x;
        spawnPos.z = spawnPos.y + this.transform.position.z;
        spawnPos.y = this.transform.position.y;


        enemy.transform.position = spawnPos;
    }


}
