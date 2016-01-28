using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPacketGroups
{
    public PacketGroup group1;
    public PacketGroup group2;
    public PacketGroup group3;
    public PacketGroup group4;
    public PacketGroup group5;
}

[System.Serializable]
public class AITowers
{
    // Array of in-game tower types: both active and inactive
    public GameObject[] walls;
    public GameObject[] basicTowers;
    public GameObject[] laserTowers;
    public GameObject[] artilleryTowers;
    public GameObject[] aoeTowers;

    [HideInInspector]
    public List<TowerController> wallCons;
    [HideInInspector]
    public List<TowerController> basicCons;
    [HideInInspector]
    public List<TowerController> laserCons;
    [HideInInspector]
    public List<TowerController> artilleryCons;
    [HideInInspector]
    public List<TowerController> aoeCons;
}

public class ComputerAI : MonoBehaviour {

    /** PUBLIC MEMEBERS **/
    public PlayerType aiStats = new PlayerType();
    public Economy econ = new Economy();
    public PacketUpgrades packUp = new PacketUpgrades();
    public TowerUpgrades towUp = new TowerUpgrades();
    public AITowers aiTowers = new AITowers();

    /** PRIVATE MEMEBERS **/
    private PacketManager packMan;
    private AIPacketGroups aiGroups = new AIPacketGroups(); // for direct access of each packet gorup

    /** PRIVATE UTILITY METHOD **/

    private TowerController GetController(string towerID, int numericID)
    {
        if (towerID == "artillery")
        {
            foreach (TowerController tCon in aiTowers.artilleryCons)
                if (tCon.numericID == numericID)
                    return tCon;
        }
        if (towerID == "basic")
        {
            foreach (TowerController tCon in aiTowers.basicCons)
                if (tCon.numericID == numericID)
                    return tCon;
        }
        if (towerID == "laser")
        {
            foreach (TowerController tCon in aiTowers.laserCons)
                if (tCon.numericID == numericID)
                    return tCon;
        }
        if (towerID == "aoe")
        {
            foreach (TowerController tCon in aiTowers.aoeCons)
                if (tCon.numericID == numericID)
                    return tCon;
        }
        return null;
    }
    private PacketGroup GetGroup(int groupNum)
    {
        switch(groupNum)
        {
            case (1):
                return aiGroups.group1;
            case (2):
                return aiGroups.group2;
            case (3):
                return aiGroups.group3;
            case (4):
                return aiGroups.group4;
            case (5):
                return aiGroups.group5;
            default:
                return null;
        }
    }
    private bool CanSendPacket(PacketGroup group, bool cheat)
    {
        if(!cheat)
        {
            if (group.SendCost() != -1 && group.SendCost() <= econ.energy && group.AreSendPacketsRemaining())
                return true;
        }
        else if (group.AreSendPacketsRemaining())
            return true;
        return false;
    }


    private void UnlockTower(int numericID, TowerType tType)
    {
        switch(tType)
        {
            case TowerType.BasicTower:
                foreach (TowerController tower in aiTowers.basicCons)
                    if (tower.numericID == numericID)
                        tower.gameObject.SetActive(true);
                break;
            case TowerType.LaserTower:
                foreach (TowerController tower in aiTowers.laserCons)
                    if (tower.numericID == numericID)
                        tower.gameObject.SetActive(true);
                break;
            case TowerType.SnowTower:
                foreach (TowerController tower in aiTowers.aoeCons)
                    if (tower.numericID == numericID)
                        tower.gameObject.SetActive(true);
                break;
            case TowerType.ArtilleryTower:
                foreach (TowerController tower in aiTowers.artilleryCons)
                    if (tower.numericID == numericID)
                        tower.gameObject.SetActive(true);
                break;
        }
    }

    private void RandomTowerUpgrade()
    {
        int randType = Random.Range(1,4);
        switch(randType)
        {
            case 1: // basic
                Debug.Log("Basic Upgrade");
                foreach(TowerController tower in aiTowers.basicCons)
                {
                    if (tower.gameObject.activeSelf)
                    {
                        int randUpgrade = Random.Range(1, 5);
                        switch (randUpgrade)
                        {
                            case 1: // damage
                                    UpgradeDamage(tower, true);
                                break;
                            case 2: // armor pierce
                                    UpgradeArmor(tower, true);
                                break;
                            case 3: // bSpeed
                                    UpgradeBulletSpeed(tower, true);
                                break;
                            case 4: // bRange
                                    UpgradeBulletSpeed(tower, true);
                                break;
                            case 5: // reload
                                    UpgradeReload(tower, false);
                                break;
                        }
                        break;
                    }
                }
                break;
            case 2: // laser
                foreach(TowerController tower in aiTowers.laserCons)
                {
                    if (tower.gameObject.activeSelf)
                    {
                        int randUpgrade = Random.Range(1, 3);
                        switch (randUpgrade)
                        {
                            case 1: // damage
                                    UpgradeDamage(tower, true);
                                break;
                            case 2: // bRange
                                    UpgradeBulletSpeed(tower, true);
                                break;
                            case 3: // reload
                                    UpgradeReload(tower, true);
                                break;
                        }
                        break;
                    }
                }
                break;
            case 3: // artillery
                foreach(TowerController tower in aiTowers.artilleryCons)
                {
                    if (tower.gameObject.activeSelf)
                    {
                        int randUpgrade = Random.Range(1, 5);
                        switch (randUpgrade)
                        {
                            case 1: // damage
                                    UpgradeDamage(tower, true);
                                break;
                            case 2: // armor pierce
                                    UpgradeArmor(tower, true);
                                break;
                            case 3: // bSpeed
                                    UpgradeBulletSpeed(tower, true);
                                break;
                            case 4: // bRange
                                    UpgradeBulletSpeed(tower, true);
                                break;
                            case 5: // reload
                                    UpgradeReload(tower, true);
                                break;
                        }
                        break;
                    }
                }
                break;
            case 4: // snow
                foreach (TowerController tower in aiTowers.aoeCons)
                {
                    if (tower.gameObject.activeSelf)
                    {
                        int randUpgrade = Random.Range(1, 2);
                        switch (randUpgrade)
                        {
                            case 1: // damage
                                    UpgradeDamage(tower, true);
                                break;
                            case 2: // bRange
                                    UpgradeBulletSpeed(tower, false);
                                break;
                        }
                        break;
                    }
                }
                break;
            default:
                break;
        }
    }

