using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HomeSceneBackBtnScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject arrow1;
    [SerializeField] GameObject arrow2;

    [SerializeField] float hoverTransitionTime = 0.1f;
    [SerializeField] float exitTransitionTime = 0.1f;

    float hoveringTimePassed = 0.0f;
    float exitingTimePassed = 0.0f;

    bool isHovering = false;

    void Update()
    {
        RectTransform arrow1RectTransform = arrow1.GetComponent<RectTransform>();
        RectTransform arrow2RectTransform = arrow2.GetComponent<RectTransform>();

        if (isHovering)
        {
            exitingTimePassed = 0.0f;
            hoveringTimePassed += Time.deltaTime;

            arrow1.GetComponent<Image>().color = Color.Lerp(new Color(255,255,255,0), Color.white, hoveringTimePassed / hoverTransitionTime);
            arrow2.GetComponent<Image>().color = Color.Lerp(new Color(255, 255, 255, 0), Color.white, hoveringTimePassed / hoverTransitionTime);

            arrow1RectTransform.anchoredPosition = new Vector2(Mathf.Lerp(0, 15, hoveringTimePassed / hoverTransitionTime), arrow1RectTransform.anchoredPosition.y);
            arrow2RectTransform.anchoredPosition = new Vector2(Mathf.Lerp(10, 30, hoveringTimePassed / hoverTransitionTime), arrow2RectTransform.anchoredPosition.y);


        }
        else
        {
            hoveringTimePassed = 0.0f;
            exitingTimePassed += Time.deltaTime;

            arrow1.GetComponent<Image>().color = Color.Lerp(Color.white, new Color(255, 255, 255, 0), exitingTimePassed / exitTransitionTime);
            arrow2.GetComponent<Image>().color = Color.Lerp(Color.white, new Color(255, 255, 255, 0), exitingTimePassed / exitTransitionTime);

            arrow1RectTransform.anchoredPosition = new Vector2(Mathf.Lerp(15, 0, exitingTimePassed / exitTransitionTime), arrow1RectTransform.anchoredPosition.y);
            arrow2RectTransform.anchoredPosition = new Vector2(Mathf.Lerp(30, 10, exitingTimePassed / exitTransitionTime), arrow2RectTransform.anchoredPosition.y);


        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isHovering = false;
    }
}
