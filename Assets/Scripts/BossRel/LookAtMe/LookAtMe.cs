using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.U2D;

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
    SpriteRenderer spriter;

    public float curSpeed;
    public float spriteTime;
    [Header("Clear Requirement")]
    public float timeCheck;
    public float reqTime;//required time for getting claer counts;
    public int curCount;
    public int RequriedCount = 3;

    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        CM = Camera.main.GetComponent<CameraMovement>();
        curSpeed = movingSpeed;
        spriter = GetComponent<SpriteRenderer>();
    }

    void OnEnable(){
        transform.position = GameManager.instance.player.transform.position + new Vector3(11, 11, 0);
        timeCheck = 0;
        CM.otherZoomIn = true;

        BossManager.instance.ClearStack = RequriedCount;
    }

    void OnDisable(){
        // GameManager.instance.player.FallingIntoIt = false;
        CM.otherZoomIn = false;
        CM.ZoomInReset();
    }
    
    

    void Update(){
        if(!GameManager.instance.isLive)
            return;

        spriter.flipX = GameManager.instance.player.transform.position.x > this.transform.position.x? true:false;
        
        spriteTime += Time.deltaTime;
        // if(Mathf.RoundToInt(spriteTime)%3 == 0){
        //     spriter.enabled = false;
        // }else{
        //     spriter.enabled = true;
        // }
        timeCheck += Time.deltaTime;
        
        if(timeCheck > reqTime){
            BossManager.instance.IncreaseCurStack();
            RepositionL();
            timeCheck = 0;
            GameManager.instance.BS.OnForSeconds(0.3f);
            CM.ZoomInToInt((int)CM.GetBaseZoom() + BossManager.instance.CurStack1*4);
        }

        LookCheck();
        if(lookingAtIt){
            GameManager.instance.MentalDamage(mentalDamageAmount*Time.deltaTime, 1);
            CM.zoomIn = true;
            timeCheck = 0;
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
            // GameManager.instance.player.FallingIntoIt = false;
            if(curSpeed < movingSpeed) curSpeed = Mathf.Lerp(curSpeed, movingSpeed, 0.5f);
            Vector3 newPos = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, Time.fixedDeltaTime*curSpeed);
            transform.position = newPos;
        }else{
            //make player's moving direction close to it.
            // GameManager.instance.player.FallingIntoIt = true;
            //check if player is looking at it - in stopDistance, gets slower. If player is looking at it, gets more slower.
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
        if(distance <= lookRange){
            GameManager.instance.player.PlayerCameraOffset.transform.position = (this.gameObject.transform.position + GameManager.instance.player.transform.position)/2;
            CM.FollowingObj = GameManager.instance.player.PlayerCameraOffset;
        }else{
            GameManager.instance.player.PlayerCameraOffset.transform.localPosition = Vector3.zero;
            CM.FollowingObj = GameManager.instance.player.PlayerCameraOffset;
        }
        if(angle <= lookAngleDegree && distance <= lookRange){
            lookingAtIt = true;
        }else{
            lookingAtIt = false;
        }
    }

    public void RepositionL(){
        //effect - 치지지직하면서 화면 글리치효과로 흔들리는거

        //repositioning
        float tempX = Random.Range(10,20);
        int minus = Random.Range(0,2);
        if(minus==0) tempX *= -1;
        float tempY = Random.Range(10,20);
        minus = Random.Range(0,2);
        if(minus==0) tempY *= -1;
        transform.position = GameManager.instance.player.transform.position + new Vector3(tempX, tempY, 0);
    }
}
