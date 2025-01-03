using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake(){
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collider){
        if(!collider.CompareTag("Area"))
            return;
        
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        

        switch(transform.tag){
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;

                float dirX = diffX < 0? -1 : 1;
                float dirY = diffY < 0? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);
                if(diffX > diffY){
                    transform.Translate(Vector3.right*dirX*60);
                }else if(diffX < diffY){
                    transform.Translate(Vector3.up*dirY*60);
                }
                break;
            case "normalMonster":
                if(coll.enabled){
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran;
                    if(dist.x > 30){
                        ran = new Vector3(0, Random.Range(-3f,3f), 0);
                    }else{
                        ran = new Vector3(Random.Range(-3f,3f), 0, 0);
                    }
                     
                    transform.Translate(ran + dist*2);
                    
                }
                
                break;
            case "ghostMonster":
                if(coll.enabled){
                    
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran;
                    if(dist.x > 30){
                        ran = new Vector3(0, Random.Range(-3f,3f), 0);
                    }else{
                        ran = new Vector3(Random.Range(-3f,3f), 0, 0);
                    }
                    transform.Translate(ran + dist*2);
                    coll.transform.GetComponent<ghostMonster>().PosReset();
                    
                }
                
                break;

        }
    }
}
