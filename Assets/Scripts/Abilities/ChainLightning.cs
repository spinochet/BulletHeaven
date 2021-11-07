using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Ability
{
    [System.Serializable]
    public struct ChainLightningLevel
    {
        public float maxChain;
        public float timing;
        public float minDist;

        public float cost;
    }

    [SerializeField] private GameObject particles;
    [SerializeField] protected List<ChainLightningLevel> levels;

    override public void Activate(int level = 0)
    {
        StartCoroutine(Chain(level));
    }

    override public void Deactivate()
    {
        
    }

    IEnumerator Chain(int level)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject current = gameObject;
        float timer = levels[level].timing;

        GameObject particle = Instantiate(particles, current.transform.position, Quaternion.identity);

        for (int i = 0; i <= levels[level].maxChain; ++i)
        {
            while (timer < levels[level].timing)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            GameObject closest = null;
            float dist = levels[level].minDist;

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
