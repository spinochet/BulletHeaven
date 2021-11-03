using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Mirror;

public class EndLevel : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= 0.0f)
        {
            PlayerNetworkManager.Instance.StopHost();
            LevelManager.Instance.NextLevel();
        }
    }
}
