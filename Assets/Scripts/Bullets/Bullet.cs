using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [System.Serializable]
    public struct BulletLevel
    {
        public bool destroyOnCollision; 
        public float speed;
        public float damage;

        public float fireRate;
        public int numBullets;
        public float bulletSpacing;
        public float angle;
    }

    [SerializeField] protected string name;
    [SerializeField] protected List<BulletLevel> levels;
    [SerializeField] protected GameObject particle;
    [SerializeField] protected GameObject healthPack;
    public float healthPackProbability = 0.25f;
    protected PlayerController owner = null;
    protected int currentLevel = 0;

    private BulletLevel powerUp;

    void Awake()
    {
        SoundManager.Instance.Play(name);
    }

    // Update is called once per frame
    void Update()
    {
        if (owner)
            transform.position += transform.forward * (levels[currentLevel].speed + owner.pawn.powerUp.speed) * Time.unscaledDeltaTime;
        else
            transform.position += transform.forward * levels[currentLevel].speed * Time.deltaTime;
            
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    // Return bullet's fire rate
    public float GetFireRate(int currentLevel = 0)
    {
        currentLevel = currentLevel >= (levels.Count-1) ? (levels.Count-1) : currentLevel;
        return levels[currentLevel].fireRate;
    }

    // Return number of bullets
    public int GetNumBullets(int currentLevel = 0)
    {
        currentLevel = currentLevel >= (levels.Count-1) ? (levels.Count-1) : currentLevel;
        return levels[currentLevel].numBullets;
    }

    // return bullet spacing
    public float GetBulletSpacing(int currentLevel = 0)
    {
        currentLevel = currentLevel >= (levels.Count-1) ? (levels.Count-1) : currentLevel;
        return levels[currentLevel].bulletSpacing;
    }

    // return bullet angle
    public float GetBulletAngle(int currentLevel = 0)
    {
        currentLevel = currentLevel >= (levels.Count-1) ? (levels.Count-1) : currentLevel;
        return levels[currentLevel].angle;
    }

    // Set the ownwe of the bullet
    public void SetOwner(PlayerController playerController, BulletLevel _powerUp, int bulletLevel = 0)
    {
        owner = playerController;
        currentLevel = bulletLevel >= (levels.Count-1) ? (levels.Count-1) : bulletLevel;
        powerUp = _powerUp;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.GetComponent<Pawn>() || col.transform.GetComponent<Bullet>())
        {
            int level = 0;
            EnemyController enemy  = null;
            if (owner)
            {
                level = owner.CurrentLevel;
                enemy = col.transform.GetComponent<EnemyController>();
            }
            level = level >= (levels.Count-1) ? (levels.Count-1) : level;

            float hp = 1;
            if (col.transform.GetComponent<Pawn>()) {
                if (owner)
                    hp = col.transform.GetComponent<Pawn>().TakeDamage(levels[level].damage + owner.pawn.powerUp.damage);
                else
                    hp = col.transform.GetComponent<Pawn>().TakeDamage(levels[level].damage);

                if (hp <= 0 && owner) {

                    // Getting player HP percentage
                    float playerHp = owner.GetPawn().GetHP();

                    // Getting random float between 0 and 1
                    System.Random rand = new System.Random();
                    float randomFloat = (float)rand.NextDouble();

                    // Getting "probability" of lower health
                    float healthP = (1 - playerHp);

                    // Using Bays to calculate probability of dropping health pack
                    if (randomFloat <= ((healthP * healthPackProbability) / healthP)) {
                        if (healthPack) {
                            Instantiate(healthPack, transform.position, transform.rotation);
                        }
                        
                    }        
                }
            }
                

            if (owner != null && hp <= 0.0f && enemy != null)
                owner.AddPoints(enemy.Score);

            if (levels[level].destroyOnCollision)
            {
                if (particle != null)
                    Instantiate(particle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
