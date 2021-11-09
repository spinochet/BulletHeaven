using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseController : EnemyController
{
    public string soundEffect;
    // Update is called once per frame
    void Update()
    {
        Scroll();
        contactTimer += Time.deltaTime;
        SoundManager.Instance.Play(soundEffect);
    }
}
