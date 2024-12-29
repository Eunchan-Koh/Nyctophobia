using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager instance;
    Volume vol;
    [Header("vignette")]
    public float[] IntensityLevel;
    Vignette vignette;
    float timeCheck;
    public float offset;
    public float offsetStrength;
    public float currentVal;
    public float targetVal;

    [Header("film grain")]
    public float a1;
    FilmGrain filmGrain;
    public float FGDisSpeed;

    [Range(0.0f, 1.0f)]
    public float filmGrainVal;
    [Header("Chromatic Aberration")]
    public float a2;
    public float CADisSpeed;
    ChromaticAberration ca;
    public float caVal;

    [Range(-100, 0)]
    public float saturationVal;
    ColorAdjustments colorAdj;
    public float satRecoverSpeed;
    public bool satRecovering;
    public bool setGrayScale;


    //this volume is not sound volume! it is UI effects
    //this only manages vignette yet
    void Awake(){
        instance = this;
        vol = GetComponent<Volume>();
        vol.profile.TryGet(out Vignette vig);
        vignette = vig;
        vol.profile.TryGet(out FilmGrain fg);
        filmGrain = fg;
        vol.profile.TryGet(out ChromaticAberration Ca);
        ca = Ca;
        vol.profile.TryGet(out ColorAdjustments CAJ);
        colorAdj = CAJ;
        saturationVal = 0;
    }
    void Update(){
        if(!GameManager.instance.isLive)
            return;
        timeCheck += Time.deltaTime;
        offset = Mathf.Sin(timeCheck)*offsetStrength;
        targetVal = Mathf.Lerp(targetVal, currentVal+offset, 0.5f*Time.deltaTime);
        vignette.intensity.Override(targetVal);

        // filmGrainTimeCheck += ;
        if(filmGrainVal > 0){
            filmGrainVal -= Time.deltaTime*FGDisSpeed;
            // filmGrain.intensity.Override(filmGrainVal);
            GameManager.instance.noiseMat.SetFloat("_Alpha", filmGrainVal);
        }else{
            filmGrainVal = 0;
        }
        // filmGrain.intensity.Override(filmGrainVal);

        if(caVal > 0){
            caVal -= Time.deltaTime*CADisSpeed;
            ca.intensity.Override(caVal);
        }else{
            caVal = 0;
        }

        //grayscale setting
        if(!setGrayScale && saturationVal < 0)
            saturationVal += Time.deltaTime*satRecoverSpeed;
        colorAdj.saturation.Override(saturationVal);

    }

    public void GrayScale(){
        setGrayScale = true;
        // saturationVal = -100;
        saturationVal = -50;
    }

    public void GrayScaleRecover(float forTime){
        StopCoroutine(GrayScaleFor(forTime));
        StartCoroutine(GrayScaleFor(forTime));
    }
    IEnumerator GrayScaleFor(float forTime){
        setGrayScale = true;
        yield return new WaitForSeconds(forTime);
        setGrayScale = false;
    }

    public void IntensityChange(int MBLv){
        if(MBLv >= IntensityLevel.Length){
            Debug.Log("error, no intensity level set for current mblv " + MBLv);
            return;
        }
        currentVal = IntensityLevel[MBLv];
        timeCheck = 0;
    }
    public void CallCAEffect(){
        caVal = 1;
    }

    public void CallFilmGrainEffect(){
        if(filmGrainVal < 0.8f)
            filmGrainVal += Time.deltaTime;
        else
            filmGrainVal = 0.8f;
    }
    public void CHangeFilmGrain(int index){
        switch(index){
            case 0:
                filmGrain.type.Override(FilmGrainLookup.Thin1);
                break;
            case 1:
                filmGrain.type.Override(FilmGrainLookup.Medium1);
                break;
            case 2:
                filmGrain.type.Override(FilmGrainLookup.Large01);
                break;
            default:
                break;
        }

        
    }
}
