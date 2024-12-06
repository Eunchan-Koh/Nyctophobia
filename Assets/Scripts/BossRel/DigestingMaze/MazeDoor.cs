using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MonoBehaviour
{
    public bool isCorrectDoor;
    DigestingMaze parentMaze;
    public GameObject warningSign;
    public SpriteRenderer warningSprite;
    public float alpha;
    public float distance;
    // [Range(2.5f,0.0f)]
    // public float warningDistance;

    void OnEnable(){
        parentMaze = GetComponentInParent<DigestingMaze>();
        warningSprite = warningSign.GetComponent<SpriteRenderer>();

        resetWarningSize();
    }

    void Update(){
        distance = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
        alpha = 1- Vector3.Distance(transform.position, GameManager.instance.player.transform.position)/6;
        if(warningSprite && !isCorrectDoor){
            warningSprite.color = new Color(1,0,0,alpha);
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(!collision.CompareTag("Player")) 
            return;
        
        parentMaze.proceedTime = 0;

        if(isCorrectDoor){
            parentMaze.SuccessDoor();
        }else{
            parentMaze.FailDoor();
        }
        warningSprite.color = new Color(1,0,0,0);
        GameManager.instance.player.transform.position = parentMaze.thisPos;
        // GameManager.instance.player.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10));
        parentMaze.ResetDoorStat();
        GameManager.instance.BS.OnForSeconds(0.1f);
    }
    public void resetWarningSize(){
        Vector3 temp = Camera.main.ViewportToWorldPoint(Vector3.up) - Camera.main.ViewportToWorldPoint(Vector3.zero);
        float verticalSize = temp.y;
        verticalSize /= 1.2f;
        warningSign.transform.localScale = new Vector3(verticalSize, verticalSize, verticalSize);
    }
}
