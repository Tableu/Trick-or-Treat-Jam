using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _img;

    private bool switched;
    // Start is called before the first frame update
    void Start()
    {
        switched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!TransitionManager.Instance.InTransition && !switched)
        {
            StartCoroutine(FadeSwitch(PortraitDB.Instance.GetPortrait(Character.Sherry, "p1Neutral")));
        }
    }

    //https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/
    public IEnumerator FadeSwitch(Sprite sprite)
    {
        switched = true;
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            _img.color = new Color(1, 1, 1, i);
            yield return null;
        }
        _img.sprite = sprite;
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            _img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    public void Switch(Sprite sprite)
    {
        _img.sprite = sprite;
    }
}
