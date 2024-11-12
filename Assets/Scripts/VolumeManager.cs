using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager instance;
    public float[] IntensityLevel;

    Volume vol;
    Vignette vignette;

    float timeCheck;
    public float offset;
    public float offsetStrength;
    public float currentVal;
    public float targetVal;

    //this volume is not sound volume! it is UI effects
    //this only manages vignette yet
    void Awake(){
        instance = this;
        vol = GetComponent<Volume>();
        vol.profile.TryGet(out Vignette vig);
        vignette = vig;
    }
    void Update(){
        if(!GameManager.instance.isLive)
            return;
        timeCheck += Time.deltaTime;
        offset = Mathf.Sin(timeCheck)*offsetStrength;
        targetVal = Mathf.Lerp(targetVal, currentVal+offset, 0.5f*Time.deltaTime);
        vignette.intensity.value = targetVal;

    }

    public void IntensityChange(int MBLv){
        if(MBLv >= IntensityLevel.Length){
            Debug.Log("error, no intensity level set for current mblv " + MBLv);
            return;
        }
        currentVal = IntensityLevel[MBLv];
        timeCheck = 0;
    }
}
