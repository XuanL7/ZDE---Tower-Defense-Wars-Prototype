using UnityEngine;
using System.Collections;

public class ToggleUI : MonoBehaviour {

    public GameObject towerPanel;
    public GameObject towerUpgradePanel;
    public GameObject packetInterface;
    public GameObject bottomLeftPanel;

    private WaveController waveCon;

    private void OnEnable()
    {
        if(waveCon.menusOn)
        {
            if (towerPanel.gameObject != null)
                towerPanel.GetComponent<DisablePanelScript>().TogglePanel(true);
            if (towerUpgradePanel.gameObject != null)
                towerUpgradePanel.GetComponent<DisablePanelScript>().TogglePanel(true);
            if (packetInterface.gameObject != null)
                packetInterface.GetComponent<DisablePanelScript>().TogglePanel(false);
            if (bottomLeftPanel.gameObject != null)
                bottomLeftPanel.GetComponent<DisablePanelScript>().TogglePanel(false);
        }
    }

    private void OnDisable()
    {
       if (waveCon.menusOn)
       {
            if (towerPanel.gameObject != null)
                towerPanel.GetComponent<DisablePanelScript>().TogglePanel(false);
            if (towerUpgradePanel.gameObject != null)
                towerUpgradePanel.GetComponent<DisablePanelScript>().TogglePanel(false);
            if (packetInterface.gameObject != null)
                packetInterface.GetComponent<DisablePanelScript>().TogglePanel(true);
            if (bottomLeftPanel.gameObject != null)
                bottomLeftPanel.GetComponent<DisablePanelScript>().TogglePanel(true);
       }
    }

    void Awake()
    {
        if ((waveCon = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<WaveController>()) == null)
            Debug.Log("Error: ToggleUI could not find WaveController!");
    }
}
