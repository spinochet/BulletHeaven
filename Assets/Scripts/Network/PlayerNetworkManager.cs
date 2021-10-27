using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class PlayerNetworkManager : NetworkManager
{
    // Singleton
    public static PlayerNetworkManager _instance;
    public static PlayerNetworkManager Instance { get { return _instance; } }

    // Player data
    private NetworkIdentity[] players;

    // Manage connections
    private int readyClients;

    // Debug flag
    public bool debugLevel;
    public int debugPlayer;

    // Story
    public int activeCharacter;
    GameObject princess;
    GameObject robot;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        players = new NetworkIdentity[4];

        if (_instance == null)
            _instance = this;
        else
            DestroyImmediate(this);
    }

    void Start()
    {
        if (debugLevel)
        {
            StartHost();
        }
    }

    void Update()
    {
        // Debug.Log(GetLocalIPAddress());
    }

    // -----------------
    // SERVER NETWORKING
    // -----------------

    // This is invoked when a server is started - including when a host is started.
    public override void OnStartServer()
    {
        if (mode == NetworkManagerMode.ServerOnly)
        {
            base.OnStartServer();
        }
    }

    public override void OnStopServer()
    {
        if (mode == NetworkManagerMode.ServerOnly)
        {
            base.OnStopServer();
        }
    }

    public override void OnStartHost()
    {
        if (mode == NetworkManagerMode.Host)
        {
            base.OnStartHost();
        }
    }

    public override void OnStopHost()
    {
        if (mode == NetworkManagerMode.Host)
        {
            base.OnStopHost();
        }
    }

    //Called on the server when a new client connects.
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
        {
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
                        princess = Instantiate(spawnPrefabs[0], new Vector3(-3.0f + (1.5f * i), 0.0f, 0.0f), Quaternion.identity);
                        NetworkServer.Spawn(princess, players[i].gameObject);

                        robot = Instantiate(spawnPrefabs[1], new Vector3(-3.0f + (1.5f * i), 0.0f, 0.0f), Quaternion.identity);
                        NetworkServer.Spawn(robot, players[i].gameObject);
                        robot.SetActive(false);
                        
                        players[i].gameObject.GetComponent<PlayerController>().TargetPossesPawn(princess.GetComponent<NetworkIdentity>());
                    }
                }

                LevelManager.Instance.StartLevel();
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

        if (debugLevel)
        {
            princess = Instantiate(spawnPrefabs[0], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            NetworkServer.Spawn(princess, identity.gameObject);
            GameObject.Find("HUD").GetComponent<HUDManager>().AssignHUD(princess.GetComponent<Pawn>(), 1);

            robot = Instantiate(spawnPrefabs[1], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            NetworkServer.Spawn(robot, identity.gameObject);
            robot.SetActive(false);
            
            identity.gameObject.GetComponent<PlayerController>().TargetPossesPawn(princess.GetComponent<NetworkIdentity>());

            LevelManager.Instance.StartLevel();
        }
        else if (networkSceneName.Contains("Lobby"))
        {
            GameObject lobby = GameObject.Find("LobbyController");
            if (lobby) lobby.GetComponent<LobbyManager>().JoinLobby(identity);
        }
    }

    // -----------------
    // CLIENT NETWORKING
    // -----------------

    // This is invoked when the client is started.
    public override void OnStartClient()
    {
        if (mode == NetworkManagerMode.ClientOnly)
        {
            base.OnStartClient();
        }
    }

    public override void OnStopClient()
    {
        if (mode == NetworkManagerMode.ClientOnly)
        {
            base.OnStopClient();
        }
    }

    // Called on the client when connected to a server. By default it sets client as ready and adds a player.
    public override void OnClientConnect(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ClientOnly || mode == NetworkManagerMode.Host)
        {
            base.OnClientConnect(conn);
        }
    }

    // Called on clients when a scene has completed loaded, when the scene load was initiated by the server.
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ClientOnly || mode == NetworkManagerMode.Host)
        {
            base.OnClientSceneChanged(conn);
        }
    }

    // -----
    // LOBBY
    // -----

    public void LoadLobby()
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
        {
            Debug.Log("Loading Lobby");
            ServerChangeScene("Lobby");
        }
    }

    // ----------
    // STORY MODE
    // ----------

    public void LoadStoryLevel(string level)
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
        {
            ServerChangeScene(level);
            readyClients = 0;
        }
    }

    // -----------
    // ARCADE MODE
    // -----------

    public void LoadArcadeLevel(string level)
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Host)
        {
            ServerChangeScene(level);
            readyClients = 0;
        }
    }

    public void SwitchCharacters(Pawn _pawn)
    {
        if (activeCharacter == 0)
        {
            if (princess != null && robot != null)
            {
                activeCharacter = 1;
                robot.SetActive(true);

                players[0].gameObject.GetComponent<PlayerController>().TargetPossesPawnPos(robot.GetComponent<NetworkIdentity>(), princess.transform.position);
                GameObject.Find("HUD").GetComponent<HUDManager>().AssignHUD(robot.GetComponent<Pawn>(), 1);

                princess.GetComponent<Pawn>().EnableMovement(false);
                princess.SetActive(false);
            }
        }
        else
        {
            if (princess != null && robot != null)
            {
                activeCharacter = 0;
                princess.SetActive(true);

                players[0].gameObject.GetComponent<PlayerController>().TargetPossesPawnPos(princess.GetComponent<NetworkIdentity>(), robot.transform.position);
                GameObject.Find("HUD").GetComponent<HUDManager>().AssignHUD(princess.GetComponent<Pawn>(), 1);

                robot.GetComponent<Pawn>().EnableMovement(false);
                robot.SetActive(false);
            }
        }

        // GameObject pawn = Instantiate(spawnPrefabs[activeCharacter], _pawn.transform.position, Quaternion.identity);
        // NetworkServer.Spawn(pawn, players[0].gameObject);
        
        // players[0].gameObject.GetComponent<PlayerController>().manager = this;
        // players[0].gameObject.GetComponent<PlayerController>().TargetPossesPawn(pawn.GetComponent<NetworkIdentity>());
    }
}
