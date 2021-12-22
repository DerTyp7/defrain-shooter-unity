using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HomeSceneBtnBigScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject background;
    [SerializeField] float hoverTransitionTime = 0.1f;
    [SerializeField] float exitTransitionTime = 0.1f;
    [SerializeField] Color backgroundColor;

    float hoveringTimePassed = 0.0f;
    float exitingTimePassed = 0.0f;

    bool isHovering = false;
    bool preventHoverOnStartUp = true;

    void Update()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        RectTransform backgroundRectTranform = background.GetComponent<RectTransform>();

        background.GetComponent<Image>().color = backgroundColor;

        if (isHovering)
        {
            exitingTimePassed = 0.0f;
            hoveringTimePassed += Time.deltaTime;

                
            backgroundRectTranform.sizeDelta = new Vector2(Mathf.Lerp(0, rectTransform.rect.width, hoveringTimePassed / hoverTransitionTime), rectTransform.rect.height);

            preventHoverOnStartUp = false;
        }
        else if(!preventHoverOnStartUp)
        {
            hoveringTimePassed = 0.0f;
            exitingTimePassed += Time.deltaTime;

            backgroundRectTranform.sizeDelta = new Vector2(Mathf.Lerp(rectTransform.rect.width, 0, exitingTimePassed / exitTransitionTime), rectTransform.rect.height);
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
