
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public SpawnData FoodBoxData;
    public int maxFoodCount;
    public int curFoodCount;
    public float levelTime;

    int level;
    int maxLevel = 3;

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
        NormalMonsterSpawnCheck();
        BossMonsterSpawnCheck();
    }

    void NormalMonsterSpawnCheck(){
        if(!GameManager.instance.isLive || !GameManager.instance.MonsterSpawn)
            return;
        
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime),maxLevel);

        if(timer>spawnData[level].spawnTime){
            timer = 0;
            Spawn();
        }
    }
    void BossMonsterSpawnCheck(){
        if(!GameManager.instance.isLive || !BossManager.instance.doingBossFight){
            curFoodCount = 0;//임시로 해놓은거, 나중에 최적화할때 수정하셈
            return;
        }
        // Debug.Log("boss spawn called!");
        timer += Time.deltaTime;
        if(timer>FoodBoxData.spawnTime){
            if(curFoodCount >= maxFoodCount){
                timer = FoodBoxData.spawnTime;
            }else{
                timer=0;
                SpawnFoodBox();
            }
            
        }
    }

    void Spawn(){
        GameObject normalMonster = GameManager.instance.pool.Get(0);
        normalMonster.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        normalMonster.GetComponent<normalMonster>().Init(spawnData[level]);
    }

    public void SpawnFoodBox(){
        curFoodCount++;
        GameObject FoodBox = GameManager.instance.pool.Get(0);
        FoodBox.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        FoodBox.GetComponent<normalMonster>().Init(FoodBoxData);
    }


}

[System.Serializable]
public class SpawnData{
    public int spriteType;
    public float spawnTime;
    public float health;
    public float speed;
    public bool doesNotAttackPlayer;

}
