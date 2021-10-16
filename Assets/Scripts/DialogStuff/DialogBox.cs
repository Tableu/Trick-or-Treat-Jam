using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MementoMori.Dialog
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField]
        string toWrite = "";

        //[SerializeField, Header("Text Settings"), Range(0, 10f)]
        //float charsPerSecond = 5f;
        [SerializeField, Tooltip("Don't modify this var, it auto sets on Init()")]
        float baseTextSize;
        [SerializeField, Header("Dynamic vars")]
        float currentSecsPerChar = 0;
        [SerializeField]
        float currentSize = 0;
        [SerializeField]
        bool isBold = false;
        [SerializeField]
        bool isItalic = false;
        [SerializeField]
        bool waitingForClick = false;

        DialogButtonController dialogButton;

        TextMeshProUGUI dialogText;
        CanvasGroup canvasGroup;

        [SerializeField]
        bool isShown = false;
        [SerializeField]
        bool isShowing = false;
        [SerializeField]
        bool isHiding = false;


        [SerializeField]
        public bool isWriting = false;


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
            dialogText = transform.Find("DialogText").GetComponent<TextMeshProUGUI>();
            dialogButton = transform.Find("DialogButton").GetComponent<DialogButtonController>();
            canvasGroup = GetComponent<CanvasGroup>();
            baseTextSize = dialogText.fontSize;

            dialogButton.Init(this);

            Reset();
        }

        internal void WriteDebugText()
        {
            dialogText.text = toWrite;
        }
        internal void ClearText()
        {
            dialogText.text = "";
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

        /// <summary>
        /// Receive Dialog object
        /// [DialogContainer]
        /// -string ID
        /// -string Message
        /// </summary>
        //public void WriteDialog(DialogContainerSO toWrite)
        //{

        //}

        public IEnumerator WriteDialog(string toWrite, float initialCharSpeed, float timeToShow = 0.5f)
        {
            if (!isShown)
            {
                yield return StartCoroutine(Show(timeToShow));
            }

            dialogText.text = "";
            currentSecsPerChar = 1f / initialCharSpeed;
            string[] spaceSplit = Regex.Split(toWrite, @"( )");
            List<string> tagEndSplit = new List<string>();

            foreach (string str in spaceSplit)
            {
                tagEndSplit.AddRange(str.Split('>'));
            }

            List<string> splitToWrite = new List<string>();

            foreach (string str in tagEndSplit)
            {
                splitToWrite.AddRange(Regex.Split(str, @"(?=\<)"));
            }

            isWriting = true;
            for (int i = 0; i < splitToWrite.Count; i++)
            {
                while (waitingForClick)
                {
                    yield return null;
                }
                if (splitToWrite[i] != "")
                {
                    if (splitToWrite[i][0] == '<')
                    {
                        //Handle and write tags
                        HandleTag(splitToWrite[i].Remove(0, 1));
                    }
                    else
                    {
                        yield return StartCoroutine(WriteWord(splitToWrite[i]));
                    }
                }
            }
            yield return StartCoroutine(Hide(timeToShow));
            isWriting = false;

            //Hide?
        }
        IEnumerator WriteWord(string toWrite)
        {
            char[] splitWord = toWrite.ToCharArray();
            float startTime = -1f;
            if (splitWord.Length > 0)
            {
                string currentText = dialogText.text;
                for (int i = 0; i < splitWord.Length; i++)
                {
                    startTime = Time.time;

                    string textToAdd = "";

                    //Handle tags (Do this on handle tags instead)
                    //textToAdd += isBold ? "<b>" : "";

                    for (int i2 = 0; i2 <= i; i2++)
                    {
                        textToAdd += splitWord[i2].ToString();
                    }

                    //Handle closing tags (Also do this in handle tags instead
                    //textToAdd += 

                    dialogText.text = currentText + textToAdd;

                    while (Time.time < startTime + (currentSecsPerChar))
                    {
                        yield return null;
                    }
                }

                ///Shouldn't add a space here since whitespaces are no longer ignored.
                //dialogText.text += " ";
                //startTime = Time.time;
                //while (Time.time < startTime + currentSecsPerChar)
                //{
                //    yield return null;
                //}

            }
        }

        void HandleTag(string toHandle)
        {
            if (!string.IsNullOrEmpty(toHandle))
            {
                bool isOpening = !(toHandle[0] == '/');
                if (!isOpening)
                {
                    toHandle = toHandle.Remove(0, 1);
                }
                Debug.Log($"Yo, gotta handle this tag '{toHandle}', and {(isOpening ? "it's opening" : "it's closing")}");

                if (toHandle.Contains("="))
                {
                    string[] splitTag = toHandle.Split('=');
                    string tag = splitTag[0];
                    string value = splitTag[1];
                    switch (tag)
                    {
                        case "speed":
                            float speed = -1;
                            float.TryParse(value, out speed);
                            if (speed != -1)
                            {
                                currentSecsPerChar = 1f / speed;
                            }
                            break;
                        case "size":
                            float sizeModifier = 1;
                            float.TryParse(value, out sizeModifier);
                            if (sizeModifier > 0)
                            {
                                dialogText.text += $"<size={baseTextSize * sizeModifier}>";
                            }
                            break;
                    }
                }
                else
                {
                    switch (toHandle)
                    {
                        case "click":
                            waitingForClick = true;
                            dialogButton.IsActive = true;
                            //TODO: Enable dialog button
                            break;
                        case "b":
                            if (isBold != isOpening)
                            {
                                dialogText.text += isOpening ? "<b>" : "</b>";
                                isBold = isOpening;
                            }
                            break;
                        case "i":
                            if (isItalic != isOpening)
                            {
                                dialogText.text += isOpening ? "<i>" : "</i>";
                            }
                            break;
                    }
                }
            }
        }
        
        public void ReceiveBoxClick()
        {
            Debug.Log("Clicked!");
            waitingForClick = false;
            dialogButton.IsActive = false;
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
                //if (GUILayout.Button("Write thing"))
                //{
                //    box.WriteDebugText();
                //}
                //if (GUILayout.Button("Clear thing"))
                //{
                //    box.ClearText();
                //}
            }
        }
    }
#endif
}