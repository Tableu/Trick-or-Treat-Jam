using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitController : MonoBehaviour
{
    [SerializeField] private Image _img;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeSwitch(_img));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/
    public IEnumerator FadeSwitch(Image img)
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            _img.color = new Color(1, 1, 1, i);
            yield return null;
        }
        _img = img;
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            _img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    public void Switch(Image img)
    {
        _img = img;
    }
}
