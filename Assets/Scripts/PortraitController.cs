using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class PortraitController : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner.AddCommandHandler("FadeOutPortrait", FadeOutPortrait);
        dialogueRunner.AddCommandHandler("FadeInPortrait", FadeInPortrait);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [YarnCommand("SwitchPortrait")]
    public void SwitchPortrait(string character, string expression)
    {
        Character charEnum = (Character)Enum.Parse(typeof(Character), character);
        SpriteRenderer img = PortraitDB.Instance.GetSpriteRenderer(charEnum);
        img.sprite = PortraitDB.Instance.GetPortrait(charEnum, expression);
    }
    
    public void FadeInPortrait(string[] parameters, Action onComplete)
    {
        string character = parameters[1];
        string expression = parameters[2];
        Character charEnum = (Character)Enum.Parse(typeof(Character), character);
        SpriteRenderer img = PortraitDB.Instance.GetSpriteRenderer(charEnum);
        img.sprite = PortraitDB.Instance.GetPortrait(charEnum, expression);
        img.color = new Color(1, 1, 1, 0);
        StartCoroutine(FadeIn(img, onComplete));
    }
    
    public void FadeOutPortrait(string[] parameters, Action onComplete)
    {
        StartCoroutine(FadeOut(PortraitDB.Instance.GetSpriteRenderer(Character.Sherry),onComplete));
        StartCoroutine(FadeOut(PortraitDB.Instance.GetSpriteRenderer(Character.Danny),onComplete));
        //StartCoroutine(FadeOut(PortraitDB.Instance.GetSpriteRenderer(Character.Policeman),onComplete));
    }
    
    //https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/
    private IEnumerator FadeIn(SpriteRenderer img, Action onComplete)
    {
        for (float i = 0; i <= 1; i += 4*Time.deltaTime)
        {
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
        img.color = new Color(1, 1, 1, 1);
        onComplete();
    }
    //https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/
    private IEnumerator FadeOut(SpriteRenderer img, Action onComplete)
    {
        for (float i = 1; i >= 0; i -= 4*Time.deltaTime)
        {
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
        img.color = new Color(1, 1, 1, 0);
        onComplete();
    }
}
