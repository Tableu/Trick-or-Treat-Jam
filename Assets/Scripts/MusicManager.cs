using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicClips;
    public AudioSource musicSrc;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        musicSrc = GetComponent<AudioSource>();
    }

    private void OnLevelWasLoaded(int level)
    {
        AudioClip levelMusic = musicClips[level];
        Debug.Log("Playing song: " + name);
        if (levelMusic)
        {
            musicSrc.clip = levelMusic;
            musicSrc.loop = true;
            musicSrc.Play();
        }
    }
}
