using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Tower tower;
    public string enemyTag;
    public float maxDistance;
    public AnimationCurve damageFromDistance;
    public float delay;
    public GameObject explosionParticles;

    void Start()
    {
        Invoke("Explosion", delay);
    }

    private void Explosion()
    {
        tower.Attack(10000000);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < maxDistance)
            {
                Enemy enemyScript = enemy.gameObject.GetComponent<Enemy>();
                enemyScript.Attack(damageFromDistance.Evaluate(distanceToEnemy));
            }
        }
        
        if (PlayerStats.particlesEnabled)
            Instantiate(explosionParticles, transform.position, transform.rotation);
    }
}
