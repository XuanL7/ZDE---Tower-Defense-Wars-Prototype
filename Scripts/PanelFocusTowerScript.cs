using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class TowerInfo
{
    public Text towerType;
    public Text damage;
    public Text armor;
    public Text bSpeed;
    public Text bRange;
    public Text reload;

    public Text damageCost;
    public Text armorCost;
    public Text bulletSpeedCost;
    public Text bulletRangeCost;
    public Text reloadCost;

    public Button dmgCostBtn;
    public Button armorCostBtn;
    public Button bltSpdCostBtn;
    public Button bltRngCostBtn;
    public Button rldCostBtn;

    public Text damageTitleText;
    public Text rangeTitleText;
}

public class PanelFocusTowerScript : MonoBehaviour {

    public TowerInfo towerInfo;

    private UpgradeManager upMan;
    private TowerController tower;
    private int mask = 1 << 9;
    Ray ray;

    public void buyDamage()
    {
        upMan.UpgradeTowerDamage(tower);
    }

    public void buyArmor()
    {
        upMan.UpgradeTowerArmor(tower);
    }

    public void buyBulletSpeed()
    {
        upMan.UpgradeTowerBulletSpeed(tower);
    }

    public void buyBulletRange()
    {
        upMan.UpgradeTowerBulletRange(tower);
    }

    public void buyReloadSpeed()
    {
        if (tower.reloadTime >= 0.2)
            upMan.UpgradeTowerReload(tower);
    }

    void setStats()
    {
        if (tower == null)
        {
            towerInfo.towerType.text = "";
            towerInfo.damage.text = "";
            towerInfo.armor.text = "";
            towerInfo.bSpeed.text = "";
            towerInfo.bRange.text = "";
            towerInfo.reload.text = "";

            towerInfo.damageCost.text = "";
            towerInfo.armorCost.text = "";
            towerInfo.bulletSpeedCost.text = "";
            towerInfo.bulletRangeCost.text = "";
            towerInfo.reloadCost.text = "";

            towerInfo.dmgCostBtn.interactable = false;
            towerInfo.armorCostBtn.interactable = false;
            towerInfo.bltSpdCostBtn.interactable = false;
            towerInfo.bltRngCostBtn.interactable = false;
            towerInfo.rldCostBtn.interactable = false;
        }
        else
        {
            if (tower.DefaltType == TowerType.SnowTower)
            {
                towerInfo.damageTitleText.text = "Slow";
                towerInfo.rangeTitleText.text = "Slow Duration";
            }
            else
            {
                towerInfo.damageTitleText.text = "Damage";
                towerInfo.rangeTitleText.text = "Weapon Range";
            }

            towerInfo.towerType.text = tower.Name;
            towerInfo.damage.text = tower.towerDamage.ToString();
            towerInfo.armor.text = tower.armorPierce.ToString();
            towerInfo.bSpeed.text = tower.bulletSpeed.ToString();
            towerInfo.bRange.text = tower.bulletRange.ToString();
            towerInfo.reload.text = tower.reloadTime.ToString();

            towerInfo.damageCost.text = "$" + upMan.damageCost(tower).ToString();
            towerInfo.armorCost.text = "$" + upMan.ArmorCost(tower).ToString();
            towerInfo.bulletSpeedCost.text = "$" + upMan.BulletSpeedCost(tower).ToString();
            towerInfo.bulletRangeCost.text = "$" + upMan.BulletRangeCost(tower).ToString();
            towerInfo.reloadCost.text = "$" + upMan.ReloadCost(tower).ToString();
        }
    }

    private bool Upgradeable(int cost)
    {
        if (cost <= upMan.econ.towerCash && cost > 0)
            return true;
        return false;
    }

    private void SetInteractiveButtons()
    {
        if (tower != null)
        {
            towerInfo.dmgCostBtn.interactable = Upgradeable(upMan.damageCost(tower));
            towerInfo.armorCostBtn.interactable = Upgradeable(upMan.ArmorCost(tower));
            towerInfo.bltSpdCostBtn.interactable = Upgradeable(upMan.BulletSpeedCost(tower));
            towerInfo.bltRngCostBtn.interactable = Upgradeable(upMan.BulletRangeCost(tower));
            towerInfo.rldCostBtn.interactable = Upgradeable(upMan.ReloadCost(tower));
        }
    }

    void Awake()
    {
        if ((upMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<UpgradeManager>()) == null)
            Debug.Log("Error: Failed to load UpgradeManager from GameLogic in PanelFocusTower!");

        setStats();
    }

    void Update()
    {
        if (Camera.main != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.collider.tag == "Interface Collider" && Input.GetMouseButtonDown(0))
                {
                      tower = hit.collider.GetComponentInParent<TowerController>();
                      setStats();
                      SetInteractiveButtons();
                }
            }
        }
       
    }

    void FixedUpdate()
    {
        setStats();
        SetInteractiveButtons();
    }
}
