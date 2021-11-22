using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private Bullet.BulletLevel attributes;
    public float duration = 10.0f;
    public float speed = 2.5f;
    private LevelManager level;

    void Start()
    {
        level = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        if (level.IsScrolling) {
            transform.position -= new Vector3(0.0f, 0.0f, speed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        Pawn pawn = col.transform.GetComponent<Pawn>();

        if (pawn && pawn.IsPlayerPawn)
        {
            pawn.PowerUp(attributes, duration);
            Destroy(gameObject);
        }
    }
}
