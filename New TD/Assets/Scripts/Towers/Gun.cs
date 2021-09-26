using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fireRate;
    public GameObject bullet;

    public string enemyTag;

    void Start()
    {
        InvokeRepeating("Shoot", 0f, fireRate);
    }

    void Shoot()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies.Length <= 0)
            return;

        if (PlayerStats.money <= 0)
            return;

        PlayerStats.money -= 1;
        Instantiate(bullet, transform);
    }
}
