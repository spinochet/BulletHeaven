using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Ultima : Ability
{
    private bool used = false;

    [Command(requiresAuthority = false)]
    override public void Activate()
    {
        if (!used)
        {
            used = true;
            Debug.Log("ULTIMA!!!");
            
            // Time.timeScale = 0.0F;
            
            // Play animation

            // Destroy enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }

            // Destroy bullets
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets)
            {
                Destroy(bullet);
            }
        }
    }

    [Command(requiresAuthority = false)]
    override public void Deactivate()
    {
        // Reset time scale
        // Time.timeScale = 1.0f;
        // Time.fixedDeltaTime = 1.0f / 60.0f;
    }
}
