using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class PlayerType
{
    public int health = 100;
    public int selfDamage = 1;
    public bool isNotPlayer = true;
    public AudioClip hitSound;
    private CameraController camCon;

    public void PacketHit()
    {
        health -= selfDamage;
        camCon = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<CameraController>();
        if (camCon != null)
            AudioSource.PlayClipAtPoint(hitSound, camCon.cameras[0].transform.position, 0.1f);
        if (health <= 0)
        {
            GameObject.FindGameObjectWithTag("EndGamePanel").GetComponent<EndGameScript>().ToggleEndGamePanel(isNotPlayer);
            Time.timeScale = 0.0f;
        }
    }
}

[System.Serializable]
public class Economy
{
    // Three currnecy types:
    [HideInInspector]
    public float energy = 0; // increase over time
    [HideInInspector]
    public float towerCash = 0; // gained from packet travel
    [HideInInspector]
    public float packetCash  = 0; // gained from tower kills

    // Starting funds for player and ai
    public float packetStartFunds = 100;
    public float towerStartFunds = 100;

    // energy gain values
    public float energyRate = 1; // amount gained per iteration
    public float energyTime = 1; // time between each iteration
    public float energyRateCost = 100;
    public float energyTimeCost = 500;
    public float energyCostMultiplier = 2;
    public float energyTimeUpgrade = 0.1f; // Value to subtract

    public void addTowerFunds(float reward)
    {
        towerCash += reward;
    }
    public void addPacketFunds(float reward)
    {
        packetCash += reward;
    }
}

[System.Serializable]
public class TowerUpgrades
{
    // Values to multiply against actual tower stats
    public float damageUpgrade = 2;
    public float armorPenUpgrade = 2;

    // Values to add to actual tower stats
    public float bulletSpeedUpgrade = 5f;
    public float bulletRangeUpgrade = 5f;
    public float reloadSpeedUpgrade = -0.1f;
    public float fireTimeSpeed = -0.1f;

    // Cost multiplier increase on cost after upgrade
    public int costMultiplier = 2;
}

[System.Serializable]
public class PacketUpgrades
{
    public float discountPerExtraPacket = 0.8f;
}

public class UpgradeManager : MonoBehaviour {
    public Economy econ = new Economy();
    public TowerUpgrades towUp = new TowerUpgrades();
    public PacketUpgrades packUp = new PacketUpgrades();
    public PlayerType playerStats = new PlayerType();

    private PacketManager packMan;
    private TowerManager towerMan;

    /** ECON Upgrade Methods     **/
    public void UpgradeEnergyRate()
    {
        if (econ.energyRateCost <= econ.towerCash)
        {
            econ.towerCash -= econ.energyRateCost;
            ++econ.energyRate;
            econ.energyRateCost *= econ.energyCostMultiplier;
        }
        else
           Debug.Log("Placeholder: Needs UI to inform lack of funds!");
    }

    public void UpgradeEnergyTime()
    {
        if (econ.energyTimeCost <= econ.towerCash)
        {
            econ.towerCash -= econ.energyTimeCost;
            econ.energyTime -= econ.energyTimeUpgrade;
            econ.energyTimeCost *= econ.energyCostMultiplier;
        }
        else
            Debug.Log("Placeholder: Needs UI to inform lack of funds!");
    }

    /** Packet Upgrade Methods **/

    public bool CanUpgrade(float cost)
    {
        if (cost <= econ.packetCash && cost != 0)
            return true;
        return false;
    }

    public float PacketsHealthCost(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = group.TotalHealthCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        return cost;
    }
    public float PacketsArmorCost(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = group.TotalArmorCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        return cost;
    }
    public float PacketsSpeedCost(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = group.TotalSpeedCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        return cost;
    }
    public float PacketsShieldCost(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = group.TotalShieldCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        return cost;
    }
    public float PacketsShieldRateCost(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = group.TotalShieldRateCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        return cost;
    }
    public float PacketsShieldChargeCost(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = group.TotalShieldChargeCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        return cost;
    }
    public float PacketsShieldDelayCost(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = group.TotalShieldDelayCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        return cost;
    }
    public float AddPacketCost(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        return group.AddPacketCost();
    }

