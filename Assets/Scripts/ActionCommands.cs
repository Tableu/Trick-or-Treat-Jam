using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActionCommand
{
    public IEnumerator DoAction(GameObject gameObject);
}

public abstract class ActionScriptableObject : ScriptableObject
{
    public abstract ActionCommand MakeAction();
}