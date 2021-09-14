using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Ability
{
    [SerializeField] private float cost = 33.0f;
    [SerializeField] private int maxChain = 3;
    [SerializeField] private float minDist = 20.0f;

    override public bool Activate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject current = GameObject.Find(owner);
        Debug.Log(current.transform.parent);

        for (int i = 0; i < maxChain; ++i)
        {
            GameObject closest = null;
            float dist = minDist;

            foreach (GameObject enemy in enemies)
            {
                Vector3 diff = current.transform.position - enemy.transform.position;
                if (diff.magnitude < dist && enemy != current)
                {
                    closest = enemy;
                    dist = diff.magnitude;
                }
            }


            if (closest == null) break;
            if (current.tag == "Enemy") Destroy(current);
            current = closest;
        }

        if (current.tag == "Enemy") Destroy(current);

        return false;
    }

    override public void Deactivate()
    {
        
    }

    override public float GetCost()
    {
        return cost;
    }
}
