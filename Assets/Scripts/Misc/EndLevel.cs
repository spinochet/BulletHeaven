using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Mirror;

public class EndLevel : NetworkBehaviour
{

    public string nextLevel;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= 0.0f)
        {
            GameObject.Find("PlayerNetworkManager").GetComponent<PlayerNetworkManager>().LoadArcadeLevel(nextLevel);
        }
    }
}
