using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : Ability
{
    [SerializeField] private float cost = 20.0f;

    override public bool Activate()
    {
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.3f / 60.0f;
        return true;
    }

    override public void Deactivate()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 1.0f / 60.0f;
    }

    override public float GetCost()
    {
        return cost;
    }
}
