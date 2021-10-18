using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class NetworkDebug : NetworkManager
{
    public int currentPawn = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartHost();

        GameObject pawn = Instantiate(spawnPrefabs[currentPawn]);
        NetworkServer.Spawn(pawn);
    }
}
