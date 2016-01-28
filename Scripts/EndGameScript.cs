using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameScript : MonoBehaviour 
{
    public Text winFailText;
    public string winner = "SYSTEM CRACKED\nYOU WIN!";
    public string loser = "CONNECTION LOST\nYOU LOSE!";
    private CanvasGroup panelGroup;

    void TogglePanel()
    {
        if (panelGroup.alpha == 1)
        {
            panelGroup.alpha = 0;
            panelGroup.interactable = false;
            panelGroup.blocksRaycasts = false;
            panelGroup.ignoreParentGroups = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            panelGroup.alpha = 1;
            panelGroup.interactable = true;
            panelGroup.blocksRaycasts = true;
            panelGroup.ignoreParentGroups = true;
            Time.timeScale = 0.0f;
        }
    }

    public void ToggleEndGamePanel(bool win)
    {
        TogglePanel();
        if (win)
            winFailText.text = winner;
        else
            winFailText.text = loser;
    }

    public void LoadMenu()
    {
        Application.LoadLevel(0);
    }

    public void ReplayLevel()
    {
        Application.LoadLevel(1);
    }

    void Awake()
    {
        panelGroup = this.GetComponent<CanvasGroup>();
        TogglePanel();
    }

}
