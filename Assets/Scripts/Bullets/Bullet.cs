using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool destroyOnCollision = true;
    [SerializeField] private bool isEnemyBullet = false;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float fireRate = 30.0f;

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

    // FIX AFTER SPRINT
    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.GetComponent<StatsController>() || col.transform.GetComponent<Bullet>())
        {
            if (col.transform.GetComponent<StatsController>())
                col.transform.GetComponent<StatsController>().ModifyHealth(-10);

            if (destroyOnCollision) Destroy(gameObject);
        }
    }
}
