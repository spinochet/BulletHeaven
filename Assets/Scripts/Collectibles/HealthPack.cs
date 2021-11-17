using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private float curePotency = 20.0f;
    public float speed = 2.5f;

    void Update()
    {
        transform.position -= new Vector3(0.0f, 0.0f, speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider col)
    {
        Pawn pawn = col.transform.GetComponent<Pawn>();

        if (pawn && pawn.IsPlayerPawn)
        {
            pawn.RestoreHealth(curePotency);
            SoundManager.Instance.Play("Health Pickup");
            Destroy(gameObject);
        }
    }
}
