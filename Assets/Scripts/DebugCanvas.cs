using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DebugCanvas : MonoBehaviour
{
    public TextMeshProUGUI DebugTextGrounded;
    public GameObject Player;
    public TextMeshProUGUI fpsText;
    public float deltaTime;

    private void Update()
    {/*
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").gameObject;
        }
        else
        {
            DebugTextGrounded.text = "isGrounded: " + Player.GetComponent<PlayerController>().isGrounded.ToString();

            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString() + "FPS";
        }
        */
    }
}
