using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeingTraced : MonoBehaviour
{
    // public GameObject childObject;
    // Bullet bullet;
    // GameObject child;
    // TrackerBullet childBullet;
    // Scanner scanner;
    // void Awake(){
    //     scanner = GetComponent<Scanner>();
    //     child = Instantiate(childObject, transform);
    //     bullet = GetComponent<Bullet>();
    //     child.GetComponent<Bullet>().Init(bullet.damage, bullet.per, Vector3.zero);
    //     child.transform.parent = GameManager.instance.transform;//to use fixed position
    //     child.transform.localPosition = Vector3.zero;
    //     child.transform.localRotation = Quaternion.identity;
        
    //     childBullet = child.GetComponent<TrackerBullet>();

    //     // childBullet.FollowingParent = this.transform;
        
    //     // childBullet.bullet.Init(bullet.damage, bullet.per, Vector3.zero);
    //     //object with BeingTraced.cs will have no collider! make sure to turn off the colliders of objects with BeingTrace.cs
    //     //child's data setting (dmg, penetration)
    // }

    // void FixedUpdate(){
    //     //if scanner finds targets
    //         //if nearestTarget exist and childbullet is not following anything, trace the found enemy until it hits the enemy once
    //     //if not, trace this gameobject
    //     Debug.Log("child bullet is " + childBullet);
    //     if(!childBullet.tracingAlr){
    //         if(scanner.targets!=null && scanner.nearestTarget){

    //             childBullet.tracingAlr = true;
    //             childBullet.target = scanner.nearestTarget;

                
    //         }else{
    //             //trace this object
    //             childBullet.target = this.transform;
    //         }
    //     }
        
    // }
}
