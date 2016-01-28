using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MixSpawnButtonScript : MonoBehaviour {

	private WaveController waveCon;

	public void ToggleSpawnType()
	{
		if (waveCon.mixedSpawn)
		{
			waveCon.mixedSpawn = false;
			this.GetComponentInChildren<Text>().text = "Normal";
		}
		else
		{
			waveCon.mixedSpawn = true;
			this.GetComponentInChildren<Text>().text = "Mixed";
		}
	}

	void Awake () {
		if ((waveCon = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<WaveController>()) == null)
			Debug.Log("Error: Could not find WaveController in Game Logic for MixSpawnButton!");
		this.GetComponentInChildren<Text>().text = "Normal";
	}	
}
