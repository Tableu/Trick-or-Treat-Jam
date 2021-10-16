using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MementoMori.Dialog
{
    public class DialogButtonController : MonoBehaviour
    {
        [SerializeField]
        bool isActive;

        [SerializeField, Header("Blinking things"), Range(0, 1f)]
        float minAlpha = 0.2f;
        [SerializeField, Range(0, 1f)]
        float maxAlpha = 0.8f;
        [SerializeField, Range(0.1f, 2f)]
        float timeBetweenBlinkStates = 0.2f;

        [SerializeField]
        CanvasGroup canvasGroup;
        Button button;

        DialogBox dialogBox;

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                if (isActive)
                {
                    StartCoroutine(BlinkCoroutine());
                }
                else
                {
                    StopAllCoroutines();
                }
                canvasGroup.alpha = isActive ? minAlpha : 0;
                button.interactable = isActive;
                //canvasGroup.blocksRaycasts = isActive;
            }
        }

        public void Init(DialogBox box)
        {
            dialogBox = box;

            canvasGroup = transform.Find("Visuals").GetComponent<CanvasGroup>();
            button = GetComponent<Button>();

            IsActive = false;
        }

        IEnumerator BlinkCoroutine()
        {
            bool isShowing = true;
            float currentTime = 0;
            float currentProgress = 0;
            float alphaDifference = maxAlpha - minAlpha;
            while (isActive)
            {
                currentProgress = currentTime / timeBetweenBlinkStates;

                canvasGroup.alpha = isShowing ? minAlpha + (alphaDifference * currentProgress) : maxAlpha - (alphaDifference * currentProgress);

                Debug.Log($"Ayy alpha: {canvasGroup.alpha}, Prog: {currentProgress}");
                yield return null;
                currentTime += Time.deltaTime;
                if (currentTime > timeBetweenBlinkStates)
                {
                    isShowing = !isShowing;
                    currentTime = 0;
                    currentProgress = 0;
                }
            }
        }

        public void Click()
        {
            dialogBox.ReceiveBoxClick();
        }
    }
}