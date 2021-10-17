using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private string soundDisclaimer;
    [SerializeField] private string warning;
    [SerializeField] private Image img;
    [SerializeField] private Button button;
    [SerializeField] private Canvas canvas;

    public bool InTransition { get; set; }

    private static TransitionManager _instance;
    public static TransitionManager Instance
    {
        get
        {
            if (_instance == null && !FindObjectOfType<TransitionManager>())
            {
                _instance = new GameObject("TransitionManager").AddComponent<TransitionManager>();
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
        DontDestroyOnLoad(canvas);
    }
    private void Start()
    {
        InTransition = false;
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            StartCoroutine(FadeInMainMenu());
        }
    }
    

    public IEnumerator FadeInMainMenu()
    {
        InTransition = true;
        button.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            text.color = new Color(1, 1, 1, i);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            text.color = new Color(1, 1, 1, i);
            yield return null;
        }
        text.text = warning;
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            text.color = new Color(1, 1, 1, i);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            text.color = new Color(1, 1, 1, i);
            yield return null;
        }

        yield return new WaitForSeconds(2);
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        img.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        InTransition = false;
    }

    public void GoToRoomScene()
    {
        button.gameObject.SetActive(false);
        img.gameObject.SetActive(true);
        StartCoroutine(MenuToRoomTransition());
    }
    private IEnumerator MenuToRoomTransition()
    {
        
        yield return StartCoroutine(BlackFadeOut(img));
        SceneManager.LoadScene("Scenes/Room Scene");
        yield return StartCoroutine(BlackFadeIn(img));
        InTransition = false;
    }
    public IEnumerator BlackFadeIn(Image img)
    {
        InTransition = true;
        img.color = new Color(0, 0, 0, 1);
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        InTransition = false;
    }

    public IEnumerator BlackFadeOut(Image img)
    {
        InTransition = true;
        img.color = new Color(0, 0, 0, 0);
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        InTransition = false;
    }
}
