using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Yarn.Unity;

public class PortraitController : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private Color fadedColor;
    [SerializeField] private DialogueTextSoundCaller dialogueTextSoundCaller;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner.AddCommandHandler("FadeOutPortrait", FadeOutPortrait);
        dialogueRunner.AddCommandHandler("FadeInPortrait", FadeInPortrait);
        dialogueRunner.AddCommandHandler("FadeOutAllPortraits", FadeOutAllPortraits);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [YarnCommand("AnimateSpeaker")]
    public void AnimateSpeaker(string character)
    {
        Character charEnum;
        if(Character.TryParse(character, out charEnum))
        {
            List<SpriteRenderer> spriteRenderers = PortraitDB.Instance.characters.Select(o => o.characterSpriteRenderer).ToList();
            foreach(SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(fadedColor.r,fadedColor.g,fadedColor.b,spriteRenderer.color.a);
                spriteRenderer.size = new Vector2(1, 1);
            }
            SpriteRenderer img = PortraitDB.Instance.GetSpriteRenderer(charEnum);
            img.color = new Color(1, 1, 1, 1);
            img.size = new Vector2(1.1f, 1.1f);

            dialogueTextSoundCaller.dialogueSound = PortraitDB.Instance.GetDialogueSound(charEnum);
        }
    }
    [YarnCommand("SwitchPortrait")]
    public void SwitchPortrait(string character, string expression)
    {
        Character charEnum;
        if(Character.TryParse(character, out charEnum))
        {
            SpriteRenderer img = PortraitDB.Instance.GetSpriteRenderer(charEnum);
            img.sprite = PortraitDB.Instance.GetPortrait(charEnum, expression);
        }
    }
    
    public void FadeInPortrait(string[] parameters, Action onComplete)
    {
        string character = parameters[1];
        string expression = parameters[2];
        Character charEnum;
        if (Character.TryParse(character, out charEnum))
        {
            SpriteRenderer img = PortraitDB.Instance.GetSpriteRenderer(charEnum);
            Color color = img.color;
            img.sprite = PortraitDB.Instance.GetPortrait(charEnum, expression);
            img.color = new Color(color.r, color.g, color.b, 0);
            StartCoroutine(FadeIn(img, onComplete));
        }
    }
    
    public void FadeOutPortrait(string[] parameters, Action onComplete)
    {
        Character charEnum;
        if (Character.TryParse(parameters[1], out charEnum))
        {
            StartCoroutine(FadeOut(PortraitDB.Instance.GetSpriteRenderer(charEnum), onComplete));
        }
        else
        {
            onComplete();
        }
    }

    public void FadeOutAllPortraits(string[] parameters, Action onComplete)
    {
        List<SpriteRenderer> spriteRenderers =
            PortraitDB.Instance.characters.Select(o => o.characterSpriteRenderer).ToList();
        for (int index = 0; index < spriteRenderers.Count - 1; index++)
        {
            StartCoroutine(FadeOut(spriteRenderers[index], delegate { Debug.Log("portrait fade out"); }));
        }

        StartCoroutine(FadeOut(spriteRenderers[spriteRenderers.Count - 1], onComplete));
    }

    //https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/
    private IEnumerator FadeIn(SpriteRenderer img, Action onComplete)
    {
        var color = img.color;
        for (float i = 0; i <= 1; i += 4*Time.deltaTime)
        {
            img.color = new Color(color.r, color.g, color.b, i);
            yield return null;
        }
        img.color = new Color(color.r, color.g, color.b, 1);
        onComplete();
    }
    //https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/
    private IEnumerator FadeOut(SpriteRenderer img, Action onComplete)
    {
        var color = img.color;
        for (float i = img.color.a; i >= 0; i -= 4*Time.deltaTime)
        {
            img.color = new Color(color.r, color.g, color.b, i);
            yield return null;
        }
        img.color = new Color(color.r, color.g, color.b, 0);
        onComplete();
    }
}
