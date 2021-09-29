using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject burstPrefab;

    // Controllers
    private PlayerController playerController;
    private CompanionController companionController;
    private EnemyController enemyController;

    // Bullet
    Bullet bullet;
    private bool isShooting;
    private float fireTimer;

    // Burst
    private BurstBullet burst;
    private bool isBurst;

    // Initiate variables
    void Awake()
    {
        bullet = bulletPrefab.GetComponent<Bullet>();
        // burst = burstPrefab.GetComponent<BurstBullet>();
    }

    // Assign bullet owner
    public void AssignOwner(PlayerController controller)
    {
        playerController = controller;
    }

    // Assign bullet owner
    public void AssignOwner(CompanionController controller)
    {
        companionController = controller;
    }

    // Assign bullet owner
    public void AssignOwner(EnemyController controller)
    {
        enemyController = controller;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.unscaledDeltaTime;

        if (isShooting && !isBurst)
        {
            if (fireTimer > 1.0f / bullet.GetFireRate())
            {
                fireTimer = 0.0f;
                GameObject b = Instantiate(bullet.gameObject, transform.position, transform.rotation);

                if (playerController != null)
                    b.GetComponent<Bullet>().AssignOwner(playerController);
                else if (companionController != null)
                    b.GetComponent<Bullet>().AssignOwner(companionController);
                else if (enemyController != null)
                    b.GetComponent<Bullet>().AssignOwner(enemyController);
            }
        }
    }

    // Start shooting
    public void StartShooting()
    {
        if (fireTimer > 1.0f / bullet.GetFireRate())
        {
            fireTimer = 0.0f;
            GameObject b = Instantiate(bulletPrefab, transform.position, transform.rotation);
        }

        isShooting = true;
    }

    // Stop shooting
    public void StopShooting()
    {
        isShooting = false;
    }

    // Start burst
    public void StartBurst()
    {
        GameObject bulletObj = Instantiate(burstPrefab, transform.position, transform.rotation, transform);
        bulletObj.transform.localPosition = Vector3.zero;

        burst = bulletObj.GetComponent<BurstBullet>();
        isBurst = burst.Activate();
    }

    // Stop burst
    public void StopBurst()
    {
        burst.Deactivate();
        isBurst = false;
    }
}
