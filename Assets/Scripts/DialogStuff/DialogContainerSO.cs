using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogContainerSO : ScriptableObject
{
    [SerializeField]
    string _dialogID;
    [SerializeField, Range(0, 60f)]
    float _initialCharsPerSeconds = 10f;
    [SerializeField, TextArea]
    string _message;

    //TODO: Add initial Portrait info

    public string DialogID
    {
        get { return _dialogID; }
    }
    public string Message
    {
        get { return _message; }
    }
    public float CharsPerSecond
    {
        get { return _initialCharsPerSeconds; }
    }

    public void SetData(string ID, string message, float charsPerSecond)
    {
        _dialogID = ID;
        _message = message;
        _initialCharsPerSeconds = charsPerSecond;
    }
}
