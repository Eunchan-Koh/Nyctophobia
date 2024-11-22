using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossEntrancePage : MonoBehaviour
{
    public Image[] images;
    public Text[] texts;
    public bool fadeOuting;
    public float fadeOutSpeed;
    public float curVal;
    Animator anim;

    void OnEnable(){
        curVal = 1;
        GameManager.instance.isLive = false;
        // GameManager.instance.SlowForAnimation();
        // anim = GetComponent<Animator>();
        // anim.speed = 1.0f/Time.timeScale;
        for(int i = 0; i < images.Length; i++){
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, curVal);
        }
        for(int i = 0; i < texts.Length; i++){
            texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, curVal);
        }
    }
    void Update(){
        if(fadeOuting && curVal > 0){
            
            curVal -= Time.deltaTime*fadeOutSpeed;
            if (curVal<0) curVal = 0;
            for(int i = 0; i < images.Length; i++){
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, curVal);
            }
            for(int i = 0; i < texts.Length; i++){
                texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, curVal);
            }
            if(curVal == 0){
                // GameManager.instance.isLive = true;
                fadeOuting = false;
                this.gameObject.SetActive(false);
            }
        
        }
    }
    void AnimationEnd(){
        fadeOuting = true;
        GameManager.instance.Resume();
    }
}
