using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Yarn.Unity;

public class AudioManager : MonoBehaviour
{
    static List<AudioSource> sources = new List<AudioSource>();
    public enum Sound
    {
        test,
    }

    //Clears any nonexistant audio sources
    public static void ClearNonexistantSources()
    {
        List<AudioSource> activeSources = new List<AudioSource>();
        foreach (AudioSource auSrc in sources)
        {
            if (auSrc != null)
            {
                activeSources.Add(auSrc);
            }
        }
        sources = activeSources;
    }

    //Gets next available audio source
    private static AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource auSource in sources)
        {
            if (!auSource.isPlaying)
            {
                return auSource;
            }
        }
        return AddNewAudioSource();
    }

    //Adds new audio source GameObject with an output to the SFX AudioMixer channel
    private static AudioSource AddNewAudioSource()
    {
        GameObject soundContainer = GameObject.Find("AudioSourceContainer");
        if (!soundContainer)
        {
            soundContainer = new GameObject("AudioSourceContainer");
        }
        GameObject soundGameObject = new GameObject($"Sound-{sources.Count}");
        soundGameObject.transform.SetParent(soundContainer.transform);
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        AudioMixer audioMixer = Resources.Load<AudioMixer>("sfxMixer");
        AudioMixerGroup[] audioMixGroup = audioMixer.FindMatchingGroups("SFX");
        audioSource.outputAudioMixerGroup = audioMixGroup[0];
        sources.Add(audioSource);
        return audioSource;
    }

    //Finds the called sound in AudioAssets
    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (AudioAssets.SoundAudioClip soundAudioClip in AudioAssets.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioclip;
            }
        }
        return null;
    }

    [YarnCommand("PlaySound")]
    //Play the sound
    public static void PlaySound(string soundName)
    {
        Sound sound = (Sound)Enum.Parse(typeof(Sound), soundName);
        GetAvailableAudioSource().PlayOneShot(GetAudioClip(sound));
    }
}
