using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;
    [Header("First Boss - gourmand")]
    public GameObject[] boss;
    public Text bossClearReqUI;
    public int ClearStack;
    public int CurStack1;
    public bool doingBossFight;
    float timer;
    public int curBossIndex;//fork = 0, look = 1
    
    void Awake()
    {
        instance = this;
        bossClearReqUI.enabled = false;
    }

    void Update(){
        bossClearReqUI.text = CurStack1+"/"+ClearStack;
    }

    public void StartBossStage(int index){
        ResetStacks();
        ResetUI();
        doingBossFight = true;
        curBossIndex = index;
        GameManager.instance.player.cannnotTurnOnCandle = true;
        GameManager.instance.MonsterSpawn = false;
        boss[index].SetActive(true);
    }


    public void IncreaseCurStack(){
        CurStack1++;
        if(CurStack1>= ClearStack){
            //clear, boss gone, return to normal condition. spawning normal monsters
            boss[curBossIndex].SetActive(false);
            doingBossFight = false;
            bossClearReqUI.enabled = false;
            GameManager.instance.player.cannnotTurnOnCandle = false;
            GameManager.instance.MonsterSpawn = true;
        }
    }

    void ResetStacks(){
        CurStack1 = 0;
    }

    void ResetUI(){
        bossClearReqUI.enabled = true;
    }
}
