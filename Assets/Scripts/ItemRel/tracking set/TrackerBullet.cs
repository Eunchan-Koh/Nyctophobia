using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerBullet : MonoBehaviour
{
    // public Bullet bullet;//to set dmg, penetration from BeingTraced.cs
    // // public Transform FollowingParent;//parent object with BeginTraced.cs
    // public Transform target;
    // public bool tracingAlr;
    // public float trackingSpeed;
    // Rigidbody2D rigid;
    // void Awake(){
    //     bullet = GetComponent<Bullet>();
    //     // bullet.Init(bullet.damage, bullet.per, Vector3.zero);
    //     rigid = GetComponent<Rigidbody2D>();
    // }
    // void FixedUpdate(){
    //     if(!target)
    //         return;
    //     Vector3 tempLoc;
    //     Vector3 direction;
    //     // tempLoc = Vector3.Lerp(transform.position, target.position, trackingSpeed*Time.fixedDeltaTime);//lerp쓰면 적 근처에서 느려짐
    //     tempLoc = target.position - transform.position;
    //     direction = tempLoc.normalized;
    //     rigid.MovePosition(transform.position + direction*trackingSpeed*Time.fixedDeltaTime);
    // }

    // void OnTriggerEnter2D(Collider2D coll){
    //     if(!coll.CompareTag("normalMonster"))
    //         return;

    //     tracingAlr = false;  
    // }
}
