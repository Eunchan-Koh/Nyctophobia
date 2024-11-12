using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{  
    Image image;
    Text text;
    bool fadeOut;
    float timer;
    [Range(0.0f, 5.0f)]
    public float fadeInSpeed;
    [Range(0.0f, 1.0f)]
    public float fadeOutTime;
    void Awake(){
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }

    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.E)){
        //     FadeOut();
        // }

        
        if(fadeOut){
            float tempVal = 0;
            timer += Time.deltaTime;
            tempVal = timer*fadeInSpeed;
            if(tempVal>1){
                fadeOut = false;
                tempVal = 1;
            }
            image.color = new Color(0,0,0,1-tempVal);
            text.color = new Color(1,1,1,1-tempVal);
        }
            
        
    }

    
   public void FadeOut(){
        StartCoroutine(FadeOutTime());
    }

    IEnumerator FadeOutTime(){
        fadeOut = false;
        image.color = new Color(0,0,0,1);
        text.color = new Color(1,1,1,1);
        yield return new WaitForSeconds(fadeOutTime);
        timer = 0;
        fadeOut = true;
    }
}
