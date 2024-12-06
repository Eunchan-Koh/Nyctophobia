using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public itemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake(){
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable(){
        textLevel.text = "Lv." + (level+1);
        
        switch(data.itemType){
            case itemData.ItemType.Melee:
            case itemData.ItemType.Range:
                switch(data.itemId){
                    case 7:
                        if(level == 0)
                            textDesc.text = string.Format(data.itemInitialDesc);
                        else
                            textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]*10, data.effectSizes[level]*100);
                        break;
                    case 10:
                        if(level == 0)
                            textDesc.text = string.Format(data.itemInitialDesc);
                        else
                            textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.effectSizes[level]*100);
                        break;
                    default:
                        if(level == 0)
                            textDesc.text = string.Format(data.itemInitialDesc);
                        else
                            textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                        break;
                    
                }
                break;
            case itemData.ItemType.Glove:
            case itemData.ItemType.Shoe:
                if(level == 0)
                    textDesc.text = string.Format(data.itemInitialDesc);
                else
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            case itemData.ItemType.Madness:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case itemData.ItemType.Magnet:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            case itemData.ItemType.Shield:
                if(level == 0)
                    textDesc.text = string.Format(data.itemInitialDesc);
                else
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);//damage for cooltime, count for max stack
                break;
            default:
                if(level == 0)
                    textDesc.text = string.Format(data.itemInitialDesc);
                else
                    textDesc.text = string.Format(data.itemDesc);
                break;
        }
        
    }


    public void OnClick(){//!!!important! on itemData, items except magnet, damages[0] and counts[0] values are not being called.
                            //at level 0, uses base damage and base count, and at level 1, uses data.damage[1] and data.counts[1]. fix it, or leave the first data empty
        switch(data.itemType){
            case itemData.ItemType.Melee:
            case itemData.ItemType.Range:
                if(level == 0){
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);

                }else{
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    float nextDeathcallSize = data.baseEffectSize;
                    nextDamage += data.baseDamage * data.damages[level];//data.damage should be between 0~1(or more, they are %)
                    nextCount += data.counts[level];
                    nextDeathcallSize *= data.effectSizes[level];// this much will be added up to the size

                    weapon.LevelUp(nextDamage, nextCount, nextDeathcallSize);
                }

                level++;
                break;
            case itemData.ItemType.Glove:
            case itemData.ItemType.Shoe:
                if (level == 0){
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }else{
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate, 0);
                }

                level++;
                break;
            case itemData.ItemType.Madness:
                if (level == 0){
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }else{
                    float nextRate = data.damages[level];
                    int tempCount = data.counts[level];
                    gear.LevelUp(nextRate, tempCount);
                }

                level++;
                break;
             case itemData.ItemType.Magnet:
                GameManager.instance.player.itemMagnet.GetComponent<CircleCollider2D>().radius *= 1+data.damages[level];

                level++;
                break;
            case itemData.ItemType.Shield:
                if (level == 0){
                    //get shield, and activate //this active will be done from gamemanager
                    GameManager.instance.isShieldActive = true;
                    //reset max stack value and cooldown
                    GameManager.instance.shieldCooldown = data.baseDamage;
                    GameManager.instance.maxShieldStack += data.baseCount;

                }else{
                    //reset max stack value and cooldown value
                    // GameManager.instance.shieldCooldown = data.baseDamage * (1-data.damages[level]);//합연산
                    GameManager.instance.shieldCooldown -= GameManager.instance.shieldCooldown * data.damages[level];//곱연산
                    GameManager.instance.maxShieldStack += data.counts[level];
                }

                level++;
                break;
            case itemData.ItemType.Heal:
                GameManager.instance.HealthHeal(GameManager.instance.maxHealth);
                break;
        }

        //check if it is selected already for curItemHUD
        if(level == 1){
            GameManager.instance.uiLevelUp.UpdateCurItem(this);
        }

        

        if(level == data.damages.Length){
            GetComponent<Button>().interactable = false;
        }
    }
}
