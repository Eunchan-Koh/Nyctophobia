using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float baseDamage;
    public float damage;
    public int count;
    public float posOffset;
    //회전 속도 같은거
    public float baseSpeed;
    public float speed;

    float timer;
    Player player;
    //only for id=7 - cont dmg area
    SpriteRenderer bulletSpriter;
    [Header("For bullet with deathCall Objects")]
    public float baseDeathcallSize;
    public float DeathcallSize;
    

    void Awake(){
        player = GameManager.instance.player;
    }
    
    
    void Update()
    {
        if(!GameManager.instance.isLive)
            return;
            
        switch(id){
            case 0://삽
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            // case 1://ice
            //     transform.Rotate(Vector3.back * speed * Time.deltaTime);
            //     break;
            case 5://ice
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 7:
                timer += Time.deltaTime;
                if(bulletSpriter){
                    bulletSpriter.color = new Color(1,1,1,(timer/speed)/2);
                    bulletSpriter.size = new Vector2(timer/speed, timer/speed);
                }
                if(timer > speed){
                    timer = 0f;
                    ColReset();
                }
                
                break;
            default://projectile
                timer += Time.deltaTime;
                if(timer > speed){
                    timer = 0f;
                    Fire();
                }
                break;
        }

        // if(Input.GetKeyDown(KeyCode.Space)){
        //     LevelUp(20,1);
        // }
    }
    void ColReset(){
        Collider2D col = transform.GetComponentInChildren<Collider2D>(true);
        col.enabled = true;
        StartCoroutine(colCancel());
        // col.enabled = false;
    }

    IEnumerator colCancel(){
        Collider2D col = transform.GetComponentInChildren<Collider2D>(true);
        yield return new WaitForSeconds(0.1f);
        col.enabled = false;

    }

    public void LevelUp(float damage, int count, float deathcallsize){//if there is no deathcall, set it to zero.
        this.baseDamage = damage;
        this.damage = damage;
        this.count += count;
        this.DeathcallSize += deathcallsize;

        if(id == 0)
            Batch();
        else if(id == 5)//ice
            Batch();
        else if(id == 7)//cont dmg area
            Batch2();
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        
    }

  
    public void Init(itemData data){
        // basic setting
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;
        // transform.localRotation = Quaternion.identity;

        // property set
        id = data.itemId;
        baseDamage = data.baseDamage;
        damage = baseDamage;
        baseDeathcallSize = data.baseEffectSize;
        DeathcallSize = baseDeathcallSize;

        count = data.baseCount;
        posOffset = data.posOffset;

        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++){
            if(data.projectile == GameManager.instance.pool.prefabs[i]){
                prefabId = i;
                break;
            }
        }

        switch(id){
            case 0://near attack rotation speed
                baseSpeed = 150;
                speed = baseSpeed;
                
                Batch();
                break;
            case 5://near attack rotation speed
                baseSpeed = 100;
                speed = baseSpeed;
                
                Batch();
                break;
            case 7://attack speed of cont. attack area
                baseSpeed = data.baseCount;
                speed = baseSpeed;
                Batch2();
                break;
            
            case 10://attack speed of cont. attack area
                baseSpeed = 0.8f;
                speed = baseSpeed;
                break;

            default://fires projectile cool speed
                baseSpeed = 0.3f;
                speed = baseSpeed;
                break;
        }
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

    }

    public void SetBulletDamageAgain(){
        switch(id){
            case 0://near attack rotation speed
                Batch();
                break;
            case 5://near attack rotation speed
                Batch();
                break;
            case 7://attack speed of cont. attack area
                Batch2();
                break;
            default://fires projectile cool speed

                break;
        }
    }

    void Batch(){
        for(int i = 0; i < count; i++){
            Transform bullet;
            if(i < transform.childCount){
                bullet = transform.GetChild(i);
            }else{
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
            

            //after changing parent, it's localposition value is changed. ex) if player is at (1,0,0), localposition of bullet is (-1,0,0) since poolmanager is at (0,0,0).
            //hence, reset the localposition to the position of bullet is based on player
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i/count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up*posOffset, Space.World);
            
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero, 0);//-100 means infinite Penetration.
        }
    }

    void Batch2(){
        
        Transform bullet;
        if(transform.childCount > 0){
            bullet = transform.GetChild(0);
        }else{
            bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.parent = transform;
        }
        bulletSpriter = bullet.GetComponent<SpriteRenderer>();
        speed = baseSpeed/(1+count*0.1f);
        //after changing parent, it's localposition value is changed. ex) if player is at (1,0,0), localposition of bullet is (-1,0,0) since poolmanager is at (0,0,0).
        //hence, reset the localposition to the position of bullet is based on player
        bullet.localPosition = Vector3.zero;
        bullet.localRotation = Quaternion.identity;

        bullet.Translate(bullet.up*posOffset, Space.World);
        Bullet tempbul = bullet.GetComponent<Bullet>();
        tempbul.transform.localScale = Vector3.one*DeathcallSize;
        tempbul.Init(damage, -100, Vector3.zero, 0);//-100 means infinite Penetration.
        
    }

    void Fire(){
        if(!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        bullet.GetComponent<Bullet>().Init(damage, count, dir, DeathcallSize);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
        
    }
 
}
