using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    AudioSource audioSource;
    public SoundManager.SoundState soundState;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        var sm = SoundManager.instance;
        audioSource.volume 
        = soundState == SoundManager.SoundState.BGM ? sm.BGMVolume : sm.SFXVolume;
    }
}
