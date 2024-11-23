using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    [Header("Look judgement")]
    public float lookRange;
    public float lookAngleDegree;
    public bool lookingAtIt;
    [Header("damage per second")]
    public float mentalDamageAmount;

    [Header("Others")]
    public float movingSpeed;
    public float lookMovingSpeed;
    [Range(0.0f, 7.0f)]
    public float stopDistance;
    Rigidbody2D rigid;
    CameraMovement CM;

    public float curSpeed;
    public float timeCheck;

    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        CM = Camera.main.GetComponent<CameraMovement>();
        curSpeed = movingSpeed;
    }

    void OnEnable(){
        transform.position = GameManager.instance.player.transform.position + new Vector3(11, 11, 0);
        timeCheck = 0;
    }
    
    

    void Update(){
        if(!GameManager.instance.isLive)
            return;

        
        timeCheck += Time.deltaTime;
        if(timeCheck > 5){
            RepositionL();
            timeCheck = 0;
        }

        LookCheck();
        if(lookingAtIt){
            GameManager.instance.MentalDamage(mentalDamageAmount*Time.deltaTime, 1);
            CM.zoomIn = true;
        }else{
            CM.zoomIn = false;
        }
    }

    void FixedUpdate(){
        if(!GameManager.instance.isLive)
            return;

        MovingMethodL();
    }

    
    public void MovingMethodL(){
        float distancePL = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
        if(distancePL > stopDistance){
            if(curSpeed < movingSpeed) curSpeed = Mathf.Lerp(curSpeed, movingSpeed, 0.5f);
            Vector3 newPos = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, Time.fixedDeltaTime*curSpeed);
            transform.position = newPos;
        }else{
            if(lookingAtIt){
                if(curSpeed>lookMovingSpeed) curSpeed = Mathf.Lerp(curSpeed, lookMovingSpeed, 0.5f);
                Vector3 newPos = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, Time.fixedDeltaTime*curSpeed);
                transform.position = newPos;
            }else{
                if(curSpeed>lookMovingSpeed) curSpeed = Mathf.Lerp(curSpeed, lookMovingSpeed*1.1f, 0.5f);
                Vector3 newPos = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, Time.fixedDeltaTime*curSpeed);
                transform.position = newPos;
            }
        } 
        
    }

    public void LookCheck(){
        Vector3 fromPtoThis;
        fromPtoThis = transform.position - GameManager.instance.player.transform.position;
        fromPtoThis = fromPtoThis.normalized;
        // Debug.Log(fromPtoThis);

        float angle;
        angle = Vector3.SignedAngle(fromPtoThis,GameManager.instance.player.lastFacing,GameManager.instance.player.lastFacing);
        // Debug.Log(temp);
        //temp shows the angle betwween player's lsat direction facing and this object.

        float distance = Vector3.Distance(GameManager.instance.player.transform.position, transform.position);
        if(angle <= lookAngleDegree && distance <= lookRange){
            lookingAtIt = true;
        }else{
            lookingAtIt = false;
        }
    }

    public void RepositionL(){
        float tempX = Random.Range(10,20);
        int minus = Random.Range(0,2);
        if(minus==0) tempX *= -1;
        float tempY = Random.Range(10,20);
        minus = Random.Range(0,2);
        if(minus==0) tempY *= -1;
        transform.position = GameManager.instance.player.transform.position + new Vector3(tempX, tempY, 0);
    }
}
