using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlayerScanRange : MonoBehaviour
{
    PlayerScan scanner;
    [Header("when player's light is")]
    public float OnRange;
    public float offRange;
    void Awake(){
        scanner = GetComponent<PlayerScan>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.instance.player.candleOn){
            scanner.scanRange = OnRange;
        }else{
            scanner.scanRange = offRange;
        }
    }
}
