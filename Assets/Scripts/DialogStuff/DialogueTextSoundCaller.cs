using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTextSoundCaller : MonoBehaviour
{
    [SerializeField]
    string _dialogueText;
    public string DialogueText
    {
        get { return _dialogueText; }
        set
        {
            _dialogueText = value;
            string charAdded = _dialogueText.Length > 0 ? $"{_dialogueText[_dialogueText.Length - 1]}" : $"";
            if (!string.IsNullOrWhiteSpace(charAdded))
            {
                //Call sound stuff let's gooooo!
            }
        }
    }
}