    public void UpgradePacketsHealth(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = PacketsHealthCost(identity, player);
        econ.packetCash -= cost;
        group.UpgradeHealth();
    }
    public void UpgradePacketsArmor(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = PacketsArmorCost(identity, player);
        econ.packetCash -= cost;
        group.UpgradeArmor();
    }
    public void UpgradePacketsSpeed(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = PacketsSpeedCost(identity, player);
        econ.packetCash -= cost;
        group.UpgradeSpeed();
    }
    public void UpgradePacketsShields(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = PacketsShieldCost(identity, player);
        econ.packetCash -= cost;
        group.UpgradeShields();
    }
    public void UpgradePacketsShieldRate(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = PacketsShieldRateCost(identity, player);
        econ.packetCash -= cost;
        group.UpgradeShieldRate();
    }
    public void UpgradePacketsShieldCharge(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = PacketsShieldChargeCost(identity, player);
        econ.packetCash -= cost;
        group.UpgradeShieldCharge();
    }
    public void UpgradePacketsShieldDelay(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        float cost = PacketsShieldDelayCost(identity, player);
        econ.packetCash -= cost;
        group.UpgradeShieldDelay();
    }
    public void AddPacket(string identity, string player)
    {
        float cost = AddPacketCost(identity, player);
        econ.packetCash -= cost;
        packMan.AddPacket(identity, player);
    }

    public void sendPacket(string identity, string player)
    {
        if(CanSendPacket(identity, player))
        {
            packMan.GetPacketGroup(identity, player).SetPacket_SendNextWave();
            econ.energy -= packMan.GetPacketGroup(identity, player).SendCost();
        }    
    }
    public bool CanSendPacket(string identity, string player)
    {
        PacketGroup group = packMan.GetPacketGroup(identity, player);
        if (group.SendCost() != -1 && group.SendCost() <= econ.energy && group.AreSendPacketsRemaining())
            return true;
        return false;
    }

    /** Tower Upgrade Methods    **/
    
    public bool purchaseTower(int cost)
    {
        if(cost <= econ.towerCash)
        {
            econ.towerCash -= cost;
            return true;
        }
        return false;
    }

    public void setTowerButtonActive()
    {
        foreach(Button button in towerMan.towerButtons)
        {
            if (button.GetComponent<TowerBtnScript>().GetTowerCost() <= econ.towerCash)
            {
                button.interactable = true;
            }
            else
                button.interactable = false;
        }
    }

    public void UpgradeTowerDamage(TowerController tower)
    {
        if (damageCost(tower) <= econ.towerCash && tower.maxUpgradeDamage > tower.upDamageCount)
        {
            econ.towerCash -= damageCost(tower);
            tower.MultiplyStats(towUp.damageUpgrade, 1);
            ++tower.upDamageCount;
        }
        else
            Debug.Log("Placeholder to inform lack of funds!");
    }

    public int damageCost(TowerController tower)
    {
        if (tower.maxUpgradeDamage <= tower.upDamageCount) return 0;

        int cost = tower.damageCost;
        if (tower.upDamageCount > 1)
        {
            cost *= towUp.costMultiplier * (tower.upDamageCount - 1);
            cost *= towUp.costMultiplier;
        }
        return cost;
    }

    public void UpgradeTowerArmor(TowerController tower )
    {
        if (ArmorCost(tower) <= econ.towerCash && tower.maxUpgradeArmor > tower.upArmorPierceCount)
        {
            econ.towerCash -= ArmorCost(tower);
            tower.MultiplyStats(1, towUp.armorPenUpgrade);
            ++tower.upArmorPierceCount;
        }
        else
            Debug.Log("Placeholder to inform lack of funds!");
    }

