using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour {

    public static BulletPool current;
    public GameObject pooledBullet;
    public int pooledAmount = 4;
    public bool willGrow = true;

    List<GameObject> pooledBullets;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        pooledBullets = new List<GameObject>();
        for (int i = 0; i < pooledAmount; ++i)
        {
            GameObject obj = (GameObject)Instantiate(pooledBullet);
            obj.SetActive(false);
            pooledBullets.Add(obj);
        }
    }

    public GameObject GetPooledBullet()
    {
        foreach (GameObject bullet in pooledBullets)
            if (!bullet.activeSelf)
                return bullet;

        if (willGrow)
        {
            GameObject bullet = Instantiate(pooledBullet) as GameObject;
            pooledBullets.Add(bullet);
            return bullet;
        }

        return null;
    }
}
