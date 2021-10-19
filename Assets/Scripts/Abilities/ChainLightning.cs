using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Ability
{
    [SerializeField] private int maxChain = 3;
    [SerializeField] private float timing;
    [SerializeField] private float minDist = 20.0f;

    override public void Activate()
    {
        StartCoroutine(Chain());

        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // GameObject current = gameObject;

        // for (int i = 0; i < maxChain; ++i)
        // {
        //     GameObject closest = null;
        //     float dist = minDist;

        //     foreach (GameObject enemy in enemies)
        //     {
        //         Vector3 diff = current.transform.position - enemy.transform.position;
        //         if (diff.magnitude < dist && enemy != current)
        //         {
        //             closest = enemy;
        //             dist = diff.magnitude;
        //         }
        //     }

        //     if (closest == null) break;
        //     if (current.tag == "Enemy") Destroy(current);
        //     current = closest;
        // }

        // if (current.tag == "Enemy") Destroy(current);
    }

    override public void Deactivate()
    {
        
    }

    IEnumerator Chain()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject current = gameObject;
        float timer = timing;

        for (int i = 0; i <= maxChain; ++i)
        {
            while (timer < timing)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            GameObject closest = null;
            float dist = minDist;

            foreach (GameObject enemy in enemies)
            {
                if (enemy)
                {
                    Vector3 diff = current.transform.position - enemy.transform.position;
                    if (diff.magnitude < dist && enemy != current)
                    {
                        closest = enemy;
                        dist = diff.magnitude;
                    }
                }
            }

            if (closest == null) break;
            if (current.tag == "Enemy") Destroy(current);
            current = closest;

            timer = 0.0f;
        }
    }
}
