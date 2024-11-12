using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public itemData.ItemType type;
    public float rate;

    public void Init(itemData data){
        // basic set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        //property set
        type = data.itemType;
        rate = data.damages[0];

        //for item 'madness'
        if(data.counts[0] != 0){
            GameManager.instance.MaxMentalReduce(data.counts[0]);
        }
        ApplyGear();
    }

    public void LevelUp(float rate, int maxMentalReduce){
        this.rate = rate;
        //for item 'madness'
        if(maxMentalReduce != 0)
            GameManager.instance.MaxMentalReduce(maxMentalReduce);
        ApplyGear();
    }

    void ApplyGear(){
        switch(type){
            case itemData.ItemType.Glove:
                RateUp();
                break;
            case itemData.ItemType.Shoe:
                SpeedUp();
                break;
            case itemData.ItemType.Madness:
                DamageUp();
                break;

        }
    }

    void RateUp(){
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons){
            float tempSpeed = weapon.baseSpeed;
            switch(weapon.id){
                case 0://melee
                    weapon.speed = tempSpeed + (tempSpeed*rate);
                    break;
                default://projectile
                    weapon.speed = tempSpeed * (1f-rate);
                    break;
            }
        }
    }

    void SpeedUp(){
        float speed = GameManager.instance.player.baseSpeed;
        GameManager.instance.player.speed = speed + speed*rate;
    }

    void DamageUp(){//only for item 'madness'
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons){
            float tempDamage = weapon.baseDamage;
            switch(weapon.id){
                default:
                    weapon.damage = tempDamage  + (tempDamage * rate * (GameManager.instance.MBLv + 1));
                    weapon.SetBulletDamageAgain();
                    // Debug.Log("called!");
                    break;
             }
        }
    }
}
