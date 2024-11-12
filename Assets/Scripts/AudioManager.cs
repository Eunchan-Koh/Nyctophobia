using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    public int bgmChannels;
    AudioSource[] bgmPlayer;
    AudioHighPassFilter bgmEffect;
    public int currentBgmIndex;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win }

    void Awake(){
        instance = this;
        Init();
    }

    void Init(){
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = new AudioSource[bgmChannels];
        currentBgmIndex = 0;
        for(int i = 0; i < bgmChannels; i++){
            bgmPlayer[i] = bgmObject.AddComponent<AudioSource>();
            bgmPlayer[i].playOnAwake = false;
            bgmPlayer[i].loop = true;
            bgmPlayer[i].volume = bgmVolume;
            if(i<bgmClips.Length)
                bgmPlayer[i].clip = bgmClips[i];
            else{
                bgmPlayer[i].clip = bgmClips[bgmClips.Length];
            }
        }
        
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        for(int i = 0; i < sfxPlayers.Length; i++){
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
            
        }
    }

    public void ChangeBgm(int index){
        if(index >= bgmClips.Length){
            Debug.Log("bgm index is out of bound!");
            return;
        }
        //changing bgm causes lag. Find out why, and fix it
            // expecting: because audio files are not loaded?
            //근데 그래서 각 브금별로 채널에 따로 넣어주고 플레이/멈춤만 작동시키는데에도 렉걸림
        // PlayBgm(false, currentBgmIndex);
        // PlayBgm(true, index);
        currentBgmIndex = index;
        
    }

    public void PlayBgm(bool isPlay, int index){
        if(isPlay){
            bgmPlayer[index].Play();
        }else{
            bgmPlayer[index].Stop();
        }
    }

    public void EffectBgm(bool isPlay){
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx){//channelIndex means last channel's index that has been used
        for(int i = 0; i < sfxPlayers.Length; i++){
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee){
                ranIndex = Random.Range(0,2);
            }
            
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;

        }
        
    }

}
