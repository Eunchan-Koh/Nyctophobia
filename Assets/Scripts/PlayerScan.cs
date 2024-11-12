using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScan : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D target;
    public Vector2 targetLocation;

    void Awake(){
        targetLocation = transform.position;
    }
    void OnEnable(){
        targetLocation = transform.position;

    }


    void FixedUpdate(){
        //target layer is player, and there is only one player in a game
        //when there is nothing in the range, target value becomes null
        target = Physics2D.CircleCast(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        if(target){
            targetLocation = target.transform.GetComponent<Rigidbody2D>().position;
        }
        // nearestTarget = GetNearest();
    }

}
