using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    [Header ("Laser Attack")]
    [SerializeField] private GameObject laser;
    [SerializeField] private float xBound = 10.0f;
    [SerializeField] private float yBound = 6.0f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(laser, new Vector3(-2.0f, 7.5f, 0.0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Vector3 startPos = Vector3.zero;
        Vector3 dirVec = Vector3.zero;

        if (timer > 2.0f)
        {
            timer = 0.0f;
            float dir = Random.Range(0.0f, 4.0f);

            if (dir <= 1.0f)
            {
                startPos = new Vector3(Random.Range(-xBound, xBound), yBound, 0.0f);
                dirVec = Vector3.down;
            }
            else if (dir <= 2.0f)
            {
                startPos = new Vector3(Random.Range(-xBound, xBound), -yBound, 0.0f);
                dirVec = Vector3.up;
            }
            else if (dir <= 3.0f)
            {
                startPos = new Vector3(xBound, Random.Range(-yBound, yBound), 0.0f);
                dirVec = Vector3.left;
            }
            else if (dir <= 4.0f)
            {
                startPos = new Vector3(-xBound, Random.Range(-yBound, yBound), 0.0f);
                dirVec = Vector3.right;
            }
            
            LaserAttack l = Instantiate(laser, startPos, Quaternion.identity).GetComponent<LaserAttack>();
            l.Shoot(dirVec);
        }
    }
}
