using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MementoMori.Dialog
{
    public class DialogManager : MonoBehaviour
    {

        //Add protrait thing.
        DialogBox textBox;

        [SerializeField, Header("Transition Vars"), Range(0.01f, 2f)]
        float showTime = 0.5f;
        [SerializeField]
        float hideTime = 0.5f;

        [SerializeField]
        bool isShown = false;

        bool isShowing = false;
        bool isHiding = false;

        CanvasGroup canvasGroup;

        static DialogManager _instance;
        public static DialogManager Instance
        {
            get
            {
                if (_instance == null && !FindObjectOfType<DialogManager>())
                {
                    //This thing doesn't exist, create one pls.
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
            }
            _instance = this;

            canvasGroup = GetComponent<CanvasGroup>();
            textBox = transform.Find("DialogBox").GetComponent<DialogBox>();

            textBox.Init();

            Reset();
        }

        public void Reset()
        {
            isShown = false;
            isShowing = false;
            isHiding = false;
            //Maybe do stuff to the canvasGroup if we want to do non-1 alpha dialog stuff.
            //canvasGroup.alpha = 0;
        }

        public void Show()
        {
            if (!isShown)
            {
                if (!isShowing)
                {
                    StartCoroutine(ShowCoroutine());
                }
            }
        }
        public void Hide()
        {
            if (isShown)
            {
                if (!isHiding)
                {
                    StartCoroutine(HideCoroutine());
                }
            }
        }

        IEnumerator ShowCoroutine()
        {
            isShowing = true;
            yield return StartCoroutine(textBox.Show(showTime));
            //yield return StartCoroutine(portrait.Show(showTime));
            isShowing = false;
            isShown = true;
        }
        IEnumerator HideCoroutine()
        {
            isHiding = true;
            //yield return StartCoroutine(portrait.Hide(hideTime));
            yield return StartCoroutine(textBox.Hide(hideTime));
            isHiding = false;
            isShown = false;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DialogManager))]
    class DialogManagerEditor : Editor
    {
        DialogManager dialogMan { get { return target as DialogManager; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying)
            {
                EditorExtensionMethods.DrawSeparator(Color.gray);
                if (GUILayout.Button("Show"))
                {
                    dialogMan.Show();
                }
                if (GUILayout.Button("Hide"))
                {
                    dialogMan.Hide();
                }
            }
        }
    }
#endif
}