    private void RandomPacketUpgrade()
    {
        int randGroup = Random.Range(1,5);
        int randStat = Random.Range(1,7);
        switch(randStat)
        {
            case 1: // hp
                Debug.Log("Group HP");
                UpgradeGroupHealth(GetGroup(randGroup), true);
                break;
            case 2: // armor
                Debug.Log("Group Armor");
                UpgradeGroupArmor(GetGroup(randGroup), true);
                break;
            case 3: // speed
                Debug.Log("Group Speed");
                UpgradeGroupSpeed(GetGroup(randGroup), true);
                break;
            case 4: // shield
                Debug.Log("Group Shields");
                UpgradeGroupShields(GetGroup(randGroup), true);
                break;
            case 5: // rate
                Debug.Log("Group Rate");
                UpgradeGroupShieldRate(GetGroup(randGroup), true);
                break;
            case 6: // delay
                Debug.Log("Group Delay");
                UpgradeGroupShieldDelay(GetGroup(randGroup), true);
                break;
            case 7: // charge
                Debug.Log("Group Charge");
                UpgradeGroupShieldCharge(GetGroup(randGroup), true);
                break;
        } 
    }

    private void UpgradeDamage(TowerController tower, bool cheat)
    {
        if (cheat)
        {
            if(tower.maxUpgradeDamage > tower.upDamageCount)
            {
                tower.MultiplyStats(towUp.damageUpgrade, 1);
                ++tower.upDamageCount;
            }
        }
        else
        {
            if (damageCost(tower) <= econ.towerCash && tower.maxUpgradeDamage > tower.upDamageCount)
            {
                econ.towerCash -= damageCost(tower);
                tower.MultiplyStats(towUp.damageUpgrade, 1);
                ++tower.upDamageCount;
            }
        }   
    }
    private void UpgradeArmor(TowerController tower, bool cheat)
    {
        if (cheat)
        {
            if (tower.maxUpgradeArmor > tower.upArmorPierceCount)
            {
                tower.MultiplyStats(1, towUp.armorPenUpgrade);
                ++tower.upArmorPierceCount;
            }
        }
        else
        {
            if (ArmorCost(tower) <= econ.towerCash && tower.maxUpgradeArmor > tower.upArmorPierceCount)
            {
                econ.towerCash -= ArmorCost(tower);
                tower.MultiplyStats(1, towUp.armorPenUpgrade);
                ++tower.upArmorPierceCount;
            }
        } 
    }
    private void UpgradeBulletSpeed(TowerController tower, bool cheat)
    {
        if (cheat)
        {
            if (tower.maxUpgradeBSpeed > tower.upBulletSpeedCount)
            {
                tower.ModifyStats(towUp.bulletSpeedUpgrade, 0, 0, 0);
                ++tower.upBulletSpeedCount;
            }
        }
        else
        {
            if (BulletSpeedCost(tower) <= econ.towerCash && tower.maxUpgradeBSpeed > tower.upBulletSpeedCount)
            {
                econ.towerCash -= BulletSpeedCost(tower);
                tower.ModifyStats(towUp.bulletSpeedUpgrade, 0, 0, 0);
                ++tower.upBulletSpeedCount;
            }
        }
    }
    private void UpgradeBulletRange(TowerController tower, bool cheat)
    {
        if (cheat)
        {
            if (tower.maxUpgradeBRange > tower.upbulletRangeCount)
            {
                tower.ModifyStats(0, towUp.bulletRangeUpgrade, 0, 0);
                ++tower.upbulletRangeCount;
            }
        }
        else
        {
            if (BulletRangeCost(tower) <= econ.towerCash && tower.maxUpgradeBRange > tower.upbulletRangeCount)
            {
                econ.towerCash -= BulletRangeCost(tower);
                tower.ModifyStats(0, towUp.bulletRangeUpgrade, 0, 0);
                ++tower.upbulletRangeCount;
            }
        } 
    }
    private void UpgradeReload(TowerController tower, bool cheat)
    {
        if (cheat)
        {
            if (tower.maxUpgradeReload > tower.upReloadSpeed)
            {
                tower.ModifyStats(0, 0, towUp.reloadSpeedUpgrade, 0);
                ++tower.upReloadSpeed;
            }
        }
        else
        {
            if (ReloadCost(tower) <= econ.towerCash && tower.maxUpgradeReload > tower.upReloadSpeed)
            {
                econ.towerCash -= ReloadCost(tower);
                tower.ModifyStats(0, 0, towUp.reloadSpeedUpgrade, 0);
                ++tower.upReloadSpeed;
            }
        } 
    }
    private void UpgradeEnergyRate(bool cheat)
    {
        if (cheat)
        {
            ++econ.energyRate;
            econ.energyRateCost *= econ.energyCostMultiplier;
        }
        else
        {
            if (econ.energyRateCost <= econ.towerCash)
            {
                econ.towerCash -= econ.energyRateCost;
                ++econ.energyRate;
                econ.energyRateCost *= econ.energyCostMultiplier;
            }
        }
    }
    private void UpgradeEnergyTime(bool cheat)
    {
        if (cheat)
        {
            econ.energyTime -= econ.energyTimeUpgrade;
            econ.energyTimeCost *= econ.energyCostMultiplier;
        }
        else
        {
            if (econ.energyTimeCost <= econ.towerCash)
            {
                econ.towerCash -= econ.energyTimeCost;
                econ.energyTime -= econ.energyTimeUpgrade;
                econ.energyTimeCost *= econ.energyCostMultiplier;
            }
        }
    }

