using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelSendingCountScript : MonoBehaviour {

    public Text pac1Text;
    public Text pac2Text;
    public Text pac3Text;
    public Text pac4Text;
    public Text pac5Text;

    public Text pac1Name;
    public Text pac2Name;
    public Text pac3Name;
    public Text pac4Name;
    public Text pac5Name;

    private PacketManager packMan;

    void UpdateSendingPacketCount()
    {
        pac1Text.text = packMan.GetPacketGroup(pac1Name.text, "Player").sendingCount.ToString() + "/" + packMan.GetPacketGroup(pac1Name.text, "Player").PacketCount().ToString();
        pac2Text.text = packMan.GetPacketGroup(pac2Name.text, "Player").sendingCount.ToString() + "/" + packMan.GetPacketGroup(pac2Name.text, "Player").PacketCount().ToString();
        pac3Text.text = packMan.GetPacketGroup(pac3Name.text, "Player").sendingCount.ToString() + "/" + packMan.GetPacketGroup(pac3Name.text, "Player").PacketCount().ToString();
        pac4Text.text = packMan.GetPacketGroup(pac4Name.text, "Player").sendingCount.ToString() + "/" + packMan.GetPacketGroup(pac4Name.text, "Player").PacketCount().ToString();
        pac5Text.text = packMan.GetPacketGroup(pac5Name.text, "Player").sendingCount.ToString() + "/" + packMan.GetPacketGroup(pac5Name.text, "Player").PacketCount().ToString();
    }

    void Awake()
    {
        if ((packMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<PacketManager>()) == null)
            Debug.Log("Error: Sending Counter panel could not find PacketManager in Game Logic!");
    }

    void FixedUpdate()
    {
        UpdateSendingPacketCount();
    }
}
