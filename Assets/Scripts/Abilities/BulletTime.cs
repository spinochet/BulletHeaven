using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : Ability
{
    [System.Serializable]
    public struct BulletTimeLevel
    {
        public float timeScale;
        public float cost;
    }

    [SerializeField] protected List<BulletTimeLevel> levels;

    override public void Activate(int level = 0)
    {
        level = level >= levels.Count ? levels.Count : level;

        Time.timeScale = levels[level].timeScale;
        Time.fixedDeltaTime = levels[level].timeScale / 60.0f;
    }

    override public void Deactivate()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 1.0f / 60.0f;
    }
}
