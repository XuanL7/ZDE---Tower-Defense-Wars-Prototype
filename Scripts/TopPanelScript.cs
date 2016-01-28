using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopPanelScript : MonoBehaviour {

	public Text currentWaveValue;
	public Text towerValue;
	public Text packetValue;
	public Text energyValue;
	public Text waveValue;
	public Text energyRateCost;
	public Text energyTimeCost;
	public Text player_HP_Text;
	public Text ai_HP_Text;

	public Button energyRatButton;
	public Button energyTimeButton;

	private UpgradeManager upMan;
	private WaveController waveCon;
	private ComputerAI compAI;

	// Use this for initialization
	void Awake () {
		if ((upMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<UpgradeManager>()) == null)
			Debug.Log("Error: TopPanelScript could not find UpgradeManagr in Game Logic");
		if ((waveCon = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<WaveController>()) == null)
			Debug.Log("Error: TopPanelScript could not find WaveController in Game Logic");
		if ((compAI = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<ComputerAI>()) == null)
			Debug.Log("Error: TopPanelScript could not find ComputerAI in Game Logic");
	}
	
	// Update is called once per frame
	void Update () {

		if (upMan.econ.energyTimeCost <= upMan.econ.towerCash)
			energyTimeButton.interactable = true;
		else
			energyTimeButton.interactable = false;

		if (upMan.econ.energyRateCost <= upMan.econ.towerCash)
			energyRatButton.interactable = true;
		else
			energyRatButton.interactable = false;

		towerValue.text = upMan.econ.towerCash.ToString();
		packetValue.text = upMan.econ.packetCash.ToString();
		energyValue.text = upMan.econ.energy.ToString();
		waveValue.text = waveCon.waveTimer.ToString() + " / " + waveCon.endTime;
		energyRateCost.text = upMan.econ.energyRateCost.ToString();
		energyTimeCost.text = upMan.econ.energyTimeCost.ToString();
		currentWaveValue.text = waveCon.currentWave.ToString();
		player_HP_Text.text = upMan.playerStats.health.ToString();
		ai_HP_Text.text = compAI.aiStats.health.ToString();
	}
}
