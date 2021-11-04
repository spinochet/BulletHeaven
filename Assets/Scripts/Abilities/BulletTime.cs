using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : Ability
{
    [SerializeField] private float timeScale = 0.3f;

    override public void Activate()
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = timeScale / 60.0f;
    }

    override public void Deactivate()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 1.0f / 60.0f;
    }
}
