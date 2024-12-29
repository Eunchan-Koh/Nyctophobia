using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Game Control")]
    public bool isLive;//time pause is not on for true
    public float gameTime;
    public float maxGameTime;
    public bool MonsterSpawn;

    [Header("Player Info")]
    public float health;
    public float maxHealth = 100;
    public float BaseMaxHealth;
    public float mentalPoint;
    public float maxMentalPoint;
    public float BaseMaxMentalPoint = 100;
    public int MBLv;
    public int maxMBLv;
    [Header("candle's continuous values")]
    public int mentalDamageRate;
    public int mentalHealRate;
    [Header("Mental damage translation to HP damage rate")]
    [Range(0.0f, 1.0f)]
    public float MPHPTransRate;
    [Header("other in-game values")]
    public int level;
    public int killCount;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("Game Objects")]
    public PoolManager pool;
    public GhostPoolManager ghostPool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;
    public int ghostCount;
    public int maxGhostCount;
    //
    [Header("Shield related")]
    GameObject shield;
    public float shieldCooldown;
    float curShieldCooldown;
    public int maxShieldStack;
    public int curShieldStack;
    public bool isShieldActive = false;
    [Header("Boss Related")]
    public float[] bossTime;
    public int curBossStage;
    public GameObject bossEntrancePage;
    [Header("Ghost related")]
    public FadeInOut fadeInOut;
    [Header("Camera Related")]
    public CameraMovement cMove;
    public BlackScreen BS;

    [Header("testing values")]
    public Image NoiseEffectScreen;
    public Material noiseMat;
    public int curBossIndex;
    public List<int> doneBoss;
    bool checkingBoss = false;


    void Awake(){
        instance = this;
        curBossStage = 0;
        noiseMat = NoiseEffectScreen.material;
        noiseMat.SetFloat("_Alpha", 0);

        doneBoss = new List<int>();
    }

    public void GameStart(){
        gameTime = 0;
        //initial monsterSpawn - set to true
        MonsterSpawn = true;
        //initial health & mental point setting
        maxHealth = BaseMaxHealth;
        health = maxHealth;
        maxMentalPoint = BaseMaxMentalPoint;
        mentalPoint = maxMentalPoint;
        MBLv = 0;

        //player's shield setting
        shield = player.shield;
        shield.SetActive(false);
        maxShieldStack = 0;
        curShieldStack = 0;
        curShieldCooldown = 0;
        isShieldActive = false;

        //current amount of ghost in the game scene
        ghostCount = 0;
        
        //boss stage count
        curBossStage = 0;

        //임시 스크립트 (first character)
        uiLevelUp.Select(1);

        //enemyCleaner is huge bullet to get rid of all enemies; must be setActive to false when GameStart
        enemyCleaner.SetActive(false);
        Resume();

        // VolumeManager.instance.IntensityChange(MBLv);

        AudioManager.instance.PlayBgm(true, 0);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver(){
        StartCoroutine("GameOverRoutine");
    }

    IEnumerator GameOverRoutine(){
        isLive = false;

        yield return new WaitForSeconds(0.5f);
        
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false, AudioManager.instance.currentBgmIndex);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory(){
        StartCoroutine("GameVictoryRoutine");
    }

    IEnumerator GameVictoryRoutine(){
        isLive = false;//이래야 에너미 클리너로 경험치가 안들어옴
        enemyCleaner.transform.position = player.transform.position;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry(){
        SceneManager.LoadScene(0);
    }

    void Update(){
        
        //exit check
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
        if(!isLive)
            return;
        
        gameTime += Time.deltaTime;

        //boss stage
        if(!checkingBoss && curBossStage < bossTime.Length && !BossManager.instance.doingBossFight && gameTime > bossTime[curBossStage]){
            checkingBoss = true;
            curBossStage++;
            bossEntrancePage.SetActive(true);

            while(true){
                int ranBossVal = Random.Range(0,BossManager.instance.boss.Length);
                //check if this boss is selected already
                if(doneBoss.Contains(ranBossVal)){
                    continue;
                }
                doneBoss.Add(ranBossVal);
                curBossIndex = ranBossVal;
                break;

            }
            
            BossManager.instance.StartBossStage(curBossIndex);
            checkingBoss = false;
            // bossEntrancePage.SetActive(true);
            // BossManager.instance.StartBossStage(curBossStage);
            // curBossStage++;
        }
        if(gameTime > maxGameTime){
            gameTime = maxGameTime;
            GameVictory();
        }



        CheckShield();

        //takes mental point damages every second if candle is off
        if(!player.candleOn){
            float mentalDmgPerTime = Time.deltaTime*mentalDamageRate;
            if(MBLv < 4 || mentalPoint >= 2)
                MentalDamage(mentalDmgPerTime);
        }else{
            float mentalHealPerTime = Time.deltaTime*mentalHealRate;
            MentalHeal(mentalHealPerTime);
        }
        
        
    }

    void CheckShield(){
        if(!isShieldActive) 
            return;
        
        //check curShieldStack and set activity status of shield
        if(curShieldStack>0 && !shield.activeSelf){
            shield.SetActive(true);
        }else if(curShieldStack <= 0 && shield.activeSelf){
            shield.SetActive(false);
        }

        //not on max stack yet, then count the cooldown
        if(maxShieldStack > curShieldStack){//not on max stack yet
            curShieldCooldown += Time.deltaTime;
        }else{//already on max stack, no need to count cooldown
            curShieldCooldown = 0;
        }
        //when cooldown count is over required value, add up stack
        if(curShieldCooldown > shieldCooldown){
            //only happens when curShieldStack < maxShieldStack due to lines above (if maxShieldStack>){curShieldCooldown += Time.deltaTime}
            curShieldStack++;
            curShieldCooldown = 0;
        }
    }

    public void MentalDamage(float dmg){
        float tempCal = mentalPoint - dmg;
        //tempcal은 양수, 즉 정신력이 데미지보다 높음
        if(tempCal>=0)
            mentalPoint = tempCal;
        else if(tempCal<0){//tempcal은 음수, 남은 정신력보다 데미지가 높음
            //아직 mblv이 맥스 레벨이 아님
            if(MBLv < maxMBLv){
                MBLvIncrease();
                mentalPoint = maxMentalPoint + tempCal;
                
                
            }else{//이미 맥스레벨임
                mentalPoint = 0;
                health += tempCal*MPHPTransRate;//tempCal이 음수이므로 데미지임
                //체력이 줄었으므로 캐릭터 사망체크
                player.PlayerDeadCheck();
            }
            
            
        }
        
    }
    public void MentalDamage(float dmg, int effectIndex){
        float tempCal = mentalPoint - dmg;
        //tempcal은 양수, 즉 정신력이 데미지보다 높음
        if(tempCal>=0)
            mentalPoint = tempCal;
        else if(tempCal<0){//tempcal은 음수, 남은 정신력보다 데미지가 높음
            //아직 mblv이 맥스 레벨이 아님
            if(MBLv < maxMBLv){
                MBLvIncrease();
                mentalPoint = maxMentalPoint + tempCal;
                
                
            }else{//이미 맥스레벨임
                mentalPoint = 0;
                health += tempCal*MPHPTransRate;//tempCal이 음수이므로 데미지임
                //체력이 줄었으므로 캐릭터 사망체크
                player.PlayerDeadCheck();
            }
        }

        //effect
        switch(effectIndex){
            case 1://지글지글 이펙트 - LookAtMe 전용
                VolumeManager.instance.CallFilmGrainEffect();
                break;
            case 2:
                VolumeManager.instance.CallCAEffect();
                fadeInOut.FadeOut();
                break;
        }
        
    }

    public void HealthDamage(float dmg){
        health -= dmg;
        //체력이 줄었으므로 캐릭터 사망체크
        player.PlayerDeadCheck();
        //피격 이펙트
        StopCoroutine(HitEffect());
        StartCoroutine(HitEffect());
    }

    IEnumerator HitEffect(){
        SpriteRenderer temp = player.getSpriter();
        // Debug.Log("check hitEffect");
        temp.color = new Color(1,1,1,0.5f);
        yield return new WaitForSeconds(0.5f);
        temp.color = new Color(1,1,1,1);

    }

    public void MBLvIncrease(){
        MBLv++;
        //MLBv에 따른 이펙트
        if(MBLv > 2){
            VolumeManager.instance.GrayScale();
        }
        //mblv이 늘었으므로 이펙트 체크 함수 넣을것
        VolumeManager.instance.IntensityChange(MBLv);
        //오디오도 변화함
        AudioManager.instance.ChangeBgm(MBLv);
        //mblv이 늘었으므로 madness 아이템 수치 다시 조정해줘야함
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void MentalHeal(float healAmount){
        float tempCal = mentalPoint + healAmount;
        if(tempCal > maxMentalPoint)
            mentalPoint = maxMentalPoint;
        else
            mentalPoint = tempCal;
    }

    public void HealthHeal(float healAmount){
        float tempCal = health + healAmount;
        if(tempCal > maxHealth)
            health = maxHealth;
        else
            health = tempCal;
    }

    public void GetExp(){
        if(!isLive)
            return;

        exp++;
        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)]){
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void MaxMentalReduce(float amount){//in percentage
        maxMentalPoint -= BaseMaxMentalPoint * 0.01f * amount;
        if(mentalPoint >= maxMentalPoint){
            mentalPoint = maxMentalPoint;
        }
    }
    public void MaxMentalIncrease(float amount){//in percentage
        maxMentalPoint += BaseMaxMentalPoint * 0.01f * amount;
    }
    public void MaxHealthReduce(float amount){//in percentage
        maxHealth -= BaseMaxHealth * 0.01f * amount;
        if(health >= maxHealth){
            health = maxHealth;
        }
    }
    public void MaxHealthIncrease(float amount){//in percentage
        maxHealth += BaseMaxHealth * 0.01f * amount;
    }

    public void Stop(){
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume(){
        isLive = true;
        Time.timeScale = 1;
    }

    public void SlowForAnimation(){
        Time.timeScale = 0.001f;
    }

}
