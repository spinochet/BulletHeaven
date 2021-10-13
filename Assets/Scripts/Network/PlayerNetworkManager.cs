using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class PlayerNetworkManager : NetworkManager
{
    // Singleton
    public static PlayerNetworkManager instance;

    // Player data
    private NetworkIdentity[] players;

    // Manage connections
    private int readyClients;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        players = new NetworkIdentity[4];

        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }

    void Update()
    {
        Debug.Log(networkAddress);
    }

    // -----------------
    // SERVER NETWORKING
    // -----------------

    // This is invoked when a server is started - including when a host is started.
    public virtual void OnStartServer()
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
        {
            base.OnStartServer();
        }
    }

    //Called on the server when a new client connects.
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
        {
            Debug.Log("Client connected");
            base.OnServerConnect(conn);
            StartCoroutine(StoreIdentity(conn));
        }
    }

    // Called on the server when a client disconnects.
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
        {
            base.OnServerDisconnect(conn);
            Debug.Log(numPlayers.ToString());

            if (numPlayers == 0)
            {
                ServerChangeScene("Lobby 1");
            }
        }
    }

    // Called on the server when a client is ready (= loaded the scene)
    public override void OnServerReady(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
        {
            base.OnServerReady(conn);
            readyClients++;

            if (networkSceneName.Contains("Level") && numPlayers == readyClients)
            {
                for (int i = 0; i < 4; ++i)
                {
                    if (players[i])
                    {
                        GameObject pawn = Instantiate(spawnPrefabs[i % 2], new Vector3(-3.0f + (1.5f * i), 0.0f, 0.0f), Quaternion.identity);
                        NetworkServer.Spawn(pawn, players[i].gameObject);
                        
                        players[i].gameObject.GetComponent<PlayerController>().manager = this;
                        players[i].gameObject.GetComponent<PlayerController>().TargetPossesPawn(pawn.GetComponent<NetworkIdentity>());
                    }
                }
            }
        }
    }

    // Wait for identity to assign it to players
    IEnumerator StoreIdentity(NetworkConnection conn)
    {
        // Wait for identity to exist
        NetworkIdentity  identity = conn.identity;
        while (!identity)
        {
            identity = conn.identity;
            yield return null;
        }

        // Assign to first unnasigned slot
        for (int i = 0; i < 4; ++i)
        {
            if (!players[i])
            {
                players[i] = identity;
                break;
            }
        }

        GameObject.Find("LobbyController").GetComponent<LobbyManager>().JoinLobby(identity);
    }

    // -----------------
    // CLIENT NETWORKING
    // -----------------

    // Called on the client when connected to a server. By default it sets client as ready and adds a player.
    public override void OnClientConnect(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ClientOnly)
        {
            base.OnClientConnect(conn);
        }
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ClientOnly)
        {
            base.OnClientSceneChanged(conn);
        }
    }

    // -----------
    // ARCADE MODE
    // -----------

    public void LoadArcadeLevel(int levelNum)
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
        {
            ServerChangeScene("Level " + levelNum.ToString());
            readyClients = 0;
        }
    }
}
