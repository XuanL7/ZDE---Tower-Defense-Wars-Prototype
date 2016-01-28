using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TowerType
{
    BasicTower,
    LaserTower,
    ArtilleryTower,
    SnowTower,
    WallTower
}

public class TowerController : MonoBehaviour
{
    /**          PUBLIC MEMBERS            **/
    [Header("Tower setup")]
    public TowerType DefaltType;
    public Transform hub;                   // Turrent hub
    public Transform barrel;                // Turrent barrel
    public ParticleSystem muzzleFlash;
    public Transform[] muzzlePositions;     // array for multiple positions
    public GameObject projectile; // the type of projectile used

    // Base stats of tower:
    [Header("Tower stats ")]
    public float towerDamage = 2;
    public float armorPierce = 0;
    public float bulletSpeed = 20f;
    public float bulletRange = 2f;
    public float reloadTime = 1f;
    public float fireTime = 0; // Depricated!


    //Value for cost to upgrade tower stat
    [Header("Tower cost")]
    public int cost = 50; // price to purchase this tower

    public int damageCost = 10;
    public int armorCost = 0;
    public int bulletSpeedCost = 10;
    public int bulletRangeCost = 10;
    public int reloadSpeedCost = 10;
    [Header("Tower upgrade stats")]
    public int upDamageCount = 1;
    public int upArmorPierceCount = 1;
    public int upBulletSpeedCount = 1;
    public int upbulletRangeCount = 1;
    public int upFireTimeCount = 1;
    public int upReloadSpeed = 1;
    [Header("MAX upgrade stats")]
    public int maxUpgradeDamage = 10;
    public int maxUpgradeArmor = 0;
    public int maxUpgradeBSpeed = 5;
    public int maxUpgradeBRange = 5;
    public int maxUpgradeReload = 3;

    //[HideInInspector]
    public int numericID = 0;
    //[HideInInspector]
    public string towerID = "";

    /**          PRIVATE  MEMBERS         **/
    private string _name = "Frag Turret";
    private List<Transform> targets;
    private Transform currTarget;
    private bool firing = false;
    private Light towerLight; // Lighting effect on object


    /**         GETTERS/SETTERS           **/
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    /**      PUBLIC UTILITY METHODS       **/

    public void MultiplyStats(float damage, float armorPen)
    {
        // REVISION: Temp fix to allow snow towr adjustment!
        if (DefaltType == TowerType.SnowTower)
            towerDamage += damage;
        else
            towerDamage *= damage;
        armorPierce *= armorPen;
    }

    public void ModifyStats(float speed, float range, float reloadSpeed, float fireRate)
    {
        bulletSpeed += speed;
        bulletRange += range;
        if (DefaltType == TowerType.LaserTower)
            this.GetComponent<SphereCollider>().radius += range;
        reloadTime += reloadSpeed;
        fireTime += fireRate;
    }

    public void Enable()
    {
        this.gameObject.SetActive(false);
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }

    public void SetLightColor(Color color)
    {
        if (towerLight != null)
        {
            towerLight.color = color;
        }
    }

    /**      PRIVATE UTILITY METHODS     **/
    void Fire()
    {
        switch(DefaltType)
        {
            case TowerType.BasicTower:
                StartCoroutine(BulletTypeFire());
                break;
            case TowerType.ArtilleryTower:
                projectile.GetComponent<ProjectileController>().SetBulletStats(bulletSpeed, bulletRange, towerDamage, armorPierce);
                projectile.transform.position = currTarget.transform.position - Random.insideUnitSphere;
                projectile.SetActive(true);
               
                break;
            case TowerType.LaserTower:
                projectile.SetActive(true);
                NonProjectileDamage();
                break;
            case TowerType.SnowTower:
               NonProjectileDamage();
                break;
        }
    }

