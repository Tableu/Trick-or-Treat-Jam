using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static List<AudioSource> sources = new List<AudioSource>();
    public enum Sound
    {
        test,
    }

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
        sources.Add(audioSource);
        return audioSource;
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (AudioAssets.SoundAudioClip soundAudioClip in AudioAssets.instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioclip;
            }
        }
        return null;
    }


    public static void PlaySound(Sound sound)
    {
        GetAvailableAudioSource().PlayOneShot(GetAudioClip(sound));
    }
}
