using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MementoMori.Dialog
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField]
        string toWrite = "";

        TextMeshProUGUI text;
        CanvasGroup canvasGroup;

        [SerializeField]
        bool isShown = false;
        [SerializeField]
        bool isShowing = false;
        [SerializeField]
        bool isHiding = false;

        private void Awake()
        {
            Init();
        }

        public void Reset()
        {
            isShown = false;
            isShowing = false;
            isHiding = false;
            canvasGroup.alpha = 0;
        }

        public void Init()
        {
            text = transform.Find("DialogText").GetComponent<TextMeshProUGUI>();
            canvasGroup = GetComponent<CanvasGroup>();

            Reset();
        }

        internal void WriteDebugText()
        {
            text.text = toWrite;
        }
        internal void ClearText()
        {
            text.text = "";
        }
        public IEnumerator Show(float timeToShow)
        {
            isShowing = true;

            float currentTime = 0;
            float currentProgress = 0;
            while (currentProgress < 1f)
            {
                currentProgress = Mathf.Clamp(currentTime / timeToShow, 0f, 1f);
                canvasGroup.alpha = currentProgress;
                yield return null;
                currentTime += Time.deltaTime;
            }
            canvasGroup.alpha = 1;
            isShowing = false;
            isShown = true;
        }
        public IEnumerator Hide(float timeToHide)
        {
            isHiding = true;

            float currentTime = 0;
            float currentProgress = 0;
            while (currentProgress < 1f)
            {
                currentProgress = Mathf.Clamp(currentTime / timeToHide, 0f, 1f);
                canvasGroup.alpha = 1f - currentProgress;
                yield return null;
                currentTime += Time.deltaTime;
            }
            canvasGroup.alpha = 0;
            isHiding = false;
            isShown = false;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DialogBox))]
    class DialogBoxEditor : Editor
    {
        DialogBox box { get { return target as DialogBox; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying)
            {
                EditorExtensionMethods.DrawSeparator(Color.gray);
                if (GUILayout.Button("Show"))
                {
                    box.StartCoroutine(box.Show(0.5f));
                }
                if (GUILayout.Button("Hide"))
                {
                    box.StartCoroutine(box.Hide(0.5f));
                }
                EditorExtensionMethods.DrawSeparator(Color.gray);
                if (GUILayout.Button("Write thing"))
                {
                    box.WriteDebugText();
                }
                if (GUILayout.Button("Clear thing"))
                {
                    box.ClearText();
                }
            }
        }
    }
#endif
}