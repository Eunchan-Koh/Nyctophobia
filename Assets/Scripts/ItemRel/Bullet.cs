using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : MonoBehaviour
{
    [Header("Being Initialized from Item")]
    //데미지
    public float damage;
    //관통 가능 숫자
    public int per;
    [Header("Set Directly from Bullet Object")]
    public bool noRotation;
    Rigidbody2D rigid;
    Collider2D col;
    [Header("For summoning effect objects - keep empty if not used")]
    public GameObject DeathCallObject;
    public int DCObjectIndex;
    public float deathcallObjectSize;
    [Header("for hit effects - such as paralysis")]
    public bool hasAdditionalEffect;
    public float effectTime;

    


    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update(){
        RotationCheck();
        // DistanceCheck();//projectiles flies forever without this method


        
    }

    void RotationCheck(){
        if(!noRotation)
            return;
        
        transform.rotation = Quaternion.identity;
    }



    void DistanceCheck(){
        if(Vector2.Distance(GameManager.instance.player.transform.position, transform.position)>50){
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
    public void Init(float damage, int per, Vector3 dir, float deathcallSize){

        this.damage = damage;
        this.per = per;

        if(deathcallSize != 0)
            deathcallObjectSize = deathcallSize;
        if(per >= 0){//not melee weapon! projectile!
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D coll){
        if(!coll.CompareTag("normalMonster") || per == -100)
            return;
        

        per--;

        if(per <= -1){//now projectile should disappear
            if(DeathCallObject){
                //getDeathCallObject
                GameObject temp = GameManager.instance.pool.GetSub(DCObjectIndex);
                temp.transform.localScale = Vector3.one*deathcallObjectSize;
                // GameObject temp = Instantiate(DeathCallObject, GameManager.instance.transform);
                temp.transform.position = coll.transform.position;
            }
            gameObject.SetActive(false);
        }   
    }

    void OnTriggerExit2D(Collider2D coll){
        if(!coll.CompareTag("Area") || per == -100)
            return;
        
        
        gameObject.SetActive(false);//the bullet exist far away from screen - set active to false
    }
}
