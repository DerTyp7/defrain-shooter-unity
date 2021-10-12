using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DebugCanvas : MonoBehaviour
{
    public TextMeshProUGUI DebugTextGrounded;
    public GameObject Player;

    private void Update()
    {
        DebugTextGrounded.text = "isGrounded: " + Player.GetComponent<PlayerController>().isGrounded.ToString();
    }
}
