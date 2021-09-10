using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : Ability
{
    override public void Activate()
    {
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.3f / 60.0f;
    }

    override public void Deactivate()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 1.0f / 60.0f;
    }
}