    private int damageCost(TowerController tower)
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
    private int ArmorCost(TowerController tower)
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
    private int BulletSpeedCost(TowerController tower)
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
    private int BulletRangeCost(TowerController tower)
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
    private int ReloadCost(TowerController tower)
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

    public float AddPacketCost(PacketGroup group)
    {
        return group.AddPacketCost();
    }
    private bool CanUpgradePacket(float cost)
    {
        if (cost <= econ.packetCash && cost != 0)
            return true;
        return false;
    }
    private bool CanUpgradeTower(float cost)
    {
        if (cost <= econ.towerCash && cost != 0)
            return true;
        return false;
    }

    private void UpgradeGroupHealth(PacketGroup group, bool cheat)
    {
        if(cheat)
        {
            group.UpgradeHealth();
        }
        else if (econ.packetCash >= GroupHealthCost(group))
        {
            Debug.Log("Econ - " + econ.packetCash);
            econ.packetCash -= GroupHealthCost(group);
            Debug.Log("Econ - " + econ.packetCash);
            group.UpgradeHealth();
        }
    }
    private void UpgradeGroupArmor(PacketGroup group, bool cheat)
    {
        if (cheat)
        {
            group.UpgradeArmor();
        }
        else if (econ.packetCash >= GroupArmorCost(group))
        {
            Debug.Log("Econ - " + econ.packetCash);
            econ.packetCash -= GroupArmorCost(group);
            Debug.Log("Econ - " + econ.packetCash);
            group.UpgradeArmor();
        }
    }
    private void UpgradeGroupSpeed(PacketGroup group, bool cheat)
    {
        if (cheat)
        {
            group.UpgradeSpeed();
        }
        else if (econ.packetCash >= GroupSpeedCost(group))
        {
            Debug.Log("Econ - " + econ.packetCash);
            econ.packetCash -= GroupSpeedCost(group);
            Debug.Log("Econ - " + econ.packetCash);
            group.UpgradeSpeed();
        }
    }
    private void UpgradeGroupShields(PacketGroup group, bool cheat)
    {
        if (cheat)
        {
            group.UpgradeShields();
        }
        else if (econ.packetCash >= GroupShieldCost(group))
        {
            Debug.Log("Econ - " + econ.packetCash);
            econ.packetCash -= GroupShieldCost(group);
            Debug.Log("Econ - " + econ.packetCash);
            group.UpgradeShields();
        }
    }
    private void UpgradeGroupShieldRate(PacketGroup group, bool cheat)
    {
        if (cheat)
        {
            group.UpgradeShieldRate();
        }
        else if (econ.packetCash >= GroupShieldRateCost(group))
        {
            Debug.Log("Econ - " + econ.packetCash);
            econ.packetCash -= GroupShieldRateCost(group);
            Debug.Log("Econ - " + econ.packetCash);
            group.UpgradeShieldRate();
        }
    }
    private void UpgradeGroupShieldDelay(PacketGroup group, bool cheat)
    {
        if (cheat)
        {
            group.UpgradeShieldDelay();
        }
        else if (econ.packetCash >= GroupShieldDelayCost(group))
        {
            Debug.Log("Econ - " + econ.packetCash);
            econ.packetCash -= GroupShieldDelayCost(group);
            Debug.Log("Econ - " + econ.packetCash);
            group.UpgradeShieldDelay();
        }
    }
    private void UpgradeGroupShieldCharge(PacketGroup group, bool cheat)
    {
        if (cheat)
        {
            group.UpgradeShieldCharge();
        }
        else if(econ.packetCash >= GroupShieldChargeCost(group))
        {
            Debug.Log("Econ - " + econ.packetCash);
            econ.packetCash -= GroupShieldChargeCost(group);
            Debug.Log("Econ - " + econ.packetCash);
            group.UpgradeShieldCharge();
        }
    }

