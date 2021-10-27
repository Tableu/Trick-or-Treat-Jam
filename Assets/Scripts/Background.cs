using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Background : MonoBehaviour
{
    [SerializeField] private Sprite background;

    [SerializeField] private SpriteRenderer currentBackground;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [YarnCommand("SwitchBackground")]
    public void SwitchBackground()
    {
        currentBackground.sprite = background;
    }
}
