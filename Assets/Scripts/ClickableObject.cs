using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string node;
    [SerializeField] private bool clickable;
    [SerializeField] private SpriteRenderer glow;
    [SerializeField] private SpriteRenderer CG;
    [SerializeField] private Sprite[] switchingSprites;
    private SpriteRenderer img;
    private int index = 0;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickable)
        {
            Debug.Log("clicked on object");
            if(node != "NoNode")
                TransitionManager.Instance.dialogueRunner.StartDialogue(node);
            img = GetComponent<SpriteRenderer>();
            clickable = false;
            glow.enabled = false;
            if (CG != null)
            {
                CG.GetComponent<FadeInCG>().FadeInObject();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(clickable)
            glow.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(clickable)
            glow.enabled = false;
    }

    [YarnCommand("SetObjectClickable")]
    public void SetObjectClickable()
    {
        clickable = true;
    }
    [YarnCommand("FadeInObject")]
    public void FadeInObject()
    {
        StartCoroutine(FadeIn());
    }
    [YarnCommand("SwitchSprite")]
    public void SwitchSprite()
    {
        if (switchingSprites.Length > 0)
        {
            img.sprite = switchingSprites[index];
            index++;
        }
    }

    private IEnumerator FadeIn()
    {
        img.color = new Color(0, 0, 0, 1);
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }
}