    private float GroupHealthCost(PacketGroup group)
    {
        float cost = group.TotalHealthCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        Debug.Log("Cost = " + cost);
        return cost;
    }
    private float GroupArmorCost(PacketGroup group)
    {
        float cost = group.TotalArmorCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        Debug.Log("Cost = " + cost);
        return cost;
    }
    private float GroupSpeedCost(PacketGroup group)
    {
        float cost = group.TotalSpeedCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        Debug.Log("Cost = " + cost);
        return cost;
    }
    private float GroupShieldCost(PacketGroup group)
    {
        float cost = group.TotalShieldCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        Debug.Log("Cost = " + cost);
        return cost;
    }
    private float GroupShieldRateCost(PacketGroup group)
    {
        float cost = group.TotalShieldRateCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        Debug.Log("Cost = " + cost);
        return cost;
    }
    private float GroupShieldChargeCost(PacketGroup group)
    {
        float cost = group.TotalShieldChargeCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        Debug.Log("Cost = " + cost);
        return cost;
    }
    private float GroupShieldDelayCost(PacketGroup group)
    {
        float cost = group.TotalShieldDelayCost();
        if (group.PacketCount() > 1)
            cost = (cost / group.PacketCount()) + ((cost * packUp.discountPerExtraPacket) * (group.PacketCount() - 1));
        Debug.Log("Cost = " + cost);
        return cost;
    }

    /////////////////////////////  WAVE METHODS (During enact behavior) /////////////////////////////////
    private void UpgradeTowerStat(string stat, string towerID, int numericID, bool cheat)
    {
        TowerController tCon = GetController(towerID, numericID);
        switch (stat)
        {
            case ("damage"):
                UpgradeDamage(tCon, cheat);
                break;
            case ("1"):
                UpgradeDamage(tCon, cheat);
                break;
            case ("armorPEn"):
                UpgradeArmor(tCon, cheat);
                break;
            case ("2"):
                UpgradeArmor(tCon, cheat);
                break;
            case ("bulletSpeed"):
                UpgradeBulletSpeed(tCon, cheat);
                break;
            case ("3"):
                UpgradeBulletSpeed(tCon, cheat);
                break;
            case ("bulletRange"):
                UpgradeBulletRange(tCon, cheat);
                break;
            case ("4"):
                UpgradeBulletRange(tCon, cheat);
                break;
            case ("reloadSpeed"):
                UpgradeReload(tCon, cheat);
                break;
            case ("5"):
                UpgradeReload(tCon, cheat);
                break;
            default:
                Debug.Log("Error: No such stat of type: " + stat + "  or tower with IDs: " + towerID + "," + numericID);
                break;
        }
    }
    private void UpgradePacketGroup(int groupNum, string stat, bool cheat)
    {
        PacketGroup group = GetGroup(groupNum);
        if(group != null)
        {
            switch (stat)
            {
                case ("health"):
                    UpgradeGroupHealth(group, cheat);
                    Debug.Log("HP");
                    break;
                case ("1"):
                    UpgradeGroupHealth(group, cheat);
                    Debug.Log("HP");
                    break;
                case ("armor"):
                    UpgradeGroupArmor(group, cheat);
                    Debug.Log("ar");
                    break;
                case ("2"):
                    UpgradeGroupArmor(group, cheat);
                    Debug.Log("ar");
                    break;
                case ("speed"):
                    UpgradeGroupSpeed(group, cheat);
                    Debug.Log("sp");
                    break;
                case ("3"):
                    UpgradeGroupSpeed(group, cheat);
                    Debug.Log("sp");
                    break;
                case ("shields"):
                    UpgradeGroupShields(group, cheat);
                    Debug.Log("sh");
                    break;
                case ("4"):
                    UpgradeGroupShields(group, cheat);
                    Debug.Log("sh");
                    break;
                case ("shieldRate"):
                    UpgradeGroupShieldRate(group, cheat);
                    Debug.Log("rat");
                    break;
                case ("5"):
                    UpgradeGroupShieldRate(group, cheat);
                    Debug.Log("rat");
                    break;
                case ("shieldDelay"):
                    UpgradeGroupShieldDelay(group, cheat);
                    Debug.Log("del");
                    break;
                case ("6"):
                    UpgradeGroupShieldDelay(group, cheat);
                    Debug.Log("del");
                    break;
                case ("shieldCharge"):
                    UpgradeGroupShieldCharge(group, cheat);
                    Debug.Log("crg");
                    break;
                case ("7"):
                    UpgradeGroupShieldCharge(group, cheat);
                    Debug.Log("crg");
                    break;
                default:
                    Debug.Log("Error: No stat of type: " + stat);
                    break;
            }
        }
        else
            Debug.Log("Error: invalid group: " + groupNum);
    }
    private void AddPacket(int groupNum, bool cheat)
    {
        PacketGroup group = GetGroup(groupNum);
        if(group != null)
        {
            if(cheat)
            {
                packMan.AddPacket(group.Ident, "AI");
            }
            else
            {
                 if(CanUpgradePacket(AddPacketCost(group)))
                 {
                    econ.packetCash -= AddPacketCost(group);
                    packMan.AddPacket(group.Ident, "AI");
                 }
            }
        }
        else
            Debug.Log("Error: no such group: " + groupNum);
    }
    private void SendPackets(int groupNum, int numToSend, bool cheat)
    {
        PacketGroup group = GetGroup(groupNum);
        if(group != null)
        {
            for(int i = 0; i < numToSend; ++i)
            if(CanSendPacket(group, cheat))
            {
                group.SetPacket_SendNextWave();
                if (!cheat)
                    econ.energy -= group.SendCost();
            }
        }
        else
            Debug.Log("Error: No such group: " + groupNum);
    }
    private void SendGroup(int groupNum)
    {
        PacketGroup group = GetGroup(groupNum);
        if (group != null)
        {
            while (group.AreSendPacketsRemaining())
            {
                group.SetPacket_SendNextWave();
            }
        }
        else
            Debug.Log("Error: No such group: " + groupNum);
    }
    private void SendAllGroups()
    {
        for(int i = 1; i <= 5; ++i)
        {
            PacketGroup group = GetGroup(i);
            if (group != null)
            {
                while (group.AreSendPacketsRemaining())
                {
                    group.SetPacket_SendNextWave();
                }
            }
            else
                Debug.Log("Error: No such group: " + i);
        }
    }

