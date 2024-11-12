using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exp : MonoBehaviour
{
    public LayerMask ItemMagnetLayers;

    void OnTriggerEnter2D(Collider2D collision){
        if(((1 << collision.gameObject.layer) | ItemMagnetLayers) == ItemMagnetLayers){
            //up exp from gamemanager
            GameManager.instance.GetExp();
            //set active to false
            gameObject.SetActive(false);
        }
            // Debug.Log("collided with magnet!");
    }
}
