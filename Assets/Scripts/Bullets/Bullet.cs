using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private bool destroyOnCollision = true;
    [SerializeField] private bool isEnemyBullet = false;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float fireRate = 30.0f;

    private StatsController owner;

    // Update is called once per frame
    void Update()
    {
        if (isEnemyBullet)
            transform.position += transform.forward * speed * Time.deltaTime;
        else
            transform.position += transform.forward * speed * Time.unscaledDeltaTime;
            
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    // Return bullet's fire rate
    public float GetFireRate()
    {
        return fireRate;
    }

    // Set owner after firing
    public void SetOwner(StatsController _owner)
    {
        owner = _owner;
    }

    // FIX AFTER SPRINT
    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.GetComponent<StatsController>() || col.transform.GetComponent<Bullet>())
        {
            if (owner != null)
                owner.AddPoints(100);

            if (col.transform.GetComponent<StatsController>())
                col.transform.GetComponent<StatsController>().ModifyHealth(-10);

            if (destroyOnCollision) Destroy(gameObject);
        }
    }
}
