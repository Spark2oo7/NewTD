using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fireRate;
    public GameObject bullet;
    public Warehouse warehouse;
    public Resource resource;

    public string enemyTag;

    void Start()
    {
        InvokeRepeating("Shoot", 0f, fireRate);
    }

    void Shoot()
    {
        if (warehouse.TryReceive(resource, 1))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            if (enemies.Length <= 0)
                return;

            if (PlayerStats.money <= 0)
                return;

            warehouse.Receive(resource, 1);
            Instantiate(bullet, transform);
        }
    }
}
