using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class ChainLightning : Ability
{
    [SerializeField] private int maxChain = 3;
    [SerializeField] private float timing;
    [SerializeField] private float minDist = 20.0f;

    [SerializeField] private GameObject particles;

    [Command(requiresAuthority = false)]
    override public void Activate()
    {
        StartCoroutine(Chain());
    }

    override public void Deactivate()
    {
        
    }

    IEnumerator Chain()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject current = gameObject;
        float timer = timing;

        GameObject particle = Instantiate(particles, current.transform.position, Quaternion.identity);
        NetworkServer.Spawn(particle);

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
            if (current.tag == "Enemy")
            {
                particle.transform.position = current.transform.position;
                Destroy(current);
            }
            current = closest;

            timer = 0.0f;
        }
    }
}
