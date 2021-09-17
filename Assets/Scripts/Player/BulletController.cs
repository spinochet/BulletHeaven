using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private GameObject bulletPrefab;
    private GameObject burstPrefab;

    // Bullet
    private bool isShooting;
    private float fireRate;
    private float fireTimer;

    // Burst
    private BurstBullet burst;
    private bool isBurst;

    // Initiate variables
    public void Setup(GameObject _bulletPrefab, GameObject _burstPrefab)
    {
        bulletPrefab = _bulletPrefab;
        burstPrefab = _burstPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.unscaledDeltaTime;

        if (isShooting && !isBurst)
        {
            if (fireTimer > 1.0f / fireRate)
            {
                fireTimer = 0.0f;
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
        }
    }

    // Start shooting
    public void StartShooting()
    {
        if (fireTimer > 1.0f / fireRate)
        {
            fireTimer = 0.0f;
            Instantiate(bulletPrefab, transform.position, transform.rotation);
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
