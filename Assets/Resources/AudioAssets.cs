using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioAssets : MonoBehaviour
{
    private static AudioAssets _instance;

    //Loads the AudioAssets prefab
    public static AudioAssets instance
    {
        get
        {
            if (_instance == null) _instance = Instantiate(Resources.Load<AudioAssets>("AudioAssets"));
            return _instance;
        }
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
