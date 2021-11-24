using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class FinalBossController : EnemyController
{
    [SerializeField] private Slider hp;

    [Header ("Big Ass Laser")]
    [SerializeField] private GameObject sideLaserPrefab;
    [SerializeField] private float sideLaserRate = 1.5f;
    [SerializeField] private float sideLaserChance = 1.0f;
    private float sideLaserTimer = 0.0f;

    [Header ("Sword Whack!")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private float swordRate = 5.0f;
    [SerializeField] private float swordChance = 1.0f;
    private float swordTimer = 0.0f;

    [Header ("Bitch Ass Laser")]
    [SerializeField] private GameObject smallLaserPrefab;
    [SerializeField] private float fireRate = 5.0f;
    [SerializeField] private float fireChance = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Scroll();

        sideLaserTimer += Time.deltaTime;
        swordTimer += Time.deltaTime;
        fireTimer += Time.deltaTime;

        if (sideLaserTimer >= sideLaserRate)
        {
            sideLaserTimer = 0.0f;
            Pew();
        }

        if (swordTimer >= swordRate)
        {
            swordTimer = 0.0f;
            Whack();
        }

        if (fireTimer > fireRate)
        {
            float f = Random.Range(0.0f, 1.0f);
            if (f < fireChance)
            {
                fireTimer = 0.0f;
                pawn.Shoot(AimAtPlayer());
            }
        }

        hp.value = pawn.GetHP();
    }

    // Fire laser from the side of the screen
    private void Pew()
    {
        GameObject sl = GameObject.Instantiate(sideLaserPrefab);
    }

    // Swing sword on half of screen
    private void Whack()
    {
        GameObject sword = GameObject.Instantiate(swordPrefab);
    }
}
