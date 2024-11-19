using System.Collections;
using System.Collections.Generic;
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

    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        CM = Camera.main.GetComponent<CameraMovement>();
    }

    void OnEnable(){
        transform.position = GameManager.instance.player.transform.position + new Vector3(11, 11, 0);
    }
    
    

    void Update(){
        if(!GameManager.instance.isLive)
            return;

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
            Vector3 newPos = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, Time.fixedDeltaTime*movingSpeed);
            transform.position = newPos;
        }else{
            if(lookingAtIt){
                Vector3 newPos = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, Time.fixedDeltaTime*lookMovingSpeed);
                transform.position = newPos;
            }else{
                Vector3 newPos = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, Time.fixedDeltaTime*lookMovingSpeed*1.1f);
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
}
