using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedScript : MonoBehaviour {

	public GameObject arrowPanel;
    public float speed = 2;

	public void ToggleSpeed()
	{
		if (Time.timeScale == 1.0)
		{
			Time.timeScale = speed;
			Color newColor = arrowPanel.GetComponent<Image>().color;
			newColor.a = 1;
			arrowPanel.GetComponent<Image>().color = newColor;
		}
		else
		{
			Time.timeScale = 1.0f;
			Color newColor = arrowPanel.GetComponent<Image>().color;
			newColor.a = 0;
			arrowPanel.GetComponent<Image>().color = newColor;
		}
	}

	void Awake()
	{
		Color newColor = arrowPanel.GetComponent<Image>().color;
			newColor.a = 0;
			arrowPanel.GetComponent<Image>().color = newColor;
	}
}
