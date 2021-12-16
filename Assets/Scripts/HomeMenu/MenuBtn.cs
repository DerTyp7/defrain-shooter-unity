using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    [Header("Background Image")]
    [SerializeField] private GameObject BtnBgImage;
    [SerializeField] private Color BgColor;
    [SerializeField] private Color OnHoverBgColor;
    
    private bool mouse_over = false;


    public void Update()
    {
        if (mouse_over)
        {
            BtnBgImage.GetComponent<Image>().color = OnHoverBgColor;
        }
        else
        {
            BtnBgImage.GetComponent<Image>().color = BgColor;
        }
    }


    public void OnPointerEnter(PointerEventData e)
    {
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData e)
    {
        mouse_over = false;
    }
}
