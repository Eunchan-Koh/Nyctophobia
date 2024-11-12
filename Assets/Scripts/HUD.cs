using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health, MP, MBLv }

    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake(){
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate(){
        switch(type){
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length-1)];
                mySlider.value = curExp/maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("LV.{0:F0}", GameManager.instance.level + 1);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.killCount);
                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                
                // float remainTime = GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime/60f);
                int second = Mathf.FloorToInt(remainTime%60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, second);
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                mySlider.value = curHealth/maxHealth;
                break;
            case InfoType.MP:
                float curMP = GameManager.instance.mentalPoint;
                float maxMP = GameManager.instance.maxMentalPoint;
                mySlider.value = curMP/maxMP;
                break;
            case InfoType.MBLv:
                myText.text = string.Format("MBLv.{0:F0}", GameManager.instance.MBLv);
                break;
        }
    }
}
