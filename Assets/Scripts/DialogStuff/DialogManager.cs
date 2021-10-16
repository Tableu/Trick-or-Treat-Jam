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
        [SerializeField]
        internal DialogContainerSO debugDialogToWrite;
        //Add protrait thing.
        DialogBox dialogBox;

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
            dialogBox = transform.Find("DialogBox").GetComponent<DialogBox>();

            dialogBox.Init();

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
            yield return StartCoroutine(dialogBox.Show(showTime));
            //yield return StartCoroutine(portrait.Show(showTime));
            isShowing = false;
            isShown = true;
        }
        IEnumerator HideCoroutine()
        {
            isHiding = true;
            //yield return StartCoroutine(portrait.Hide(hideTime));
            yield return StartCoroutine(dialogBox.Hide(hideTime));
            isHiding = false;
            isShown = false;
        }

        public void WriteMessage(DialogContainerSO toWrite)
        {
            if (!dialogBox.isWriting)
            {
                ClearText();
                if (toWrite == null)
                {
                    //Debugging purposes, when called through the editor without a dialog.
                    toWrite = new DialogContainerSO();
                    toWrite.SetData("TestMessage", "TestMessage lmao <TestTag>This message has tag</TestTag>", 10f);
                }

                if (!string.IsNullOrEmpty(toWrite.Message))
                {
                    StartCoroutine(dialogBox.WriteDialog(toWrite.Message, toWrite.CharsPerSecond));
                }
            }
        }
        public void ClearText()
        {
            dialogBox.ClearText();
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
                EditorExtensionMethods.DrawSeparator(Color.gray);
                if (GUILayout.Button("Write default message"))
                {
                    dialogMan.WriteMessage(dialogMan.debugDialogToWrite);
                }
                if (GUILayout.Button("Clear thing"))
                {
                    dialogMan.ClearText();
                }
            }
        }
    }
#endif
}