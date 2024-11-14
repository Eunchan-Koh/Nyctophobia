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
    
    

    void Update(){
        if(!GameManager.instance.isLive)
            return;
        
        LookCheck();
        if(lookingAtIt){
            GameManager.instance.MentalDamage(mentalDamageAmount*Time.deltaTime, 1);
            
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
