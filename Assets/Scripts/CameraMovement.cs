using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraMovement : MonoBehaviour
{
    PixelPerfectCamera pix;
    float curVal;
    public bool zoomIn;
    public float zoomInSpeed;
    public float zoomOutSpeed;
    // [SerializeField]
    float maxZoom;
    float baseZoom;
    void Awake()
    {
        pix = GetComponent<PixelPerfectCamera>();
        maxZoom = 30;
        baseZoom = pix.assetsPPU;
        curVal = pix.assetsPPU;
    }

    // Update is called once per frame
    void Update()
    {
        if(zoomIn){
            if(curVal < maxZoom)
                curVal += Time.deltaTime*zoomInSpeed;
            else
                curVal = maxZoom;
        }else{
            if(curVal > baseZoom)
                curVal -= Time.deltaTime*zoomOutSpeed;
            else
                curVal = baseZoom;
        }
        pix.assetsPPU = Mathf.RoundToInt(curVal);
    }
}
