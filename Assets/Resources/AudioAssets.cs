using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioAssets : MonoBehaviour
{
    private static AudioAssets _instance;

    public static AudioAssets instance
    {
        get
        {
            if (_instance == null) _instance = Instantiate(Resources.Load<AudioAssets>("AudioAssets"));
            return _instance;
        }
    }

    public SoundAudioClip[] soundAudioClipArray;
    [System.Serializable]
    public class SoundAudioClip
    {
        public AudioManager.Sound sound;
        public AudioClip audioclip;
        public AudioMixer audioMixer;
    }
}
