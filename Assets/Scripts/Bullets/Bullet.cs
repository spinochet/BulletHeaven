using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private bool destroyOnCollision = true;
    [SerializeField] private bool isEnemyBullet = false;

    [SerializeField] protected string name;
    [SerializeField] protected float speed = 5.0f;
    [SerializeField] protected float damage = 10.0f;
    [SerializeField] protected float fireRate = 30.0f;

    private Pawn owner;

    void Awake()
    {
        SoundManager.Instance.Play(name);
    }

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
        if (col.transform.GetComponent<Pawn>() || col.transform.GetComponent<Bullet>())
        {
            // if (owner != null)
            //     owner.AddPoints(100);

            if (col.transform.GetComponent<Pawn>())
                col.transform.GetComponent<Pawn>().TakeDamage(10);

            if (destroyOnCollision) Destroy(gameObject);
        }
        else if (col.gameObject.tag == "Letter")
        {
            Debug.Log("Here");
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
