using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollision : MonoBehaviour
{
    [SerializeField] private float damage = 20.0f;

    void OnTriggerEnter(Collider col)
    {
        // NewPlayerController player = col.transform.GetComponent<NewPlayerController>();

        // if (player)
        // {
        //     player.TakeDamage(damage);
        // }
    }
}
