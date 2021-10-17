using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    private static StoryManager _instance;
    [SerializeField] public List<Flag> flags;
    public static StoryManager Instance
    {
        get
        {
            if (_instance == null && !FindObjectOfType<StoryManager>())
            {
                _instance = new GameObject("SceneNavigationManager").AddComponent<StoryManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(this);
    }
    
    public bool GetFlag(string id)
    {
        return flags.Find(x=>x.id == id).flag;
    }

    public void SetFlag(string id, bool value)
    {
        Flag flag = flags.Find(x => x.id == id);
        if(flag != null)
            flag.flag = value;
    }
}

[System.Serializable]
public class StoryFlags
{
    
}

[System.Serializable]
public class Flag
{
    public bool flag;
    public string id;
}