using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;
    SpawnData curSpawnData;

    [SerializeField]
    int level;
    // int maxLevel = 0;

    float timer;

    void Awake(){
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        // foreach(Transform ob in spawnPoint){
        //     Debug.Log(ob.position);
        // }
        
    }

    void Update()
    {
        if(!GameManager.instance.isLive || GameManager.instance.MBLv < 3)
            return;

        //1st case
        timer += Time.deltaTime;
        if(timer>spawnData[0].spawnTime){// all ghost enemy spawns in the same time period
            timer = 0;
            
            if(GameManager.instance.maxGhostCount > GameManager.instance.ghostCount){//spawns
                Spawn();
                GameManager.instance.ghostCount++;
            }else {//don't spawn

            }
        }
        
        //2nd case - from basic enemy spawner
        /////////////////////////////////////////////
        // timer += Time.deltaTime;
        // level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime),maxLevel);
        // if(timer>spawnData[level].spawnTime){
        //     timer = 0;
        //     if(GameManager.instance.maxGhostCount > GameManager.instance.ghostCount){//spawns

        //         Spawn();
        //         GameManager.instance.ghostCount++;
        //     }else {//don't spawn

        //     }
        // }
        //////////////////////////////////////////////
        
        // if(Input.GetKeyDown(KeyCode.Space)){
        //     Spawn();
        // }

    }

    void Spawn(){
        int ranVal = Random.Range(0,spawnData.Length);
        GameObject ghostMonster = GameManager.instance.ghostPool.Get(ranVal);
        ghostMonster.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // ghostMonster.GetComponent<ghostMonster>().Init(spawnData[level]);
        curSpawnData = spawnData[ranVal];
        ghostMonster.GetComponent<ghostMonster>().Init(curSpawnData);
    }
}
