using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
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
    public GameObject FollowingObj;
    public CinemachineVirtualCamera cinemachineVC;
    CinemachineTransposer cineTran;
    public bool hasCameraDamping;
    public bool followingPlayer;
    public bool otherZoomIn;

    public float GetBaseZoom(){
        return baseZoom;
    }
    void Awake()
    {
        pix = GetComponent<PixelPerfectCamera>();
        maxZoom = 45;
        baseZoom = pix.assetsPPU;
        curVal = pix.assetsPPU;

        cineTran = cinemachineVC.GetCinemachineComponent<CinemachineTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        ZoomInCheck();
        FollowPlayerCheck();
        DampingCheck();
    }

    void ZoomInCheck(){
        if(otherZoomIn) return;
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

    public void ZoomInToInt(int size){
        pix.assetsPPU = size;
    }

    public void ZoomInReset(){
        pix.assetsPPU = Mathf.RoundToInt(baseZoom);
    }

    void FollowPlayerCheck(){
        if(followingPlayer){
            cinemachineVC.Follow = FollowingObj.transform;
        }else{
            cinemachineVC.Follow = null;
        }
    }

    void DampingCheck(){
        if(hasCameraDamping){
            cineTran.m_XDamping = 1;
            cineTran.m_YDamping = 1;
            cineTran.m_ZDamping = 1;
        }else{
            cineTran.m_XDamping = 0;
            cineTran.m_YDamping = 0;
            cineTran.m_ZDamping = 0;
        }
        

    }
}
