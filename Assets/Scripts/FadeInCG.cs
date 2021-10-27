using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class FadeInCG : MonoBehaviour
{
    [SerializeField] private SpriteRenderer img;

    public GameObject[] children;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [YarnCommand("FadeInObject")]
    public void FadeInObject()
    {
        var boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
            boxCollider.enabled = true;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        img.color = new Color(1, 1, 1, 0);
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }

        foreach (GameObject child in children)
        {
            child.SetActive(true);
        }
    }
}
