using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    //Music source and tracks
    public AudioClip[] musicClips;
    public AudioSource musicSrc;

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
        PlayMusic(SceneManager.GetActiveScene().buildIndex);
    }

    //OnLevelWasLoaded only works for scene changes
    //AFTER game is first loaded.
    //This will play any further scene changes.
    private void OnLevelWasLoaded(int levelIndex)
    {
        PlayMusic(levelIndex);
    }

    //Plays music track according to level index
    void PlayMusic(int songToChangeTo)
    {
        
        AudioClip levelMusic = musicClips[songToChangeTo];
        if (levelMusic)
        {
            musicSrc.clip = musicClips[songToChangeTo];
            musicSrc.loop = true;
            musicSrc.Play();
        }
        Debug.Log("Playing song: " + songToChangeTo);
    }
}
