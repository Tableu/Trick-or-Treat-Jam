using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using TMPText = TMPro.TextMeshProUGUI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private TMPText text;
    [SerializeField] private string soundDisclaimer;
    public DialogueRunner dialogueRunner;
    [SerializeField] private string warning;
    [SerializeField] private Image img;
    [SerializeField] private Button button;
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<GameObject> roomPrefabs;
    private GameObject _room;

    public enum Room
    {
        LivingRoomNight,
        DannyRoomNight,
        DannyRoomMorning,
        LivingRoomMorning,
        DannyRoomNight2,
        LivingRoomNight2,
        LivingRoomFogged,
        LivingRoomFoggedNote,
        DannyDoor
    }

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
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            StartCoroutine(FadeInMainMenu());
        }
        dialogueRunner.AddCommandHandler("RoomToRoomTransition", RoomToRoomTransition);
    }
    

    public IEnumerator FadeInMainMenu()
    {
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
    }

    public void GoToRoomScene()
    {
        button.gameObject.SetActive(false);
        img.gameObject.SetActive(true);
        StartCoroutine(MenuToRoomTransition());
    }
    [YarnCommand("LoadRoom")]
    public void LoadRoom(string room)
    {
        Room roomEnum;
        Debug.Log(room);
        if (Room.TryParse(room, out roomEnum))
        {
            if(_room != null)
                Destroy(_room);
            _room = Instantiate(roomPrefabs[(int)roomEnum]);
            Debug.Log("RoomLoaded");
        }
    }
    private IEnumerator MenuToRoomTransition()
    {
        yield return StartCoroutine(BlackFadeOut(img,"October 30th", delegate 
        {
            SceneManager.LoadScene("Scenes/Room Scene");
        }));
        StartCoroutine(BlackFadeIn(img,delegate
        {
            _room = GameObject.Find("LivingRoomNight");
            dialogueRunner.StartDialogue();
        }));
    }
    private void RoomToRoomTransition(string[] parameters, Action onComplete)
    {
        dialogueRunner.dialogueUI.DialogueComplete();
        img.gameObject.SetActive(true);
        StartCoroutine(BlackFadeOut(img,parameters[2], delegate
        {
            LoadRoom(parameters[1]);
            StartCoroutine(BlackFadeIn(img, delegate
            {
                img.gameObject.SetActive(false);
                onComplete();
            }));
        }));
    }
    
    public IEnumerator BlackFadeIn(Image img, Action callback)
    {
        img.color = new Color(0, 0, 0, 1);
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        Debug.Log("BlackFadeIn");

        callback();
    }

    public IEnumerator BlackFadeOut(Image img, string date, Action callback)
    {
        img.color = new Color(0, 0, 0, 0);
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        Debug.Log("BlackFadeOut");
        if (date != "NoDate")
        {
            text.gameObject.SetActive(true);
            text.text = date.Replace('_',' ');
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                text.color = new Color(1, 1, 1, i);
                yield return null;
            }

            yield return new WaitForSeconds(1);
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                text.color = new Color(1, 1, 1, i);
                yield return null;
            }
            text.gameObject.SetActive(false);
        }
        callback();
    }
}
