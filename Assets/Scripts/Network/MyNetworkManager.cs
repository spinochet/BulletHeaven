using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] private GameObject[] characters;

    // public override void OnClientConnect(NetworkConnection conn)
    // {
    //     base.OnClientConnect(conn);
    // }
}
