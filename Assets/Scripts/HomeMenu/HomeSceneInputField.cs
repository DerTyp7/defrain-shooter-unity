using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HomeSceneInputField : MonoBehaviour, IPointerClickHandler
{
    GameObject marker;
    GameObject focusText;
    GameObject helpText;
    GameObject underline;

    bool isFocused = false;
    bool transitionStartDone = false;
    bool transitionEndDone = false;

    void Start()
    {
        marker = gameObject.transform.Find("Marker").gameObject;
        focusText = gameObject.transform.Find("FocusText").gameObject;
        helpText = gameObject.transform.Find("HelpText").gameObject;
        underline = gameObject.transform.Find("Underline").gameObject;
        
    }

    void Update()
    {
        if (!gameObject.GetComponent<TMP_InputField>().isFocused)
        {
            isFocused = false;

            if (transitionEndDone)
            {
                transitionEndDone = false;
            }
        }


        if (!transitionStartDone)
        {
            marker.SetActive(true);
            focusText.SetActive(true);
            helpText.SetActive(true);
        }
        else
        {
            marker.SetActive(false);
            focusText.SetActive(false);
            helpText.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isFocused = true;
        transitionStartDone = false;
    }
}