    /** PUBLIC UTILITY METHODS **/

    //////////////////////////// WAVE METHOD: ENACT BEHAVIOR ///////////////////////////////////
    public void enactNextBehavior(int currentWave)
    {
        switch (currentWave)
        {
            case 1:// tutorial Wave

                SendPackets(1, 1, true);

                UnlockTower(1, TowerType.BasicTower);

                UpgradeTowerStat("1", "basic", 1, true);
                break;
            case 2:// Show them amor packet
                SendPackets(1, 1, true);
                SendPackets(2, 1, true);
                // DO STUFF                

                break;
            case 3:// Speed packet is comming out
                SendPackets(1, 1, true);
                SendPackets(2, 1, true);
                SendPackets(3, 1, true);

                UnlockTower(1, TowerType.LaserTower);
                UnlockTower(4, TowerType.BasicTower);

                // DO STUFF
                break;
            case 4:// Shild packet is comming out
                SendGroup(1);
                SendGroup(2);
                SendGroup(4);

                break;
            case 5:// First stage for Pepe
                AddPacket(5, true);

                SendGroup(5);

                break;
            case 6:// Bunch of health packets
                AddPacket(1, true); AddPacket(1, true); AddPacket(1, true); AddPacket(1, true);

                SendGroup(1);


                break;
            case 7:// Mixed packets
                AddPacket(2, true); AddPacket(2, true); AddPacket(2, true); AddPacket(2, true);
                AddPacket(3, true); AddPacket(3, true); AddPacket(3, true); AddPacket(3, true);
                AddPacket(4, true); AddPacket(4, true); AddPacket(4, true); AddPacket(4, true);

                SendPackets(1, 2, true);
                SendPackets(2, 2, true);
                SendPackets(3, 2, true);

                UnlockTower(1, TowerType.SnowTower);
                break;
            case 8:// Speed packet wave;        
                SendGroup(3);

                UnlockTower(1, TowerType.ArtilleryTower);
                UnlockTower(2, TowerType.BasicTower);
                break;

            case 9://normal stage
                SendPackets(1, 2, true);
                SendPackets(2, 2, true);
                SendPackets(3, 2, true);
                SendPackets(4, 2, true);

                UnlockTower(2, TowerType.LaserTower);
                UnlockTower(3, TowerType.BasicTower);

                break;
            case 10:// Boss wave                
                AddPacket(5, true); AddPacket(5, true); AddPacket(5, true);

                SendPackets(1, 2, true);
                SendPackets(2, 2, true);
                SendPackets(3, 2, true);
                SendPackets(4, 2, true);
                SendPackets(5, 2, true);

                UnlockTower(2, TowerType.ArtilleryTower);
                UnlockTower(2, TowerType.SnowTower);
                UnlockTower(5, TowerType.BasicTower);
                break;
            case 11:// Tanker is comming~

                SendPackets(1, 4, true);
                SendGroup(2);

                UnlockTower(3, TowerType.LaserTower);
                UnlockTower(3, TowerType.SnowTower);
                UnlockTower(3, TowerType.ArtilleryTower);
                UnlockTower(6, TowerType.BasicTower);
                break;
            case 12:
                SendPackets(1, 3, true);
                SendPackets(2, 3, true);
                SendPackets(3, 3, true);
                SendGroup(4);


                UnlockTower(6, TowerType.LaserTower);
                UnlockTower(4, TowerType.SnowTower);
                UnlockTower(4, TowerType.ArtilleryTower);
                UnlockTower(7, TowerType.BasicTower);
                UnlockTower(8, TowerType.BasicTower);

                break;
            case 13:
                SendPackets(3, 3, true);
                SendPackets(4, 3, true);
                SendPackets(2, 2, true);

                UnlockTower(4, TowerType.LaserTower);
                UnlockTower(5, TowerType.LaserTower);

                break;
            case 14:
                SendPackets(1, 3, true);
                SendPackets(2, 4, true);
                SendPackets(5, 1, true);

                UnlockTower(9, TowerType.BasicTower);
                UnlockTower(10, TowerType.BasicTower);
                UnlockTower(11, TowerType.BasicTower);

                break;
            case 15:// pepe
                SendGroup(5);
                SendPackets(3, 2, true);
                SendPackets(4, 3, true);

                UnlockTower(5, TowerType.ArtilleryTower);
                UnlockTower(6, TowerType.ArtilleryTower);
                UnlockTower(7, TowerType.LaserTower);
                UnlockTower(5, TowerType.SnowTower);
                break;
            case 16:
                SendPackets(1, 4, true);
                SendPackets(2, 4, true);
                SendPackets(3, 4, true);

                UnlockTower(12, TowerType.BasicTower);
                UnlockTower(13, TowerType.BasicTower);
                UnlockTower(14, TowerType.BasicTower);
                UnlockTower(15, TowerType.BasicTower);
                UnlockTower(8, TowerType.LaserTower);
                UnlockTower(9, TowerType.LaserTower);
                UnlockTower(6, TowerType.SnowTower);
                break;

            case 17:
                SendPackets(3, 5, true);
                SendPackets(4, 3, true);
                SendPackets(5, 3, true);

                UnlockTower(16, TowerType.BasicTower);
                UnlockTower(17, TowerType.BasicTower);
                UnlockTower(18, TowerType.BasicTower);
                UnlockTower(19, TowerType.BasicTower);
                UnlockTower(7, TowerType.ArtilleryTower);
                UnlockTower(8, TowerType.ArtilleryTower);
                UnlockTower(10, TowerType.LaserTower);
                UnlockTower(11, TowerType.LaserTower);
                UnlockTower(7, TowerType.SnowTower);
                UnlockTower(8, TowerType.SnowTower);
                UnlockTower(9, TowerType.SnowTower);
                break;

            case 18:

                SendPackets(1, 5, true);
                SendPackets(2, 5, true);
                SendPackets(4, 5, true);

                UnlockTower(12, TowerType.LaserTower);
                UnlockTower(13, TowerType.LaserTower);
                UnlockTower(14, TowerType.LaserTower);
                UnlockTower(9, TowerType.ArtilleryTower);
                UnlockTower(10, TowerType.ArtilleryTower);
                UnlockTower(10, TowerType.SnowTower);
                UnlockTower(11, TowerType.SnowTower);
                UnlockTower(12, TowerType.SnowTower);
                break;

            case 19:

                SendPackets(1, 5, true);
                SendPackets(2, 5, true);
                SendPackets(3, 5, true);
                SendPackets(4, 2, true);

                UnlockTower(22, TowerType.BasicTower);
                UnlockTower(23, TowerType.BasicTower);
                UnlockTower(24, TowerType.BasicTower);
                UnlockTower(25, TowerType.BasicTower);
                UnlockTower(11, TowerType.ArtilleryTower);
                UnlockTower(15, TowerType.LaserTower);
                UnlockTower(13, TowerType.SnowTower);
                break;

            case 20:
                SendAllGroups();


                UnlockTower(20, TowerType.BasicTower);
                UnlockTower(21, TowerType.BasicTower);
                UnlockTower(12, TowerType.ArtilleryTower);
                UnlockTower(13, TowerType.ArtilleryTower);
                UnlockTower(16, TowerType.LaserTower);
                UnlockTower(14, TowerType.SnowTower);
                break;

            case 21:

                UpgradePacketGroup(1, "1", true);
                UpgradePacketGroup(1, "3", true);
                UpgradePacketGroup(2, "1", true);
                UpgradePacketGroup(2, "2", true);

                SendGroup(1);
                SendGroup(2);

                UnlockTower(14, TowerType.ArtilleryTower);
                UnlockTower(15, TowerType.SnowTower);
                UnlockTower(17, TowerType.LaserTower);
                break;

            case 22:

                UpgradePacketGroup(1, "3", true);
                UpgradePacketGroup(1, "3", true);
                UpgradePacketGroup(2, "3", true);
                UpgradePacketGroup(2, "3", true);
                UpgradePacketGroup(3, "3", true);
                UpgradePacketGroup(3, "3", true);


                SendGroup(1);
                SendGroup(2);
                SendGroup(3);


                UnlockTower(26, TowerType.BasicTower);
                UnlockTower(27, TowerType.BasicTower);
                UnlockTower(28, TowerType.BasicTower);
                UnlockTower(29, TowerType.BasicTower);
                UnlockTower(18, TowerType.LaserTower);
                UnlockTower(16, TowerType.SnowTower);
                UnlockTower(17, TowerType.SnowTower);
                break;

            case 23:
                UpgradePacketGroup(2, "1", true);
                UpgradePacketGroup(2, "3", true);
                UpgradePacketGroup(2, "1", true);
                UpgradePacketGroup(3, "1", true);
                UpgradePacketGroup(3, "1", true);
                UpgradePacketGroup(4, "4", true);
                UpgradePacketGroup(4, "7", true);

                SendGroup(2);
                SendGroup(3);
                SendGroup(4);

                UnlockTower(30, TowerType.BasicTower);
                UnlockTower(31, TowerType.BasicTower);
                UnlockTower(32, TowerType.BasicTower);
                UnlockTower(33, TowerType.BasicTower);
                UnlockTower(15, TowerType.ArtilleryTower);
                UnlockTower(16, TowerType.ArtilleryTower);
                UnlockTower(19, TowerType.LaserTower);
                UnlockTower(18, TowerType.SnowTower);
                UnlockTower(19, TowerType.SnowTower);
                break;

            case 24:
                UpgradePacketGroup(4, "1", true);
                UpgradePacketGroup(4, "3", true);
                UpgradePacketGroup(4, "3", true);
                UpgradePacketGroup(4, "4", true);
                UpgradePacketGroup(4, "7", true);
                UpgradePacketGroup(2, "3", true);
                SendGroup(1);
                SendGroup(2);
                SendGroup(3);
                SendGroup(4);

                UnlockTower(38, TowerType.BasicTower);
                UnlockTower(39, TowerType.BasicTower);
                UnlockTower(40, TowerType.BasicTower);
                UnlockTower(41, TowerType.BasicTower);
                UnlockTower(17, TowerType.ArtilleryTower);
                UnlockTower(18, TowerType.ArtilleryTower);
                break;

            case 25:
                UpgradePacketGroup(3, "1", true);
                UpgradePacketGroup(3, "3", true);
                UpgradePacketGroup(5, "1", true);
                UpgradePacketGroup(5, "2", true);
                UpgradePacketGroup(5, "3", true);
                UpgradePacketGroup(5, "4", true);
                UpgradePacketGroup(5, "5", true);
                UpgradePacketGroup(5, "6", true);
                UpgradePacketGroup(5, "7", true);

                SendGroup(3);
                SendGroup(5);

                UnlockTower(34, TowerType.BasicTower);
                UnlockTower(35, TowerType.BasicTower);
                UnlockTower(36, TowerType.BasicTower);
                UnlockTower(37, TowerType.BasicTower);

                break;
            case 26:

                UpgradePacketGroup(1, "1", true);
                UpgradePacketGroup(5, "1", true);
                SendGroup(1);
                SendGroup(5);

                break;
            case 27:
                UpgradePacketGroup(5, "1", true);
                UpgradePacketGroup(5, "2", true);
                UpgradePacketGroup(5, "3", true);
                UpgradePacketGroup(5, "4", true);
                UpgradePacketGroup(5, "5", true);
                UpgradePacketGroup(5, "6", true);
                UpgradePacketGroup(5, "7", true);

                SendGroup(2);
                SendGroup(1);
                SendGroup(5);

                break;
            case 28:
                UpgradePacketGroup(1, "1", true);
                UpgradePacketGroup(1, "3", true);
                UpgradePacketGroup(1, "1", true);
                UpgradePacketGroup(1, "3", true);
                UpgradePacketGroup(2, "2", true);
                UpgradePacketGroup(2, "2", true);
                UpgradePacketGroup(2, "3", true);
                UpgradePacketGroup(3, "1", true);
                UpgradePacketGroup(3, "1", true);
                UpgradePacketGroup(3, "3", true);
                UpgradePacketGroup(3, "3", true);
                UpgradePacketGroup(4, "1", true);
                UpgradePacketGroup(4, "3", true);
                UpgradePacketGroup(4, "4", true);
                UpgradePacketGroup(4, "5", true);
                UpgradePacketGroup(4, "6", true);
                UpgradePacketGroup(4, "4", true);
                UpgradePacketGroup(4, "5", true);
                UpgradePacketGroup(4, "6", true);
                UpgradePacketGroup(4, "7", true);
                UpgradePacketGroup(4, "7", true);

                SendGroup(2);
                SendGroup(3);
                SendGroup(4);


                break;
            case 29:
                UpgradePacketGroup(1, "1", true);
                UpgradePacketGroup(1, "3", true);
                UpgradePacketGroup(2, "1", true);
                UpgradePacketGroup(2, "1", true);
                UpgradePacketGroup(4, "1", true);
                UpgradePacketGroup(4, "4", true);

                UpgradePacketGroup(2, "2", true);
                UpgradePacketGroup(2, "2", true);
                UpgradePacketGroup(2, "3", true);

                SendGroup(1);
                SendGroup(2);
                SendGroup(3);
                SendGroup(4);

                break;
            case 30:
                UpgradePacketGroup(4, "1", true);
                UpgradePacketGroup(4, "3", true);
                UpgradePacketGroup(4, "5", true);
                UpgradePacketGroup(4, "4", true);
                UpgradePacketGroup(5, "1", true);
                UpgradePacketGroup(5, "2", true);
                UpgradePacketGroup(5, "3", true);
                UpgradePacketGroup(5, "4", true);
                UpgradePacketGroup(5, "5", true);
                UpgradePacketGroup(5, "6", true);
                UpgradePacketGroup(5, "7", true);
                SendAllGroups();
                break;
            case 31:
                UpgradePacketGroup(1, "1", true);
                UpgradePacketGroup(1, "3", true);

                UpgradePacketGroup(3, "1", true);
                UpgradePacketGroup(3, "3", true);
                SendGroup(1);
                SendGroup(3);
                break;
            case 32:
                UpgradePacketGroup(4, "5", true);
                UpgradePacketGroup(4, "1", true);
                UpgradePacketGroup(4, "3", true);
                UpgradePacketGroup(4, "4", true);
                UpgradePacketGroup(2, "1", true);
                UpgradePacketGroup(2, "2", true);
                UpgradePacketGroup(2, "3", true);

                SendGroup(1);
                SendGroup(2);
                SendGroup(4);


                break;
            case 33:

                UpgradePacketGroup(4, "1", true);
                UpgradePacketGroup(4, "3", true);
                UpgradePacketGroup(4, "5", true);
                UpgradePacketGroup(5, "1", true);
                UpgradePacketGroup(5, "2", true);
                SendGroup(4);
                SendGroup(5);
                break;
            case 34:
                UpgradePacketGroup(2, "2", true);
                UpgradePacketGroup(5, "3", true);
                UpgradePacketGroup(5, "4", true);
                UpgradePacketGroup(5, "5", true);
                UpgradePacketGroup(5, "6", true);
                UpgradePacketGroup(5, "7", true);
                SendGroup(5);

                break;
            case 35:
                UpgradePacketGroup(1, "1", true);
                UpgradePacketGroup(1, "3", true);
                SendGroup(1);
                break;
            case 36:
                UpgradePacketGroup(2, "1", true);
                UpgradePacketGroup(2, "2", true);
                UpgradePacketGroup(2, "3", true);
                SendGroup(2);
                break;
            case 37:
                UpgradePacketGroup(3, "1", true);
                UpgradePacketGroup(3, "3", true);
                SendGroup(3);
                break;
            case 38:
                UpgradePacketGroup(4, "1", true);
                UpgradePacketGroup(4, "3", true);
                UpgradePacketGroup(4, "4", true);
                UpgradePacketGroup(4, "5", true);
                UpgradePacketGroup(4, "6", true);
                UpgradePacketGroup(4, "7", true);

                SendGroup(4);
                break;
            case 39:
                UpgradePacketGroup(5, "1", true);
                UpgradePacketGroup(5, "2", true);
                UpgradePacketGroup(5, "3", true);
                UpgradePacketGroup(5, "4", true);
                UpgradePacketGroup(5, "5", true);
                UpgradePacketGroup(5, "6", true);
                UpgradePacketGroup(5, "7", true);
                SendGroup(5);
                break;

            case 40:

                SendAllGroups();
                break;
            default:
                Debug.Log("Sorry: Out of waves!");
                break;
        }
    }

