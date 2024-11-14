using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.SceneTemplate;
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
            filmGrain.intensity.Override(filmGrainVal);
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
        filmGrainVal = 1;
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
