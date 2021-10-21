using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class MusicManager : MonoBehaviour
{
    //Music source and tracks
    public AudioClip[] musicClips;
    public AudioSource musicSrc;

    public enum Music
    {
        PumpkinMansOtherworld = 0,
        MementoMoriTitle = 1,
        LivingRoomDay = 2
    }
    // Don't destroy gameobject
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //Plays first index track since technically
    //no level was loaded at start.
    void Start()
    {
        musicSrc = GetComponent<AudioSource>();
        PlayMusic("MementoMoriTitle");
    }

    [YarnCommand("PlayMusic")]
    //Plays music track according to level index
    public void PlayMusic(string songToChangeTo)
    {
        
        int index = (int)Enum.Parse(typeof(Music), songToChangeTo);
        AudioClip levelMusic = musicClips[index];
        if (levelMusic)
        {
            musicSrc.clip = musicClips[index];
            musicSrc.loop = true;
            musicSrc.Play();
        }
        Debug.Log("Playing song: " + songToChangeTo);
    }
}
