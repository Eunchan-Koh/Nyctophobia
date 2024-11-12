using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SizeChanger : MonoBehaviour
{
    public void ChangeSize(Vector3 vecSize){
        this.GetComponent<Transform>().localScale = vecSize;
    }
}
