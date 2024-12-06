using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    Animator anim;
    public int maxItemCount;
    public int curItemCount = 0;
    public Item[] selectedItems;
    public Image[] itemUI;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
        anim = GetComponentInChildren<Animator>();

        curItemCount = 0;

        selectedItems = new Item[maxItemCount];
        for(int i = 0; i < maxItemCount; i++){
            selectedItems[i] = null;
        }
    }

    public void Show(){
        Next();
        rect.localScale = Vector3.one;
        // anim.SetTrigger("Showing");
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
    }

    public void Hide(){
        rect.localScale = Vector3.zero;
        // anim.SetTrigger("Hiding");
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
        //add animation - since time scale is 0, make sure to use animator's update mode to unscaled time

        //then resume
        
    }

    public void Hide2(){//will be called from levelupUI animation event
        
    }

    public void UpdateCurItem(Item receivedItem){
        selectedItems[curItemCount] = receivedItem;
        itemUI[curItemCount].sprite = receivedItem.data.itemIcon;
        itemUI[curItemCount].color = new Color(1,1,1,1);
        curItemCount++;
    }

    public void Select(int index){
        items[index].OnClick();
    }

    void Next(){
        //1 모든 아이템 비활성화
        foreach(Item item in items){
            item.gameObject.SetActive(false);
        }
        //2 그 중 랜덤 3개 아이템 활성
        int[] ran = new int[3];
        if(curItemCount < maxItemCount){
            while(true){
                ran[0] = Random.Range(0,items.Length);
                ran[1] = Random.Range(0,items.Length);
                ran[2] = Random.Range(0,items.Length);

                if(ran[0] != ran[1] && ran[0] != ran[2] && ran[1] != ran[2])
                break; 
                
            }
            for(int i = 0; i < ran.Length; i++){
                Item ranItem = items[ran[i]];

                //3 만렙 아이템은 소비 아이템으로 대체
                if(ranItem.level == ranItem.data.damages.Length){
                    ranItem = items[4];
                }
                ranItem.gameObject.SetActive(true);
                
            }
        }else{
            while(true){
                ran[0] = Random.Range(0,selectedItems.Length);
                ran[1] = Random.Range(0,selectedItems.Length);
                ran[2] = Random.Range(0,selectedItems.Length);

                if(ran[0] != ran[1] && ran[0] != ran[2] && ran[1] != ran[2])
                break; 
                
            }
            for(int i = 0; i < ran.Length; i++){
                Item ranItem = selectedItems[ran[i]];

                //3 만렙 아이템은 소비 아이템으로 대체
                if(ranItem.level == ranItem.data.damages.Length){
                    ranItem = items[4];
                }
                ranItem.gameObject.SetActive(true);
                
            }
        }
        

        

        
    }
}
