using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    [Header ("Stats")]
    [SerializeField] private float maxHP;
    private float hp;

    [Header ("Laser Attack")]
    [SerializeField] private GameObject laser;
    [SerializeField] private float xBound = 8.0f;
    [SerializeField] private float yBound = 4.0f;

    [Header ("Sword Attack")]
    [SerializeField] private GameObject sword;

    private float timer;
    private float swordTimer;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        swordTimer += Time.deltaTime;

        if (swordTimer >= 5.0f)
        {
            swordTimer = 0.0f;
            float f = Random.Range(0.0f, 1.0f);
            if (f < 0.25f)
            {
                Instantiate(sword, new Vector3(45.0f, 3.0f, 0.0f), Quaternion.Euler(90.0f, 0.0f, 0.0f));
            }
        }

        LaserPhase();
    }

    void LaserPhase()
    {

        if (timer > 2.0f)
        {
            timer = 0.0f;
            float dir = Random.Range(0.0f, 4.0f);

            Vector3 startPos = Vector3.zero;
            Vector3 dirVec = Vector3.zero;
            Quaternion rotation = Quaternion.identity;

            if (dir <= 1.0f)
            {
                startPos = new Vector3(Random.Range(-xBound, xBound), 3.0f, 20.0f);
                dirVec = Vector3.down;
                rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            else if (dir <= 2.0f)
            {
                startPos = new Vector3(Random.Range(-xBound, xBound), 3.0f, -20.0f);
                dirVec = Vector3.up;
                rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            else if (dir <= 3.0f)
            {
                startPos = new Vector3(35.0f, 3.0f, Random.Range(-yBound, yBound));
                dirVec = Vector3.left;
                rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            }
            else if (dir <= 4.0f)
            {
                startPos = new Vector3(-35.0f, 3.0f, Random.Range(-yBound, yBound));
                dirVec = Vector3.right;
                rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }
            
            LaserAttack l = Instantiate(laser, startPos, rotation).GetComponent<LaserAttack>();
            l.Shoot(dirVec);
        }
    }

    void SwordAttack()
    {
        float dir = Random.Range(0.0f, 2.0f);

        if (dir < 1.0f)
        {

        }
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject);

        // if (col.gameObject.GetComponent<NewBullet>())
        // {
        //     hp -= 20.0f;
        //     Debug.Log("Bullet");
        //     Destroy(col.gameObject);
        // }
        // else if (col.gameObject.GetComponent<BurstBullet>())
        // {
        //     hp -= 40.0f;
        //     Debug.Log("Burst");
        //     Destroy(col.gameObject);
        // }

        if (hp <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
