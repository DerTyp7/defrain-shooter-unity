using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class HomeSceneBtnScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Color onHoverColor;
    [SerializeField] Color onPressedColor;

    Color originBgColor;
    Image bgImage;
    GameObject marker;

    bool isHovering = false;

    //Color Lerp
    [SerializeField] float hoverBgTransitionTime = 0.4f;
    [SerializeField] float exitBgTransitionTime = 0.2f;

    //Marker Lerp
    [SerializeField] float markerHeight = 20.0f;
    [SerializeField] float hoverMarkerTransitionTime = 0.1f;
    [SerializeField] float exitMarkerTransitionTime = 0.1f;

    float hoveringTimePassed = 0.0f;
    float exitingTimePassed = 0.0f;

    void Start()
    {
        marker = gameObject.transform.GetChild(1).gameObject;
        bgImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        originBgColor = bgImage.color;
    }

    void Update()
    {
        
        if (isHovering)
        {
            exitingTimePassed = 0.0f;
            hoveringTimePassed += Time.deltaTime;

            //Color
            bgImage.color = Color.Lerp(originBgColor, onHoverColor, hoveringTimePassed / hoverBgTransitionTime);

            //Marker
            RectTransform rectTranform = marker.GetComponent<RectTransform>();
            rectTranform.sizeDelta = new Vector2(rectTranform.rect.width, Mathf.Lerp(0, markerHeight, hoveringTimePassed / hoverMarkerTransitionTime));
        }
        else
        {
            hoveringTimePassed = 0.0f;
            exitingTimePassed += Time.deltaTime;
            
            //Color
            bgImage.color = Color.Lerp(onHoverColor, originBgColor, exitingTimePassed / exitBgTransitionTime);

            //Marker
            RectTransform rectTranform = marker.GetComponent<RectTransform>();
            rectTranform.sizeDelta = new Vector2(rectTranform.rect.width, Mathf.Lerp(markerHeight, 0, exitingTimePassed / exitMarkerTransitionTime));

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
