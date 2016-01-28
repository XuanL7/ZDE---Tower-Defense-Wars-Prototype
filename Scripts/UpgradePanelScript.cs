using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class PanelInfo
{
    public Text packetNameText;

    public Text hPText;
    public Text armText;
    public Text spdText;
    public Text shldText;
    public Text shldDelText;
    public Text shldRatText;
    public Text shldCrgText;

    public Text btnHPText;
    public Text btnArmText;
    public Text btnSpdText;
    public Text btnShldText;
    public Text btnShldDelText;
    public Text btnShldRatText;
    public Text btnshldCrgText;

    public Button hpButton;
    public Button armButton;
    public Button spdButton;
    public Button shldButton;
    public Button shldRatButton;
    public Button shldDelButton;
    public Button shldCrgButton;

    public Text addPacketCost;
    public Text addPacketCount;
    public Button addPAcketButton;
}

public class UpgradePanelScript : MonoBehaviour {

    /** PUBLIC MEMBERS **/
    public PanelInfo panel;

    /** PRIVATE MEMBERS **/
    private PacketManager packMan;
    private UpgradeManager upMan;
    private string player = "Player";
    private string identity = "";
    private PacketGroup currentGroup;
    private PacketStats myStats;

    /** PUBLIC UTILITY METHODS  **/

    public void EnablePanel()
    {
        this.gameObject.SetActive(true);
    }
    public void DisablePanel()
    {
        this.gameObject.SetActive(false);
    }
    public void changeGroup(Button btn)
    {
        identity = btn.GetComponentInChildren<Text>().text;
        currentGroup = packMan.GetPacketGroup(identity, player);
        myStats = currentGroup.GetGroupStats();
    }

    public void UpgradeHealth()
    {
        upMan.UpgradePacketsHealth(identity, player);
    }
    public void UpgradeArmor()
    {
        upMan.UpgradePacketsArmor(identity, player);
    }
    public void UpgradeSpeed()
    {
        upMan.UpgradePacketsSpeed(identity, player);
    }
    public void UpgradeShields()
    {
        upMan.UpgradePacketsShields(identity, player);
    }
    public void UpgradeShieldRate()
    {
        upMan.UpgradePacketsShieldRate(identity, player);
    }
    public void UpgradeShieldDelay()
    {
        upMan.UpgradePacketsShieldDelay(identity, player);
    }
    public void UpgradeShieldCharge()
    {
        upMan.UpgradePacketsShieldCharge(identity, player);
    }
    public void AddPacket()
    {
        upMan.AddPacket(identity, player);
    }

    /** PRIVATE UTILITY METHODS **/

    private void UpdateStats()
    {
        panel.packetNameText.text = identity;

        panel.hPText.text = myStats.health.ToString();
        panel.armText.text = myStats.armor.ToString();
        panel.spdText.text = myStats.speed.ToString();
        panel.shldText.text = myStats.shields.ToString();
        panel.shldDelText.text = myStats.shieldDelay.ToString();
        panel.shldRatText.text = myStats.shieldRate.ToString();
        panel.shldCrgText.text = myStats.shieldCharge.ToString();

        panel.addPacketCount.text = currentGroup.PacketCount().ToString();
    }
    private void UpdateCost()
    {
        float cost;
        cost = upMan.PacketsHealthCost(identity, player);
        setButtonState(cost, panel.btnHPText, panel.hpButton);

        cost = upMan.PacketsArmorCost(identity, player);
        setButtonState(cost, panel.btnArmText, panel.armButton);


        cost = upMan.PacketsSpeedCost(identity, player);
        setButtonState(cost, panel.btnSpdText, panel.spdButton);

        cost = upMan.PacketsShieldCost(identity, player);
        setButtonState(cost, panel.btnShldText, panel.shldButton);

        cost = upMan.PacketsShieldRateCost(identity, player);
        setButtonState(cost, panel.btnShldRatText, panel.shldRatButton);

        cost = upMan.PacketsShieldDelayCost(identity, player);
        setButtonState(cost, panel.btnShldDelText, panel.shldDelButton);

        cost = upMan.PacketsShieldChargeCost(identity, player);
        setButtonState(cost, panel.btnshldCrgText, panel.shldCrgButton);

        cost = upMan.AddPacketCost(identity, player);
        panel.addPacketCost.text = currentGroup.AddPacketCost().ToString();
        setButtonState(cost, panel.addPacketCost, panel.addPAcketButton);
    }
    private void setButtonState(float cost, Text txt ,Button btn)
    {
        if (upMan.CanUpgrade(cost))
        {
            txt.text = cost.ToString();
            btn.interactable = true;
        }
        else
        {
            txt.text = "";
            btn.interactable = false;
        }
    }

    /** STANDARD UNITY METHODS **/

    private void Awake()
    {
        packMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<PacketManager>();
        identity = packMan.packetButtons[0].GetComponentInChildren<Text>().text;
        currentGroup = packMan.GetPacketGroup(identity, player);
        upMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<UpgradeManager>();
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (this.gameObject.activeSelf == true)
        {
            UpdateStats();
            UpdateCost();
        }
    }
}
