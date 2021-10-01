using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : Controller
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject burstPrefab;
    [SerializeField] private bool isTimeScale;

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

    // Update is called once per frame
    void Update()
    {
        if (!IsPaused())
        {
            if (isTimeScale)
                fireTimer += Time.deltaTime;
            else
                fireTimer += Time.unscaledDeltaTime;

            if (isShooting && !isBurst)
            {
                if (fireTimer > 1.0f / bullet.GetFireRate())
                {
                    fireTimer = 0.0f;
                    GameObject b = Instantiate(bullet.gameObject, transform.position, transform.rotation);
                    b.GetComponent<Bullet>().SetOwner(GetComponent<StatsController>());
                }
            }
        }
    }

    // Start shooting
    public void StartShooting()
    {
        if (!IsPaused())
        {
            if (fireTimer > 1.0f / bullet.GetFireRate())
            {
                fireTimer = 0.0f;
                GameObject b = Instantiate(bulletPrefab, transform.position, transform.rotation);
            }

            isShooting = true;
        }
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
