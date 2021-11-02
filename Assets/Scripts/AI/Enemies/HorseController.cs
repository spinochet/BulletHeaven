using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseController : EnemyController
{
    // Update is called once per frame
    void Update()
    {
        controller.Move(-Vector3.forward * speed * Time.deltaTime);
        contactTimer += Time.deltaTime;
    }
}
