using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool destroyOnCollision = true;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float fireRate = 30.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // Return bullet's fire rate
    public float GetFireRate()
    {
        return fireRate;
    }
}
