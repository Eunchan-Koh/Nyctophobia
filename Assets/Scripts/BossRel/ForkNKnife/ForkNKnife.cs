using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class ForkNKnife : MonoBehaviour
{
    Rigidbody2D rigid;
    Collider2D coll;
    public float dmg;
    public float mentalDmg;
    public float height;
    // public float waitTime;
    bool isMoving;
    float timer;
    float tempX;
    float tempY;
    float targetY;
    int bottomCount;
    [Range(0.0f, 5.0f)]
    public float LerpSpeed;
    
    //method2
    Vector3 pullPos;
    public int pattern;
    Vector3 targetPos;

    //method3
    Vector3 pos1;
    float waitTime = 2;

    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        isMoving = true;
        bottomCount = 0;
        pullPos = transform.position;
        pos1 = Camera.main.GetComponentsInChildren<Transform>()[1].position;
    }

    void OnEnable(){
        transform.position = Camera.main.GetComponentsInChildren<Transform>()[1].position;
    }

    void Update(){
        if(!GameManager.instance.isLive)
            return;

        // MovingMethod1();
        // MovingMethod2();
        MovingMethod3();
        
    }

    void MovingMethod1(){
        timer += Time.deltaTime;

        if(timer>0){
            tempY = Mathf.Sin(timer*timer) * height;
            if(tempY < 0) tempY = 0;
            if(tempY != 0){
                if(!isMoving)
                    isMoving = true;
                tempX = GameManager.instance.player.transform.position.x;
                tempX = Mathf.Lerp(transform.position.x, tempX, LerpSpeed * Time.deltaTime);
                targetY = GameManager.instance.player.transform.position.y;
            }else{
                if(isMoving){
                    bottomCount++;
                    isMoving = false;
                }
                

                tempX = transform.position.x;
                targetY = transform.position.y;
                // StartCoroutine(hold(waitTime));
            }
                

            transform.position = new Vector3(tempX, targetY + tempY, transform.position.z);
            if(bottomCount>15){
                timer = 0;
                bottomCount = 0;
            }
        }
    }

    void MovingMethod2(){
        //pulling
        if(pattern == 0){
            transform.position = new Vector3(transform.position.x, transform.position.y + (LerpSpeed*Time.deltaTime), transform.position.z);
            if(transform.position.y >= pullPos.y+height){
                
                pattern = 1;
                pullPos = GameManager.instance.player.transform.position;
                coll.enabled = true;
            }
        }
        //shooting
        if(pattern == 1){
            transform.position = Vector3.MoveTowards(transform.position, pullPos, LerpSpeed * 10 * Time.deltaTime);
            if(transform.position == pullPos){
                pattern = 0;
                coll.enabled = false;
            }
        }
    }
    void MovingMethod3(){
        //waiting
        timer += Time.deltaTime;
        if(pattern == 0){
            pos1 = Camera.main.GetComponentsInChildren<Transform>()[1].position;
            transform.position = Vector3.Lerp(transform.position, pos1, 2.0f * Time.deltaTime);
            if(timer >= waitTime){
                timer = 0;
                pattern = 1;
                pullPos = GameManager.instance.player.transform.position;
            }
        }
        //shooting
        if(pattern == 1){
            transform.position = Vector3.MoveTowards(transform.position, pullPos, LerpSpeed * 10 * Time.deltaTime);
            float tempDis = Vector3.Distance(transform.position, pullPos);
            if(tempDis< 1.0f) coll.enabled = true;
            if(transform.position == pullPos){
                StartCoroutine(HoldFork());
            }
        }
    }

    IEnumerator HoldFork(){
        yield return new WaitForSeconds(0.5f);
        pattern = 0;
        coll.enabled = false;
    }

    // IEnumerator hold(float waitFor){
    //     move = false;
    //     yield return new WaitForSeconds(waitFor);
    //     move = true;
    // }

    void OnTriggerEnter2D(Collider2D collider){
        //hits player or food
        if(collider.CompareTag("Player")){
            GameManager.instance.HealthDamage(dmg);
            GameManager.instance.MentalDamage(mentalDmg);
            //effect for getting hit(mental)
        }else if(collider.CompareTag("Food")){//there is no food yet
            //스택쌓음, 일정개수 쌓이면 bossManager안에서 클리어판정내고 gamemanager에 spawnMonster 다시 true로 바꾸기, 현재 보스 비활성화 등등 일어남
            BossManager.instance.IncreaseCurStack();
            collider.gameObject.SetActive(false);
        }else{
            return;
        }
            
            


    }
}
