using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigestingMaze : MonoBehaviour
{
    public int RequriedCount;
    public int wrongCount;
    public int maxWrongCount;
    public Vector3 thisPos;
    public MazeDoor[] MazeDoors;
    public GameObject[] doors;
    public GameObject[] walls;
    public int size;
    public Vector3 cameraSize;
    void OnEnable(){
        // Time.timeScale = 0.001f;
        GameManager.instance.cMove.followingPlayer = false;
        transform.position = GameManager.instance.player.transform.position;
        thisPos = transform.position;
        wrongCount = 0;
        ResetDWPosition();
        // Time.timeScale = 1;

        //reset true door pos
        ResetDoorStat();
    
        BossManager.instance.ClearStack = RequriedCount;
        GameManager.instance.cMove.otherZoomIn = true;
        size = (int)GameManager.instance.cMove.GetBaseZoom();

        
    }

    void Update(){
        ResetDWPosition();
        if(wrongCount >= maxWrongCount){
            GameManager.instance.fadeInOut.fadeOutTime= 3;
            GameManager.instance.fadeInOut.FadeOut();
            GameManager.instance.GameOver();
        }
    }


    public void ResetDoorStat(){
        for(int i = 0; i < doors.Length; i++){
            MazeDoors[i].isCorrectDoor = false;
        }
        int ran = Random.Range(0,doors.Length);
        MazeDoors[ran].isCorrectDoor = true;
    }

    void OnDisable(){
        ClearDM();
    }
    void ClearDM(){
        GameManager.instance.cMove.followingPlayer = true;

        GameManager.instance.cMove.otherZoomIn = false;
    }

    void ResetDWPosition(){
        cameraSize = Camera.main.ViewportToWorldPoint (new Vector3(1.0f,1.0f,10.0f)) - Camera.main.ViewportToWorldPoint (new Vector3(0.0f,0.0f,10.0f));
        
        // Debug.Log(cameraSize);
        //right up
        walls[0].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(1.0f,1.0f,10.0f));
        walls[1].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(1.0f,1.0f,10.0f));
        //right down
        walls[2].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(1.0f,0.0f,10.0f));
        walls[3].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(1.0f,0.0f,10.0f));
        //left down
        walls[4].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(0.0f,0.0f,10.0f));
        walls[5].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(0.0f,0.0f,10.0f));
        //left up
        walls[6].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(0.0f,1.0f,10.0f));
        walls[7].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(0.0f,1.0f,10.0f));

        //right
        doors[0].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(1.0f,0.5f,10.0f));
        //left
        doors[1].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(0.0f,0.5f,10.0f));
        //down
        doors[2].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(0.5f,0.0f,10.0f));
        //up
        doors[3].transform.position = Camera.main.ViewportToWorldPoint (new Vector3(0.5f,1.0f,10.0f));

        // walls[0].transform.localScale = new Vector3((cameraSize.x-1)/2, (cameraSize.y-1)/2, walls[0].transform.localScale.z);
        //vertical walls
        walls[0].transform.localScale = new Vector3(walls[0].transform.localScale.x, (cameraSize.y-1)/2, walls[0].transform.localScale.z);
        walls[2].transform.localScale = new Vector3(walls[2].transform.localScale.x, (cameraSize.y-1)/2, walls[2].transform.localScale.z);
        walls[4].transform.localScale = new Vector3(walls[4].transform.localScale.x, (cameraSize.y-1)/2, walls[4].transform.localScale.z);
        walls[6].transform.localScale = new Vector3(walls[6].transform.localScale.x, (cameraSize.y-1)/2, walls[6].transform.localScale.z);

        //horizontal walls
        walls[1].transform.localScale = new Vector3((cameraSize.x-1)/2, walls[1].transform.localScale.y, walls[1].transform.localScale.z);
        walls[3].transform.localScale = new Vector3((cameraSize.x-1)/2, walls[3].transform.localScale.y, walls[3].transform.localScale.z);
        walls[5].transform.localScale = new Vector3((cameraSize.x-1)/2, walls[5].transform.localScale.y, walls[5].transform.localScale.z);
        walls[7].transform.localScale = new Vector3((cameraSize.x-1)/2, walls[7].transform.localScale.y, walls[7].transform.localScale.z);
    }

    public void SuccessDoor(){
        BossManager.instance.IncreaseCurStack();
    }

    public void FailDoor(){
        wrongCount++;
        GameManager.instance.MentalDamage(30);
        //additional effect
        GameManager.instance.cMove.ZoomInToInt(size+(wrongCount*wrongCount*4));
        // ResetDWPosition();
    }
}
