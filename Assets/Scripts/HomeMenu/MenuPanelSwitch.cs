using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelSwitch : MonoBehaviour
{
    [SerializeField] private GameObject MainPanel, JoinPanel, HostPanel, OptionsPanel;

    [SerializeField] private Button JoinBtn, HostBtn, OptionsBtn;

    private void Start()
    {
        ResetToMain();
        JoinBtn.onClick.AddListener(SwitchJoinPanel);
        HostBtn.onClick.AddListener(SwitchHostPanel);
        OptionsBtn.onClick.AddListener(SwitchOptionsPanel);        
    }
    public void ResetToMain()
    {
        JoinPanel.SetActive(false);
        HostPanel.SetActive(false);
        OptionsPanel.SetActive(false);

        SetBackgroundBrightness(1f);
        MainPanel.SetActive(true);
        
    }

    public void TurnAllOff()
    {
        JoinPanel.SetActive(false);
        HostPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        MainPanel.SetActive(false);
        
    }

    public void SwitchJoinPanel()
    {
        TurnAllOff();
        SetBackgroundBrightness(0.5f);
        JoinPanel.SetActive(true);
    }

    public void SwitchHostPanel()
    {
        TurnAllOff();
        SetBackgroundBrightness(0.5f);
        HostPanel.SetActive(true);
    }

    public void SwitchOptionsPanel()
    {
        TurnAllOff();
        SetBackgroundBrightness(0.5f);
        OptionsPanel.SetActive(true);
    }

    void SetBackgroundBrightness(float b) // 0.0 -> 1 
    {
        GetComponent<Image>().color = new Color(b, b, b);
    }
}
