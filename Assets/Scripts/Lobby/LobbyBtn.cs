using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbyBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject marker;

    //Marker Lerp
    [SerializeField] float markerWidth = 40.0f;
    [SerializeField] float hoverMarkerTransitionTime = 0.1f;
    [SerializeField] float exitMarkerTransitionTime = 0.1f;

    float hoveringTimePassed = 0.0f;
    float exitingTimePassed = 0.0f;


    bool isHovering = false;

    void Update()
    {
        if (isHovering)
        {
            exitingTimePassed = 0.0f;
            hoveringTimePassed += Time.deltaTime;

            //Marker
            RectTransform rectTranform = marker.GetComponent<RectTransform>();
            rectTranform.sizeDelta = new Vector2(Mathf.Lerp(0, markerWidth, hoveringTimePassed / hoverMarkerTransitionTime), rectTranform.rect.height);
        }
        else
        {
            hoveringTimePassed = 0.0f;
            exitingTimePassed += Time.deltaTime;

            //Marker
            RectTransform rectTranform = marker.GetComponent<RectTransform>();
            rectTranform.sizeDelta = new Vector2(Mathf.Lerp(markerWidth, 0, exitingTimePassed / exitMarkerTransitionTime), rectTranform.rect.height);

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
