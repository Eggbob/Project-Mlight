using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    public GameObject GobeHunter;
    public GameObject Gobe;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Vector3 spawnPos = this.transform.position;
            spawnPos.y += 1f;
            Instantiate(GobeHunter, spawnPos, Quaternion.identity);
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            Vector3 spawnPos = this.transform.position;
            spawnPos.y += 1f;
            Instantiate(Gobe, spawnPos, Quaternion.identity);
        }

    }
}
