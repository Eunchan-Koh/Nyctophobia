using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowArea : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float slowRate;
    public LayerMask targetLayer;
    public float existTime;
    Animator anim;
    void Awake(){
        anim = GetComponent<Animator>();
    }
 

    void OnEnable(){
        StartCoroutine(TimeCheck());
    }

    IEnumerator TimeCheck(){
        yield return new WaitForSeconds(existTime -0.3f);
        anim.SetTrigger("Disappear");
        yield return new WaitForSeconds(0.35f);
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider){
        // if(!collider.CompareTag("normalMonster") || !collider.CompareTag("Player"))
        //     return;
        // Debug.Log("collider name "+collider.name);
        // Debug.Log("0. col layer "+collider.gameObject.layer);
        // Debug.Log("1. cal 1<<col layer "+(1 << collider.gameObject.layer));
        // Debug.Log("2. target layer "+(targetLayer.value));
        // Debug.Log("3. output" +((1 << collider.gameObject.layer) & targetLayer.value));
        if(((1 << collider.gameObject.layer) | targetLayer.value) != targetLayer.value)
            return;
        

        if(collider.GetComponent<Player>()){
            Player tempP = collider.GetComponent<Player>();
            tempP.speed = tempP.baseSpeed * (1-slowRate);
        }else if(collider.GetComponent<normalMonster>()){
            normalMonster tempM = collider.GetComponent<normalMonster>();
            tempM.speed = tempM.baseSpeed * (1-slowRate);
        }

    }
    void OnTriggerStay2D(Collider2D collider){
        // if(!collider.CompareTag("normalMonster") || !collider.CompareTag("Player"))
        //     return;
        // Debug.Log("collider name "+collider.name);
        // Debug.Log("0. col layer "+collider.gameObject.layer);
        // Debug.Log("1. cal 1<<col layer "+(1 << collider.gameObject.layer));
        // Debug.Log("2. target layer "+(targetLayer.value));
        // Debug.Log("3. output" +((1 << collider.gameObject.layer) & targetLayer.value));
        if(((1 << collider.gameObject.layer) | targetLayer.value) != targetLayer.value)
            return;
        

        if(collider.GetComponent<Player>()){
            Player tempP = collider.GetComponent<Player>();
            tempP.speed = tempP.baseSpeed * (1-slowRate);
        }else if(collider.GetComponent<normalMonster>()){
            normalMonster tempM = collider.GetComponent<normalMonster>();
            tempM.speed = tempM.baseSpeed * (1-slowRate);
        }

    }

    void OnTriggerExit2D(Collider2D collider){
        // if(!collider.CompareTag("normalMonster") || !collider.CompareTag("Player"))
        //     return;
        if(((1 << collider.gameObject.layer) | targetLayer.value) != targetLayer.value)
            return;
        
        if(collider.GetComponent<Player>()){
            Player tempP = collider.GetComponent<Player>();
            tempP.speed = tempP.baseSpeed;
        }else if(collider.GetComponent<normalMonster>()){
            normalMonster tempM = collider.GetComponent<normalMonster>();
            tempM.speed = tempM.baseSpeed;
        }

    }
}
