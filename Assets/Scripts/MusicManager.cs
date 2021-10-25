using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class MusicManager : MonoBehaviour
{
    //Music source and tracks
    public List<MusicTrack> musicClips;
    public AudioSource musicSrc;
    public AudioSource motiveSrc;

    private bool isPlayingMotive = false;
    public enum Track
    {
        MementoMoriTitle,
        PumpkinMansBreath,
        LivingRoomDay,
        DannysAgony,
        DannysRoom,
        HelpfulClue2,
        SherrysFear,
        SherrysPain,
        Silence,
        PumpkinManSpeaks,
        PlayMotive,
        LivingRoomNight,
        DannyIsGone,
        TerrifyingClue
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

    void Update()
    {
        if (isPlayingMotive && !motiveSrc.isPlaying)
        {
            musicSrc.UnPause();
            isPlayingMotive = false;
        }
    }
    [YarnCommand("PlayMusic")]
    //Plays music track according to level index
    public void PlayMusic(string songToChangeTo)
    {
        
        int index = (int)Enum.Parse(typeof(Track), songToChangeTo);
        AudioClip levelMusic = musicClips[index].audioClip;
        if (levelMusic)
        {
            if (musicClips[index].loop)
            {
                StartCoroutine(StartFade(musicSrc, 0.3f,0, delegate
                {
                    musicSrc.clip = musicClips[index].audioClip;
                    musicSrc.loop = musicClips[index].loop;
                    musicSrc.Play();
                    StartCoroutine(StartFade(musicSrc, 0.3f, 1, delegate { }));
                }));
            }
            else
            {
                StartCoroutine(StartFade(musicSrc, 0.3f, 0, delegate
                {
                    musicSrc.Pause();
                    motiveSrc.clip = musicClips[index].audioClip;
                    motiveSrc.loop = false;
                    motiveSrc.Play();
                    isPlayingMotive = true;
                    StartCoroutine(StartFade(motiveSrc, 0.3f, 1, delegate { }));
                }));
            }
        }
        Debug.Log("Playing song: " + songToChangeTo);
    }

    [YarnCommand("StopMusic")]
    public void StopMusic()
    {
        musicSrc.Stop();
    }
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume, Action callback)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        callback();
        yield break;
    }
}

[System.Serializable]
public class MusicTrack
{
    public bool loop;
    public AudioClip audioClip;
    public MusicManager.Track track;
}
