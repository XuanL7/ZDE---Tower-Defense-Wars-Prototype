using UnityEngine;
using System.Collections;

public class DisablePanelScript : MonoBehaviour {

	public void TogglePanel(bool toggle)
	{
		if (toggle)
		{
			this.GetComponent<CanvasGroup>().alpha = 1;
			this.GetComponent<CanvasGroup>().interactable = true;
			this.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			this.GetComponent<CanvasGroup>().alpha = 0;
			this.GetComponent<CanvasGroup>().interactable = false;
			this.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}
}
