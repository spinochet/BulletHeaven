using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private float curePotency = 20.0f;

    public void OnTriggerEnter(Collider col)
    {
        Pawn pawn = col.transform.GetComponent<Pawn>();

        if (pawn && pawn.IsPlayerPawn)
        {
            pawn.RestoreHealth(curePotency);
            Destroy(gameObject);
        }
    }
}
