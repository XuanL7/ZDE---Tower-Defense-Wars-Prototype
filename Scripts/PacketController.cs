using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PacketStats
{
    public float health = 10;
    public float armor = 10;
    public float speed = 10;
    public float shields = 10;
    public float shieldDelay = 10;
    public float shieldRate = 10;
    public float shieldCharge = 10;
}

[System.Serializable]
public class PacketCosts
{
    public float healthCost = 10;
    public float healthMultiplier = 2; // multiplied against cost for each upgrade

    public float armorCost = 10;
    public float armorMultiplier = 2;

    public float speedCost = 10;
    public float speedhMultiplier = 2;

    public float shieldCost = 10;
    public float shieldMultiplier = 2;

    public float shieldDelayCost = 10;
    public float ShieldDelayMultiplier = 2;

    public float shieldRateCost = 10;
    public float shieldRateMultiplier = 2;

    public float shieldChargeCost = 10;
    public float shieldChargeMultiplier = 2;

    public float addPacketCost = 100; // Cost to ad packet to group

    public float killValue = 10; // value this packet rewards for killing it
    public float killValueUpgrade = 1.5f;
    public float travelMultiplier = 1; // value to multiply against distance when Disabled() from spawn

    public float sendCost = 10; // base cost to send packet!
    public float extraSendCostPerUpgrade = 1;

    public float multiplyCostAfterMax = 2;
}

[System.Serializable]
public class PacketUpgradeMaximums
{
    public int healthMax = 3;
    public int armorMax = 3;
    public int speedMax = 3;
    public int shieldMax = 3;
    public int shieldDelayMax = 3;
    public int shiedRateMax = 3;
    public int shieldChargeMax = 3;
}

[System.Serializable]
public class PacketUpgrade
{
    // base value multiplied by these
    public float health = 2;
    public float armor = 2;
    public float speed = 2;
    public float shields = 2;
    // base value added by these
    public float shieldRate = 1;
    public float shieldDelay = -0.1f;
    public float shieldCharge = -0.1f;
}

public class PacketController : MonoBehaviour {

    /**                       PUBLIC MEMBERS              **/
    public PacketStats packetStats;
    public PacketCosts packetCosts;
    public PacketUpgradeMaximums packMax;
    public PacketUpgrade packetUpgrades;

    [HideInInspector]
    public bool sendNextWave = false; // FLAG to send on next wave

    /**                      PRIVATE MEMBERS             **/
    private string _type = "Default";

    public float health = 0;
    public float armor = 0;
    public float speed = 0;
    public float shields = 0;
    public float shieldRate = 0; // value added to shield drain, every shield charge
    public float shieldDelay = 0; // delay before shiels begin regenerating
    public float shieldCharge = 0; // time between each regen

    private float timeAlive; // the amount of time packet was enabled
    private float shieldDrain; // current state of shield value
    private bool isShieldRegening = false; // ensures InvokeRepeating("ShieldRegen") only called once
    private Transform _spawn;
    private AIPath _aiPath;
    private UpgradeManager upMan;
    private ComputerAI compAI;
    private bool isEnergy = false;

    /**                   GETTERS/SETTERS                **/

    public string Type
    {
        get { return _type; }
        set { _type = value; }
    }
    public Transform Spawn
    {
        get { return _spawn; }
        set { _spawn = value; }
    }

    /**               PUBLIC UTILITY METHODS             **/

    public PacketStats GetStats
    {
        get { return packetStats; }
    }
    public void SetTarget(Transform target)
    {
        _aiPath.target = target;
    }
    public Transform GetTarget()
    {
        return _aiPath.target;
    }

    public void EnergyType()
    {
        isEnergy = true;
    }

    public void TakeDamage(float damage, float piercedArmor)
    {
        // adjust armor absorb using passed value
        piercedArmor = armor - piercedArmor;
        if (piercedArmor > 80)
            piercedArmor = 80;
        // if less than 0, set to 0 to prevent additin of health
        if (piercedArmor < 0)
            piercedArmor = 0;

        // reduce shields or health from damage
        if (shieldDrain <= 0)
        {// Subtract armor piercing as a percentage from damage (cast to int for whole number value)
            health -= (float)(damage * (1 - (piercedArmor / 100.0)));
        }
        else
        {
            if (isEnergy)
                shieldDrain -= (damage * 2);
            else
                shieldDrain -= damage;
            CancelInvoke("ShieldRegen");
        }

        if (health <= 0)
        {
            Disable();
            KillReward();
        }

        // if shield NOT regening, start regen timer back up
        if (shieldDrain < shields && !isShieldRegening)
        {
            InvokeRepeating("ShieldRegen", shieldDelay, shieldCharge);
        }

        isEnergy = false;
    }

    public void TakeDebuff(TowerType towertype,float debuff ,float time)
    {
        float OriginSpeed;
        OriginSpeed = _aiPath.speed;
        if (_aiPath.speed - debuff <= 1)
            _aiPath.speed = 2;
        else
            _aiPath.speed = _aiPath.speed - debuff;
        StartCoroutine(Recover(OriginSpeed, time)); 
    }

    IEnumerator Recover(float OrignSpeed, float time)
    {
        yield return new WaitForSeconds(time);
        _aiPath.speed = OrignSpeed;
    }

