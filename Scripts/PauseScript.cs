using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour {

	public GameObject pausePanel;

	void TogglePanel()
	{
		CanvasGroup panelGroup = pausePanel.GetComponent<CanvasGroup>();
		if(panelGroup.alpha == 1)
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

	public void UnpauseGame()
	{
		TogglePanel();
	}

	public void MainMenu()
	{
		Application.LoadLevel(0);
	}

	void Awake()
	{
		TogglePanel();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown("escape"))
		{
			TogglePanel();
		}
	}
}