    public int ArmorCost(TowerController tower)
    {
        if (tower.maxUpgradeArmor <= tower.upArmorPierceCount) return 0;

        int cost = tower.armorCost;
        if (tower.upArmorPierceCount > 1)
        {
            cost *= towUp.costMultiplier * (tower.upArmorPierceCount - 1);
            cost *= towUp.costMultiplier;
        }
        return cost;
    }

    public void UpgradeTowerBulletSpeed(TowerController tower)
    {
        if (BulletSpeedCost(tower) <= econ.towerCash && tower.maxUpgradeBSpeed > tower.upBulletSpeedCount)
        {
            econ.towerCash -= BulletSpeedCost(tower);
            tower.ModifyStats(towUp.bulletSpeedUpgrade, 0, 0, 0);
            ++tower.upBulletSpeedCount;
        }
        else
            Debug.Log("Placeholder to inform lack of funds!");
    }

    public int BulletSpeedCost(TowerController tower)
    {
        if (tower.maxUpgradeBSpeed <= tower.upBulletSpeedCount) return 0;

        int cost = tower.bulletSpeedCost;
        if (tower.upBulletSpeedCount > 1)
        {
            cost *= towUp.costMultiplier * (tower.upBulletSpeedCount - 1);
            cost *= towUp.costMultiplier;
        }
        return cost;
    }

    public void UpgradeTowerBulletRange(TowerController tower)
    {
        if (BulletRangeCost(tower) <= econ.towerCash && tower.maxUpgradeBRange > tower.upbulletRangeCount)
        {
            econ.towerCash -= BulletRangeCost(tower);
            tower.ModifyStats(0, towUp.bulletRangeUpgrade, 0, 0);
            ++tower.upbulletRangeCount;
        }
        else
            Debug.Log("Placeholder to inform lack of funds!");
    }

    public int BulletRangeCost(TowerController tower)
    {
        if (tower.maxUpgradeBRange <= tower.upbulletRangeCount) return 0;

        int cost = tower.bulletRangeCost;
        if (tower.upbulletRangeCount > 1)
        {
            cost *= towUp.costMultiplier * (tower.upbulletRangeCount - 1);
            cost *= towUp.costMultiplier;
        }
        return cost;
    }

    public void UpgradeTowerReload(TowerController tower)
    {
        if (ReloadCost(tower) <= econ.towerCash && tower.maxUpgradeReload > tower.upReloadSpeed)
        {
            econ.towerCash -= ReloadCost(tower);
            tower.ModifyStats(0, 0, towUp.reloadSpeedUpgrade, 0);
            ++tower.upReloadSpeed;
        }
        else
            Debug.Log("Placeholder to inform lack of funds!");
    }

    public int ReloadCost(TowerController tower)
    {
        if (tower.maxUpgradeReload <= tower.upReloadSpeed) return 0;

        int cost = tower.reloadSpeedCost;
        if (tower.upReloadSpeed > 1)
        {
            cost *= towUp.costMultiplier * (tower.upReloadSpeed - 1);
            cost *= towUp.costMultiplier;
        }
        return cost;
    }
    

    /** ECON MANAGEMENT Methods  **/

    private void EnergyGain()
    {
        econ.energy += econ.energyRate;
    }

    /** Standard Control Methods **/

    void Awake()
    {
        if((packMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<PacketManager>()) == null)
            Debug.Log("Error: Could not find PacketManager on ObjectWithTag 'Game Logic' in UpgradeManager");
        if((towerMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<TowerManager>()) == null)
            Debug.Log("Error: Could not find TowerManager on ObjectWithTag 'Game Logic' in UpgradeManager");
    }

    void Start()
    {
        econ.towerCash = econ.towerStartFunds;
        econ.packetCash = econ.packetStartFunds;
        InvokeRepeating("EnergyGain", econ.energyTime, econ.energyTime);
    }

    void Update()
    {
        setTowerButtonActive();
    }
}
