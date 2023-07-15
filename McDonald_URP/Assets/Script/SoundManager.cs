using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public GameObject SoundObject;
    public AudioClip[] BGM;
    [Range(0,1)]
    public float BGMVolume = 0.5f;
    [Range(0,1)]
    public float SFXVolume = 0.5f;
    public enum SoundState
    {
        BGM,
        SFX
    }
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        SetAudio(BGM[Random.Range(0,BGM.Length)], SoundState.BGM, true);
    }
    void Update()
    {

    }
    public void SetAudio(AudioClip audio,SoundState soundState, bool looping)
    {
        var sound = Instantiate(SoundObject,Camera.main.transform.position,Quaternion.identity)
        .GetComponent<AudioSource>();
        sound.volume = soundState == SoundState.BGM ? BGMVolume : SFXVolume;
        sound.clip = audio;
        sound.GetComponent<Sound>().soundState = soundState;
        sound.loop = looping;
        sound.Play();
        Destroy(sound.gameObject,audio.length);
    }
}