    IEnumerator BulletTypeFire()
    {
        foreach (Transform muzzlePos in muzzlePositions)
        {
            GameObject bullet = BulletPool.current.GetPooledBullet();
            if (bullet != null)
            {
                bullet.GetComponent<ProjectileController>().SetBulletStats(bulletSpeed, bulletRange, towerDamage, armorPierce);
                bullet.transform.position = muzzlePos.position;
                bullet.transform.rotation = muzzlePos.rotation;
                bullet.SetActive(true);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    void NonProjectileDamage()
    {
        PacketController pack;
        if((pack = currTarget.GetComponent<PacketController>() ) != null)
        {
            switch (DefaltType)
            {
                case TowerType.LaserTower:
                    pack.EnergyType();
                    pack.TakeDamage(towerDamage, armorPierce);
                    break;
                case TowerType.SnowTower:
                    gameObject.GetComponentInChildren<ParticleSystem>().Play();
                    gameObject.GetComponent<AudioSource>().Play();
                    foreach(Transform target in targets){
                        if (target.gameObject.activeSelf)
                            target.GetComponent<PacketController>().TakeDebuff(TowerType.SnowTower, towerDamage, bulletRange);
                    }
                    break;

            }

        }
    }

    /// <summary>
    /// ProjectVectorOnPlane, used by TrackTarget
    /// </summary>
    /// <param name="planeNormal"></param>
    /// <param name="vector"></param>
    /// <returns>void</returns>
    Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
    {
        return vector - (Vector3.Dot(vector, planeNormal) * planeNormal);
    }

    /// <summary>
    /// TrackTarget: point turrent hub and barrel in relation to target
    /// </summary>
    /// <param name="target"></param>
    /// <returns>bool: representing tacked success on target</returns>
    private bool TrackTarget(Transform target)
    {
        if (hub != null && target != null && target.gameObject.activeSelf == true)
        {
            Vector3 headingVector = ProjectVectorOnPlane(hub.up, target.transform.position - hub.position);
            Quaternion newHubRotation = Quaternion.LookRotation(headingVector);

            // Apply heading rotation
            hub.rotation = Quaternion.Slerp(hub.rotation, newHubRotation, Time.deltaTime * 10f);

            if (barrel != null)
            {
                Vector3 elevationVector = ProjectVectorOnPlane(hub.right, target.transform.position - barrel.position);
                Quaternion newBarrelRotation = Quaternion.LookRotation(elevationVector);

                // Apply elevation rotation
                barrel.rotation = Quaternion.Slerp(barrel.rotation, newBarrelRotation, Time.deltaTime * 10f);
            }
            return true;
        }
        return false;
    }

    private void GetTarget()
    {
        if (targets.Count > 0)
            currTarget = targets[0];
        else
            currTarget = null;
    }

    private void RemoveKilledTargets()
    {
        for (int i = targets.Count - 1; i >= 0; --i)
            if (!targets[i].gameObject.activeSelf)
                targets.RemoveAt(i);
    }

    void Awake()
    {
        if ((towerLight = this.GetComponentInChildren<Light>()) == null)
            Debug.Log("Error: Missing Light component on tower - " + _name);
        targets = new List<Transform>();
    }

    void FixedUpdate()
    {
        RemoveKilledTargets();
        GetTarget();

        if (TrackTarget(currTarget))
        {
            if (!firing)
            {
                switch (DefaltType)
                {
                    case TowerType.BasicTower:
                        InvokeRepeating("Fire", reloadTime, reloadTime);
                        break;
                    case TowerType.LaserTower:
                        InvokeRepeating("Fire", reloadTime, reloadTime);
                        break;
                    case TowerType.SnowTower:
                        InvokeRepeating("Fire",0,5);
                        break;
                    case TowerType.ArtilleryTower:
                        InvokeRepeating("Fire", 0, reloadTime);
                        break;
                }

                    firing = true;
                
            
            }
        }
        else
        {
            CancelInvoke("Fire");
            firing = false;
            switch(DefaltType)
            {
                case TowerType.LaserTower:
                CancelInvoke("LaserDamage");
                projectile.SetActive(false);
                break;
                case TowerType.SnowTower:
                gameObject.GetComponentInChildren<ParticleSystem>().Stop();
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != this.gameObject.tag && (other.tag == "Player" || other.tag == "AI"))
        {
            targets.Add(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        targets.Remove(other.transform);
    }
}