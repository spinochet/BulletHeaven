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
        public float damage;
        public float cost;
    }

    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject trail;
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
        GameObject trailObject = Instantiate(trail, transform.position, Quaternion.identity);
        Vector3 closestPosition = transform.position;
        float timer = levels[level].timing;

        for (int i = 0; i < levels[level].maxChain; ++i)
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
                    Vector3 diff = closestPosition - enemy.transform.position;
                    if (diff.magnitude < dist && diff.magnitude > 0.1f)
                    {
                        closest = enemy;
                        dist = diff.magnitude;
                    }
                }
            }

            if (closest)
            {
                closestPosition = closest.transform.position;
                Instantiate(particles, closestPosition, Quaternion.identity);
                SoundManager.Instance.Play("Chain Lightning");
                trailObject.transform.position = closestPosition;
                
                Pawn enemy = closest.transform.GetComponent<Pawn>();
                if (enemy) {
                    enemy.TakeDamage(levels[level].damage);
                }

            }
            else
            {
                break;
            }

            timer = 0.0f;
        }
    }
}
