using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostMonster : MonoBehaviour
{
    public float speed;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    public bool isAlive;
    public float mentalDamage;
    Animator anim;
    Collider2D coll;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    PlayerScan playerScanner;

    void Awake(){
        isAlive = true;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        playerScanner = GetComponentInChildren<PlayerScan>();
    }

    void FixedUpdate(){
        if(!GameManager.instance.isLive || GameManager.instance.MBLv < 3)
            return;

        if(playerScanner.target)
            target = playerScanner.target.transform.GetComponent<Rigidbody2D>();
        
        Vector2 dirVec = playerScanner.targetLocation - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate(){
        if(!GameManager.instance.isLive)
            return;

        if(target)
            spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable(){
        isAlive = true;
        // target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        playerScanner.targetLocation = transform.position;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 3;
        // anim.SetBool("Dead", false);
    }
    public void Init(SpawnData data){
        // anim.runtimeAnimatorController = animCon[data.spriteType];
        playerScanner.targetLocation = transform.position;
        speed = data.speed;
    }

    public void PosReset(){
        playerScanner.targetLocation = transform.position;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(!collider.transform.CompareTag("Player"))
            return;

        //give mental damage
        GameManager.instance.MentalDamage(mentalDamage,2);
        //add effects for mental damage
        // GameManager.instance.fadeInOut.FadeOut();
        //make mentalDamage method contains mental damage ui(after setting another method for candle off damage), or keep ui effect on ghostmonsters to make each monsters has their own UIs
        
        //disappears
        isAlive = false;
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 1;
        
        //this method should be called from animator, not in script. used for temporary now:
        Disappear();
        //spawners will respawn the ghost monsters as well


    }

    void Disappear(){
        gameObject.SetActive(false);
        GameManager.instance.ghostCount--;
    }



}
