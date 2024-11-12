using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    void Update()
    {
        if(!BossManager.instance.doingBossFight)
            this.gameObject.SetActive(false);
    }
}
