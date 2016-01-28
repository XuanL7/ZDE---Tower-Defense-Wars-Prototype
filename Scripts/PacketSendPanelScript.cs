using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PacketSendPanelScript : MonoBehaviour 
{
    /** PRIVATE MEMBERS **/
    private string player = "Player";
    private string identity;
    private UpgradeManager upMan;
    private Button btn;

    public void sendPacket()
    {
        upMan.sendPacket(identity, player);
    }

    public void ToggleButton()
    {
        if (upMan.CanSendPacket(identity, player))
            btn.interactable = true;
        else
            btn.interactable = false;
    }

    void Awake()
    {
        if ((upMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<UpgradeManager>()) == null)
            Debug.Log("Error: Could not find UpgradeManager in Game Logic for PacketSendPanel");

        if ((btn = this.GetComponent<Button>()) == null)
            Debug.Log("Error: Failed to get Button component!");

        if ((identity = btn.GetComponentInChildren<Text>().text) == null)
            Debug.Log("Error: Missing Text component in child of this");
    }

    void Update()
    {
        ToggleButton();
    }
}
