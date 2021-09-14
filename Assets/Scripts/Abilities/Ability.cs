using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Ability : MonoBehaviour
{
    protected string owner;

    public void SetOwner(string _owner)
    {
        owner = _owner;
    }

    abstract public bool Activate();
    abstract public void Deactivate();
    abstract public float GetCost();
}
