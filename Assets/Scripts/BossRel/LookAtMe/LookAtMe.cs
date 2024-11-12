using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{

    void Update(){
        Vector3 fromPtoThis;
        fromPtoThis = transform.position - GameManager.instance.player.transform.position;
        fromPtoThis = fromPtoThis.normalized;
        // Debug.Log(fromPtoThis);

        float angle;
        angle = Vector3.SignedAngle(fromPtoThis,GameManager.instance.player.lastFacing,GameManager.instance.player.lastFacing);
        // Debug.Log(temp);
        //temp shows the angle betwween player's lsat direction facing and this object.

        float distance = Vector3.Distance(GameManager.instance.player.transform.position, transform.position);
        if(angle <= 45 && distance <= 5){
            Debug.Log("MP damage!");
        }
    }
}
