using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

abstract public class Ability : NetworkBehaviour
{
    [SerializeField] protected float cost = 20.0f;
    [SerializeField] protected bool overTime;

    abstract public void Activate();
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
