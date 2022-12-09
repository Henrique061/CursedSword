using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuLogin : MonoBehaviour
{
    [SerializeField] GameObject loginWithEmailPanel;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject disconnectPanel;
    [SerializeField] TextMeshProUGUI textConnected;

    public void ShowPanelLoginWithEmail()
    {
        loginWithEmailPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void ReturnFromLoginWithEmail()
    {
        loginWithEmailPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void EnableDisconnectPanel(string message)
    {
        disconnectPanel.SetActive(true);
        loginPanel.SetActive(false);
        loginWithEmailPanel.SetActive(false);
        textConnected.text = message;
    }

    public void DisableDisconnectPanel()
    {
        disconnectPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
}
