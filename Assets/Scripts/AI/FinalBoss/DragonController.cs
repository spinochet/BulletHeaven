using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    [Header ("Laser Attack")]
    [SerializeField] private GameObject laser;
    [SerializeField] private float xBound = 8.0f;
    [SerializeField] private float yBound = 4.0f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

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
}
