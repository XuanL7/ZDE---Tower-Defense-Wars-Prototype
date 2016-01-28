using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour 
{
    public GameObject menuPanel;
    public GameObject creditsPanel;

    public void StartGame()
    {
        Application.LoadLevel(1);
    }

    public void ToggleCredits()
    {
        if (menuPanel.activeSelf)
        {
            menuPanel.SetActive(false);
            creditsPanel.SetActive(true);
        }
        else
        {
            menuPanel.SetActive(true);
            creditsPanel.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
