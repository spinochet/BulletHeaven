using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Ability : MonoBehaviour
{
    [SerializeField] protected float cost = 20.0f;
    [SerializeField] protected bool overTime;

    abstract public void Activate(int level = 0);
    abstract public void Deactivate();

    public bool IsOverTime()
    {
        return overTime;
    }
    
    public float GetCost()
    {
        return cost;
    }
}
