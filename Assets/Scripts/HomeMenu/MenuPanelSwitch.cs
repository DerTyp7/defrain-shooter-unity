using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelSwitch : MonoBehaviour
{
    [SerializeField] private GameObject MainPanel, JoinPanel, HostPanel, OptionsPanel;

    [SerializeField] private Button JoinBtn, HostBtn, OptionsBtn;
    private Button BackBtn;

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
        JoinPanel.SetActive(true);
    }

    public void SwitchHostPanel()
    {
        TurnAllOff();
        HostPanel.SetActive(true);
    }

    public void SwitchOptionsPanel()
    {
        TurnAllOff();
        OptionsPanel.SetActive(true);
    }

}
