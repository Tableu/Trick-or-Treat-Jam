using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioAssets : MonoBehaviour
{
    private static AudioAssets _instance;

    //Loads the AudioAssets prefab
    public static AudioAssets Instance
    {
        get
        {
            if (_instance == null && !FindObjectOfType<AudioAssets>())
            {
                _instance = Instantiate(Resources.Load<AudioAssets>("AudioAssets"));
            }

            return _instance;
        }
    }
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(this);
    }
    //Array that holds all the audioClips (names are found in the enum in SoundManager.cs)
    public SoundAudioClip[] soundAudioClipArray;
    [System.Serializable]
    public class SoundAudioClip
    {
        public AudioManager.Sound sound;
        public AudioClip audioclip;
    }
}
