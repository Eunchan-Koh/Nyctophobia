using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class normalMonster : MonoBehaviour
{
    public float baseSpeed;
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isAlive = true;
    public bool cannotMove = false;

    Animator anim;
    Collider2D coll;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    [Header("Is it foodBox?")]
    public bool isFoodBox;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        wait = new WaitForFixedUpdate();
        cannotMove = false;
    }

    
    void FixedUpdate()
    {
        if(!GameManager.instance.isLive)
            return;
        
        if(!isAlive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        if(isFoodBox && !BossManager.instance.doingBossFight){
            gameObject.SetActive(false);
        }

        if(cannotMove)
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        
    }

    void LateUpdate(){
        if(!GameManager.instance.isLive)
            return;
            
        if(!isAlive)
            return;

        if(cannotMove)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable(){
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isAlive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data){
        anim.runtimeAnimatorController = animCon[data.spriteType];
        baseSpeed = data.speed;
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
        isFoodBox = data.doesNotAttackPlayer;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(!collider.CompareTag("Bullet") || !isAlive)
            return;

        Bullet colBul = collider.GetComponent<Bullet>();
        TakeDamage(colBul);
        if (colBul.hasAdditionalEffect){
            StopCoroutine("KnockBack");
            StopCoroutine(Paralized(colBul.effectTime));
            StartCoroutine(Paralized(colBul.effectTime));
        }
        
    }

    IEnumerator Paralized(float time){
        cannotMove = true;
        rigid.velocity = Vector2.zero;
        spriter.color = new Color(1,1,0,1);
        yield return new WaitForSeconds(time);
        cannotMove = false;
        spriter.color = new Color(1,1,1,1);
    }


    public void TakeDamage(Bullet bull){
        health -= bull.damage;
        // Debug.Log("received damage, " + bull.damage + "!");

        if(health > 0){
            //alive, hit action
            StartCoroutine("KnockBack");
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }else{
            //dead
            isAlive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            if(isFoodBox){
                GameObject food = GameManager.instance.pool.GetSub(1);
                food.transform.position = transform.position;
            }
            anim.SetBool("Dead", true);
            GameManager.instance.killCount++;
            //drops exp ball
            GameObject expBall = GameManager.instance.pool.Get(4);//4th is the exp ball object
            expBall.transform.position = transform.position;
            // GameManager.instance.GetExp();
            if(GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

        }
    }

    IEnumerator KnockBack(){

        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);


    }
    void Dead(){
            
        gameObject.SetActive(false);
    }
}