    /** STANDARD UNITY METHODS **/

    void Awake()
    {
        if ((packMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<PacketManager>()) == null)
            Debug.Log("Error: Failed to find PacketManager in Game Logic");
        if ((aiGroups.group1 = packMan.GetPacketGroup(0, "AI")) == null)
            Debug.Log("Error: Failed to grab aiPacket group from PacketManager in CompAI");
        if ((aiGroups.group2 = packMan.GetPacketGroup(1, "AI")) == null)
            Debug.Log("Error: Failed to grab aiPacket group from PacketManager in CompAI");
        if ((aiGroups.group3 = packMan.GetPacketGroup(2, "AI")) == null)
            Debug.Log("Error: Failed to grab aiPacket group from PacketManager in CompAI");
        if ((aiGroups.group4 = packMan.GetPacketGroup(3, "AI")) == null)
            Debug.Log("Error: Failed to grab aiPacket group from PacketManager in CompAI");
        if ((aiGroups.group5 = packMan.GetPacketGroup(4, "AI")) == null)
            Debug.Log("Error: Failed to grab aiPacket group from PacketManager in CompAI");

        int idNum = 1;
        foreach (GameObject tower in aiTowers.basicTowers)
        {
            tower.SetActive(false);
            TowerController towerController = tower.GetComponent<TowerController>();
            towerController.towerID = "basic" + idNum;
            towerController.numericID = idNum;
            ++idNum;
            aiTowers.basicCons.Add(towerController);
        }

        idNum = 1;
        foreach (GameObject tower in aiTowers.laserTowers)
        {
            tower.SetActive(false);
            TowerController towerController = tower.GetComponent<TowerController>();
            towerController.towerID = "laser" + idNum;
            towerController.numericID = idNum;
            ++idNum;
            aiTowers.laserCons.Add(towerController);
        }

        idNum = 1;
        foreach (GameObject tower in aiTowers.artilleryTowers)
        {
            tower.SetActive(false);
            TowerController towerController = tower.GetComponent<TowerController>();
            towerController.towerID = "artillery" + idNum;
            towerController.numericID = idNum;
            ++idNum;
            aiTowers.artilleryCons.Add(towerController);
        }

        idNum = 1;
        foreach (GameObject tower in aiTowers.aoeTowers)
        {
            tower.SetActive(false);
            TowerController towerController = tower.GetComponent<TowerController>();
            towerController.towerID = "aoe" + idNum;
            towerController.numericID = idNum;
            ++idNum;
            aiTowers.aoeCons.Add(towerController);
        }
    }
}
