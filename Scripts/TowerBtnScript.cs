using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerBtnScript : MonoBehaviour {

	public GameObject towerType;

	public int GetTowerCost()
	{
		return towerType.GetComponent<TowerController>().cost;
	}

	void Awake()
	{
		if((towerType.GetComponent<TowerController>().Name = this.GetComponentInChildren<Text>().text) == null)
			Debug.Log("Error: Failed to load towerController.Name with button.Text component value!");
	}
}
