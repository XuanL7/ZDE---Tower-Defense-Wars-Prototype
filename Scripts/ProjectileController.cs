using UnityEngine;
using System.Collections;


public enum ProjectileType
{
    Basic,
    Laser,
    Artillery,
    SnowTower
}

public class ProjectileController : MonoBehaviour {

    [Header("Projectile Type")]
    public ProjectileType DefaultType;
    
    [Header("Projectile status")]
    public float bulletSpeed = 20.0f;
    public float bulletLife = 2f;
    private float bulletDamage = 6;
    private float armorPierce = 0;
    public ParticleSystem artillyEffect;
    private bool hashit = false;
    
    public void SetBulletStats(float speed, float range, float damage, float armorPenetration)
    {
        bulletSpeed = speed;
        bulletLife = range;
        if (DefaultType == ProjectileType.Artillery)
           artillyEffect.startSize = bulletSpeed;
        bulletDamage = damage;
        armorPierce = armorPenetration;
    }

    void Destroy()
    {

        gameObject.SetActive(false);
        hashit = false;
        
    }
    void OnEnable()
    {
        this.gameObject.layer = 1 << 1;
        switch (DefaultType) { 
            case ProjectileType.Basic:
                Invoke("Destroy", bulletLife);
                break;
            case ProjectileType.Artillery:
                gameObject.GetComponent<SphereCollider>().radius = 0.1f;
                Invoke("Destroy", 1.6f);
                break;
            case ProjectileType.Laser:
                break;
            case ProjectileType.SnowTower:
                break;
            
        }
    }
    void OnDisable()
    {
        CancelInvoke("Destroy");
    }

    void FixedUpdate()
    {
        switch (DefaultType)
        {
            case ProjectileType.Basic:
                transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
                break;
            case ProjectileType.Artillery:
                if (gameObject.GetComponent<SphereCollider>().radius < bulletSpeed)
                    gameObject.GetComponent<SphereCollider>().radius = gameObject.GetComponent<SphereCollider>().radius + 0.05f;
                break;
            case ProjectileType.Laser:
                break;
            case ProjectileType.SnowTower:
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        PacketController packet;
        if ((packet = other.gameObject.GetComponent<PacketController>()) != null)
        {
            Mathf.Abs(armorPierce); // ensures armor piercing is valid
            switch (DefaultType)
            {
                case ProjectileType.Basic:
                    if (!hashit) {
                    packet.TakeDamage(bulletDamage, armorPierce);
                    hashit = true;
                    }
                    break;
                case ProjectileType.Artillery:
                    packet.TakeDamage(bulletDamage, armorPierce);
                    break;
                case ProjectileType.Laser:
                    break;
                case ProjectileType.SnowTower:
                    break;
            }
        }
    }
}
