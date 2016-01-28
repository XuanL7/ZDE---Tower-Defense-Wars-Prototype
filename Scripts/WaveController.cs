using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveController : MonoBehaviour {

	/** PUBLIC MEMBERS **/

	public GameObject leftPanel;
	public GameObject bottomLeftPanel;
	public GameObject rightPanel;
    public GameObject bottomRightPanel;

	public int waveDuration = 30;
	public int firstWaveTime = 60;
	public int endTime = 60; // time till end of next wave
	public bool mixedSpawn = false; //Toggle mixing of spawn packets
	[HideInInspector]
	public int waveTimer = 0;
	[HideInInspector]
	public int currentWave = 1;
	[HideInInspector]
	public bool menusOn = true;

	/** PRIVATE EMEMBERS **/
	PacketManager packMan;
	ComputerAI aiComp;
	private bool isSpawning = false;
	private bool packetsMoving = false;

	/** PRIVATE UTILITY METHODS **/
	private void WaveTime()
	{
		menusOn = true;
		++waveTimer;
		if (waveTimer >= endTime)
		{
            TogglePanels(false);
			CancelInvoke("WaveTime");
			waveTimer = 0;
			endTime = waveDuration;
			isSpawning = true;
			menusOn = false;
		}
	}

    private void TogglePanels(bool toggle)
    {
        bottomLeftPanel.GetComponent<DisablePanelScript>().TogglePanel(toggle);
        bottomRightPanel.GetComponent<DisablePanelScript>().TogglePanel(toggle);
        leftPanel.GetComponent<DisablePanelScript>().TogglePanel(toggle);
        rightPanel.GetComponent<DisablePanelScript>().TogglePanel(toggle);
    }

	/** STANDARD UNITY METHODS **/

	void Awake()
	{
		if ((packMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<PacketManager>()) == null)
			Debug.Log("Error: Could not find PAcketManager in Game Logic for WaveController");
		if ((aiComp = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<ComputerAI>()) == null)
			Debug.Log("Error: Could not find ComputerAI in Game Logic for WaveController");
		//if ((camCon = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<CameraController>()) == null)
			//Debug.Log("Error: Could not find CameraController in Game Logic for WaveController");
	}

	void Start()
	{
		aiComp.enactNextBehavior(currentWave);
		endTime = firstWaveTime;
		InvokeRepeating("WaveTime", 1, 1);
	}

	void Update()
	{
		if (isSpawning)
		{
			if (mixedSpawn)
				packMan.SpawnMixedWave();
			else
				packMan.SpawnNormalWave();

			isSpawning = false;
			packetsMoving = true;
		}

		if (packetsMoving)
		{
			if (packMan.ArePacketsCompleteWave())
			{
				packetsMoving = false;
				++currentWave;
				InvokeRepeating("WaveTime", 1, 1);
				aiComp.enactNextBehavior(currentWave);
                TogglePanels(true);
			}
		}
	}
}
