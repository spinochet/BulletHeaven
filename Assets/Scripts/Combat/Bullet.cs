using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool destroyOnCollision = true;
    [SerializeField] private bool isEnemyBullet = true;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float fireRate = 30.0f;

    // Controllers
    private PlayerController playerController;
    private CompanionController companionController;
    private EnemyController enemyController;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
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
            if (playerController) playerController.AddPoints();

            if (col.transform.GetComponent<StatsController>())
                col.transform.GetComponent<StatsController>().ModifyHealth(-10);

            if (destroyOnCollision) Destroy(gameObject);
        }
    }
}
