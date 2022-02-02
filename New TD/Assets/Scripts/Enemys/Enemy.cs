using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float normalSpeed;
    public float nowSpeed;
    public float attack;
    public Rigidbody2D rb;
    public string towerTag;
    public Tower target;
    public GameObject dieParticles;

    public float hp;
    public bool freez;
    public float freezTime;
    
    public SpriteRenderer spriteRender;
    public Sprite freezSprite;
    public Sprite normalSprite;

    void Start()
    {
        normalSprite = spriteRender.sprite;
        nowSpeed = normalSpeed;
    }

    void Update()
    {
        if (freez)
        {
            freezTime -= Time.deltaTime;
            if (freezTime <= 0)
            {
                freezTime = 0;
                freez = false;
                nowSpeed = normalSpeed;
                spriteRender.sprite = normalSprite;
            }
        }

        if (target)
        {
            if (freez)
            {
                target.Attack(attack * Time.deltaTime);
            }
            else
            {
                target.Attack(attack * Time.deltaTime / 2);
            }
            return;
        }

        if (!target)
        {
            transform.Translate(-transform.position.normalized * nowSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (target)
            return;

        if (collision.gameObject.tag == towerTag)
        {
            target = collision.gameObject.GetComponent<Tower>();
        }
    }

    public void Attack(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            if (PlayerStats.particlesEnabled)
                Instantiate(dieParticles, transform.position, transform.rotation);
            Destroy(gameObject);
            return;
        }
    }

    public void Freeze(float time)
    {
        if (freez)
        {
            freezTime += time;
        }
        else
        {
            freez = true;
            freezTime = time;
            nowSpeed = normalSpeed / 2;
            spriteRender.sprite = freezSprite;
        }
    }
}
