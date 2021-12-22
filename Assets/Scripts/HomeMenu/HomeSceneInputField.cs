using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HomeSceneInputField : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] float markerHeight = 20.0f;
    [SerializeField] float transitionStartTime = 0.2f;
    [SerializeField] float transitionEndTime = 0.2f;

    [SerializeField] Color helpTextOriginColor;
    [SerializeField] Color helpTextColor;
    [SerializeField] Color focusTextOriginColor;
    [SerializeField] Color focusTextColor;

    [SerializeField] float helpTextEndPosition = -25f;
    [SerializeField] float focusTextEndPosition = 20f;

    [Header("GameObjects")]
    [SerializeField] GameObject marker;
    [SerializeField] GameObject focusText;
    [SerializeField] GameObject helpText;
    [SerializeField] GameObject underline;
    [SerializeField] GameObject placeholder;

    TMP_InputField inputField;

    bool isFocused = false;
    bool isActive = false;
    bool transitionCycleStarted = false;

    float transitionStartTimePassed = 0;
    float transitionEndTimePassed = 0;
    

    void Start()
    {
        inputField = gameObject.GetComponent<TMP_InputField>();
        InputValidator validator = gameObject.GetComponent<InputValidator>();

        if (gameObject.GetComponent<InputValidator>() != null)
        {
            helpText.GetComponent<TMP_Text>().text = gameObject.GetComponent<InputValidator>().GetHelpText();
            focusText.GetComponent<TMP_Text>().text = "Enter " + validator.GetValidatorTypeName();
            placeholder.GetComponent<TMP_Text>().text = "Enter " + validator.GetValidatorTypeName() + "...";

            if(validator.InputType == InputValidator.TypeOfInput.Port) // Because PORT has a default value and so there is text in the inputfield by start
            {
                isActive = true;
                focusText.GetComponent<TMP_Text>().color = focusTextColor;

                RectTransform focusTextRectTranform = focusText.GetComponent<RectTransform>();
                focusTextRectTranform.anchoredPosition = new Vector2(focusTextRectTranform.anchoredPosition.x, focusTextEndPosition);

            }
        }
    }

    void Update()
    {
        //UnFocus && isActive
        if (!inputField.isFocused)
        {
            isFocused = false;

            if (inputField.text != "")
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }

        #region transition
        if (!transitionCycleStarted && isFocused) // Start Transition
        {
            StartTransition();
        }
        else if(transitionCycleStarted && !isFocused) // End Transition
        {
            EndTransition();
        }
        else if(!transitionCycleStarted && !isFocused) // Start cancelled by user
        {

            transitionStartTimePassed = 0;
            transitionEndTimePassed = 0;

            RectTransform markerRectTranform = marker.GetComponent<RectTransform>();
            markerRectTranform.sizeDelta = new Vector2(markerRectTranform.rect.width, 0);

            helpText.GetComponent<TMP_Text>().color = helpTextOriginColor;

            if (!isActive)
                focusText.GetComponent<TMP_Text>().color = focusTextOriginColor;

        }
        #endregion
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isFocused)
        {
            isFocused = true;
            transitionCycleStarted = false;
        }
    }

    #region transitions
    void StartTransition()
    {
        transitionStartTimePassed += Time.deltaTime;
        RectTransform markerRectTranform = marker.GetComponent<RectTransform>();
        markerRectTranform.sizeDelta = new Vector2(markerRectTranform.rect.width, Mathf.Lerp(0, markerHeight, transitionStartTimePassed / transitionStartTime));

        //Help Text / Focus text
        helpText.GetComponent<TMP_Text>().color = Color.Lerp(helpTextOriginColor, helpTextColor, transitionStartTimePassed / transitionStartTime);
        if (!isActive)
            focusText.GetComponent<TMP_Text>().color = Color.Lerp(focusTextOriginColor, focusTextColor, transitionStartTimePassed / transitionStartTime);

        RectTransform helpTextRectTranform = helpText.GetComponent<RectTransform>();
        helpTextRectTranform.anchoredPosition = new Vector2(helpTextRectTranform.anchoredPosition.x, Mathf.Lerp(helpTextEndPosition / 2, helpTextEndPosition, transitionStartTimePassed / transitionStartTime));

        RectTransform focusTextRectTranform = focusText.GetComponent<RectTransform>();

        if (!isActive)
            focusTextRectTranform.anchoredPosition = new Vector2(focusTextRectTranform.anchoredPosition.x, Mathf.Lerp(focusTextEndPosition / 2, focusTextEndPosition, transitionStartTimePassed / transitionStartTime));


        if (transitionStartTimePassed >= transitionStartTime)
        {
            transitionCycleStarted = true;
            transitionStartTimePassed = 0;
        }
    }

    void EndTransition()
    {
        transitionEndTimePassed += Time.deltaTime;
        RectTransform rectTranform = marker.GetComponent<RectTransform>();
        rectTranform.sizeDelta = new Vector2(rectTranform.rect.width, Mathf.Lerp(markerHeight, 0, transitionEndTimePassed / transitionEndTime));


        //Help Text / Focus text
        helpText.GetComponent<TMP_Text>().color = Color.Lerp(focusTextColor, helpTextOriginColor, transitionEndTimePassed / transitionEndTime);

        if(!isActive)
            focusText.GetComponent<TMP_Text>().color = Color.Lerp(focusTextColor, focusTextOriginColor, transitionEndTimePassed / transitionEndTime);

        
        RectTransform helpTextRectTranform = helpText.GetComponent<RectTransform>();
        helpTextRectTranform.anchoredPosition = new Vector2(helpTextRectTranform.anchoredPosition.x, Mathf.Lerp(helpTextEndPosition, helpTextEndPosition / 2, transitionEndTimePassed / transitionEndTime));

        if (!isActive)
        {
            RectTransform focusTextRectTranform = focusText.GetComponent<RectTransform>();
            focusTextRectTranform.anchoredPosition = new Vector2(focusTextRectTranform.anchoredPosition.x, Mathf.Lerp(focusTextEndPosition, focusTextEndPosition / 2, transitionEndTimePassed / transitionEndTime));
        }

        if (transitionEndTimePassed >= transitionEndTime)
        {
            transitionCycleStarted = false;
            transitionEndTimePassed = 0;
        }
    }
    #endregion
    
}
