using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class PlayerUIController : NetworkBehaviour
{
    [SerializeField] Canvas playerUICanvas;
    [SerializeField] Image damageImage;
    [SerializeField] TMP_Text healthText;
    float hitVal = 0;
    private void Start()
    {
        if (isLocalPlayer)
        {
            playerUICanvas.enabled = true;
        }
    }
    /*
    void Start()
    {
        GameObject imgObject = new GameObject("testAAA");

        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(playerUICanvas.transform); // setting parent
        trans.localScale = Vector3.one;
        trans.anchoredPosition = new Vector2(0f, 0f); // setting position, will be on center
        trans.sizeDelta = new Vector2(150, 200); // custom size

        Image image = imgObject.AddComponent<Image>();
        Texture2D tex = Resources.Load<Texture2D>("red");
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        imgObject.transform.SetParent(playerUICanvas.transform);
    }*/

    // Update is called once per frame
    void FixedUpdate()
    {
        hitVal = gravityValue(hitVal,0.01f,0.01f,1,0,false);
        damageImage.GetComponent<CanvasRenderer>().SetAlpha(hitVal);
        healthText.text = GetComponent<Player>().health.ToString();
    }

    public void showHit() 
    {
        hitVal = 1;
    }


    float gravityValue(float curretnValue, float rateOfChangePos, float rateOfChangeNeg, float maxValue, float minValue, bool add)
    {
        // The currentValue will be advanced by the rateOfChangePos and reduced by the rateOfChangeNeg depending on the add boolean. But only in the specified range.
        // Usage: val = gravityValue(val, 0.01f, 0.05f, 1, 0, true);
        float value = curretnValue;
        if (add) value += rateOfChangePos;
        else value -= rateOfChangeNeg;

        return Mathf.Clamp(value, minValue, maxValue);
    }
}
