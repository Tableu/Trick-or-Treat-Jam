using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string node;
    private bool clickable;
    private SpriteRenderer img;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked on object");
        TransitionManager.Instance.dialogueRunner.StartDialogue(node);
        img = GetComponent<SpriteRenderer>();
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