    public void Enable()
    {
        health = packetStats.health;
        armor = packetStats.armor;
        speed = packetStats.speed;
        shields = packetStats.shields;

        shieldRate = packetStats.shieldRate;
        shieldDelay = packetStats.shieldDelay;
        shieldCharge = packetStats.shieldCharge;

        shieldDrain = shields;
        _aiPath.speed = speed;
        timeAlive = 0;
        InvokeRepeating("AliveTimer", 1, 1);
        this.gameObject.SetActive(true);
        sendNextWave = false;
    }
    public void Disable()
    {
        this.gameObject.SetActive(false);
        this.transform.position = _spawn.position;
        CancelInvoke("AliveTimer");
        TravelReward();
    }

    public void Upgrade_Health()
    {
        packetStats.health *= packetUpgrades.health;
        packetCosts.healthCost *= packetCosts.healthMultiplier;
        packetCosts.sendCost += packetCosts.extraSendCostPerUpgrade;
        packetCosts.killValue *= packetCosts.killValueUpgrade;
    }
    public void Upgrade_Armor()
    {
        packetStats.armor *= packetUpgrades.armor;
        packetCosts.armorCost *= packetCosts.armorMultiplier;
        packetCosts.sendCost += packetCosts.extraSendCostPerUpgrade;
        packetCosts.killValue *= packetCosts.killValueUpgrade;
    }
    public void Upgrade_Shields()
    {
        packetStats.shields *= packetUpgrades.shields;
        packetCosts.shieldCost *= packetCosts.shieldMultiplier;
        packetCosts.sendCost += packetCosts.extraSendCostPerUpgrade;
        packetCosts.killValue *= packetCosts.killValueUpgrade;
    }
    public void Upgrade_Speed()
    {
        packetStats.speed *= packetUpgrades.speed;
        packetCosts.speedCost *= packetCosts.speedhMultiplier;
        packetCosts.sendCost += packetCosts.extraSendCostPerUpgrade;
        packetCosts.killValue *= packetCosts.killValueUpgrade;
    }
    public void Upgrade_ShieldRate()
    {
        packetStats.shieldRate += packetUpgrades.shieldRate;
        packetCosts.shieldRateCost *= packetCosts.shieldRateMultiplier;
        packetCosts.sendCost += packetCosts.extraSendCostPerUpgrade;
        packetCosts.killValue *= packetCosts.killValueUpgrade;
    }
    public void Upgrade_ShieldCharge()
    {
        packetStats.shieldCharge += packetUpgrades.shieldCharge;
        packetCosts.shieldChargeCost *= packetCosts.shieldChargeMultiplier;
        packetCosts.sendCost += packetCosts.extraSendCostPerUpgrade;
        packetCosts.killValue *= packetCosts.killValueUpgrade;
    }
    public void Upgrade_ShieldDelay()
    {
        packetStats.shieldDelay += packetUpgrades.shieldDelay;
        packetCosts.shieldDelayCost *= packetCosts.ShieldDelayMultiplier;
        packetCosts.sendCost += packetCosts.extraSendCostPerUpgrade;
        packetCosts.killValue *= packetCosts.killValueUpgrade;
    }

    public float Get_Upgrade_Health_Cost()
    {
        return packetCosts.healthCost;
    }
    public float Get_Upgrade_Armor_Cost()
    {
        return packetCosts.armorCost;
    }
    public float Get_Upgrade_Shields_Cost()
    {
        return packetCosts.shieldCost;
    }
    public float Get_Upgrade_Speed_Cost()
    {
        return packetCosts.speedCost;
    }
    public float Get_Upgrade_ShieldRate_Cost()
    {
        return packetCosts.shieldRateCost;
    }
    public float Get_Upgrade_ShieldCharge_Cost()
    {
        return packetCosts.shieldChargeCost;
    }
    public float Get_Upgrade_ShieldDelay_Cost()
    {
        return packetCosts.shieldDelayCost;
    }

    /**              PRIVATE UTILITY METHOD              **/

    private void AliveTimer()
    {
        ++timeAlive;
    }
    private void TravelReward()
    {
        var heading = Spawn.position - transform.position;
        float distance = heading.magnitude;
        float rewardValue = (distance + timeAlive) * packetCosts.travelMultiplier;

        if (this.gameObject.tag == "AI")
            compAI.econ.addTowerFunds(rewardValue);
        else if (this.gameObject.tag == "Player")
            upMan.econ.addTowerFunds(rewardValue);
    }
    private void KillReward()
    {
        if (this.gameObject.tag == "AI")
            upMan.econ.addPacketFunds(packetCosts.killValue);
        else if (this.gameObject.tag == "Player")
            compAI.econ.addPacketFunds(packetCosts.killValue);
    }
    private void PlayerHitReward()
    {
        if (this.gameObject.tag == "Player")
            compAI.aiStats.PacketHit();
        else    
            upMan.playerStats.PacketHit();
    }
    private void ShieldRegen()
    {
        if ((shieldDrain + shieldRate) < shields)
        {// increase shields if below current shield stat
            shieldDrain += shieldRate;
            isShieldRegening = true;
        }
        else
        {// Disable shield regen
            CancelInvoke("ShieldRegen");
            isShieldRegening = false;
        }
    }

    /**             STANDARD UNITY METHOD               **/

    void Awake()
    {
        if ((_aiPath = this.gameObject.GetComponent<AIPath>()) == null)
            Debug.Log("Error: Packet._aiPath missing component 'AiPath'");
        if ((upMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<UpgradeManager>()) == null)
            Debug.Log("Error: packetController could not find UpgradeManager in Game Logic");
        if ((compAI = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<ComputerAI>()) == null)
            Debug.Log("Error: packetController could not find ComputerAI in Game Logic");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "End")
        {
            PlayerHitReward();
            Disable();
        }
    }
}
