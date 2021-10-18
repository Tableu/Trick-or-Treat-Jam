using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public AudioSource uiSrc;
    public AudioClip uiClick;
    public AudioClip uiHover;
    public AudioClip uiBack;
    // Start is called before the first frame update
    void Start()
    {
        uiSrc = GetComponent<AudioSource>();
    }

    public void Click()
    {
        uiSrc.PlayOneShot(uiClick);
    }

    public void Hover()
    {
        uiSrc.PlayOneShot(uiHover);
    }

    public void Back()
    {
        uiSrc.PlayOneShot(uiBack);
    }
}
