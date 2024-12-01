using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    Image image;
    void Awake(){
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void OnForSeconds(float seconds){
        StartCoroutine(OnForSecondsCor(seconds));
    }

    IEnumerator OnForSecondsCor(float seconds){
        image.enabled = true;
        yield return new WaitForSeconds(seconds);
        image.enabled = false;
    }
}
