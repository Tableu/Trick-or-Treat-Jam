using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string node;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked on object");
        TransitionManager.Instance.dialogueRunner.StartDialogue(node);
    }
}
