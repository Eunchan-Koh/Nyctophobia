using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MonoBehaviour
{
    public bool isCorrectDoor;
    DigestingMaze parentMaze;

    void OnEnable(){
        parentMaze = GetComponentInParent<DigestingMaze>();
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(!collision.CompareTag("Player")) 
            return;

        if(isCorrectDoor){
            parentMaze.SuccessDoor();
        }else{
            parentMaze.FailDoor();
        }
        GameManager.instance.player.transform.position = parentMaze.thisPos;
        parentMaze.ResetDoorStat();
    }
}
