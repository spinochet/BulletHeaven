using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Mirror;

public class CutScene : NetworkBehaviour
{
    public string scene;

    // Update is called once per frame
    [Command(requiresAuthority = false)]
    public void NextLevel()
    {
        PlayerNetworkManager.Instance.LoadArcadeLevel(scene);
    }
}
