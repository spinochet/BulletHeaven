using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Mirror;

public class PlayerManager : NetworkManager
{
    public enum GameMode {Lobby, StoryMode, ArcadeMode};

    public struct PawnMessage : NetworkMessage
    {
        public NetworkIdentity player;
        public NetworkIdentity pawn;
    }

    // Manager
    public static PlayerManager instance;
    private int numPlayers;
    private PlayerController[] players;

    [SerializeField] private GameMode currentMode;

    // Characters
    [Space (10)]
    [SerializeField] private List<GameObject> characterPrefabs;
    [SerializeField] private Color[] playerColors;

    // Connection
    private GameObject localPlayer;
    private bool isConnectionReady;

    // Players objects for offline
    private Pawn p1Pawn;
    private Pawn p2Pawn;
    private Pawn p3Pawn;
    private Pawn p4Pawn;

    void Awake()
    {
        if (Application.isBatchMode)
        {
            StartServer();
            Debug.Log("Server started");
        }
        // else
        // {
        //     StartClient();
        // }

        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        players = new PlayerController[4];

    }

    // -----------------
    // SERVER NETWORKING
    // -----------------

    // Like Start(), but only called on server and host.
    public override void OnStartServer()
    {
        GetComponent<PlayerInputManager>().enabled = false;
    }

    //Called on the server when a new client connects.
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ServerOnly)
        {
            Debug.Log("Client connected");
            base.OnServerConnect(conn);
            StartCoroutine(WaitForServerConnect(conn));

            ++numPlayers;
        }
    }

    // Called on the server when a client disconnects.
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ServerOnly)
        {
            base.OnServerDisconnect(conn);
            --numPlayers;
        }
    }

    IEnumerator WaitForServerConnect(NetworkConnection conn)
    {
        NetworkIdentity  identity = conn.identity;
        while (!identity)
        {
            identity = conn.identity;
            yield return null;
        }

        if (currentMode == GameMode.Lobby)
        {
            GameObject.Find("LobbyController").GetComponent<LobbyManager>().JoinLobby(identity);
            players[numPlayers] = identity.gameObject.GetComponent<PlayerController>();
        }

        // Spawn lobby 

        // AddPlayer(identity.gameObject.GetComponent<PlayerInput>());
    }

    // -----------------
    // CLIENT NETWORKING
    // -----------------

    // Like Start(), but only called for objects the client has authority over.
    public override void OnStartClient()
    {
        GetComponent<PlayerInputManager>().enabled = false;
    }

    // Called on the client when connected to a server. By default it sets client as ready and adds a player.
    public override void OnClientConnect(NetworkConnection conn)
    {
        if (mode == NetworkManagerMode.ClientOnly)
        {
            base.OnClientConnect(conn);
            NetworkClient.RegisterHandler<PawnMessage>(AssignPawn);
            StartCoroutine(WaitForClientConnect(conn));
        }
    }

    IEnumerator WaitForClientConnect(NetworkConnection conn)
    {
        NetworkIdentity  identity = conn.identity;
        while (!identity)
        {
            identity = conn.identity;
            yield return null;
        }

        isConnectionReady = true;
    }

    // -----------------
    // PLAYER MANAGEMENT
    // -----------------

    // Add player to manager
    public void AddPlayer(PlayerInput player)
    {
        players[numPlayers] = player.transform.GetComponent<PlayerController>();
        // players[numPlayers].manager = this;

        // player.transform.GetComponent<PlayerController>().playerNum = numPlayers;

        if (currentMode == GameMode.StoryMode)
        {
            GameObject pawn = Instantiate(characterPrefabs[1], Vector3.zero, Quaternion.identity);
            // pawn.GetComponent<Pawn>().playerNum = numPlayers;

            if (mode == NetworkManagerMode.ServerOnly)
            {
                NetworkServer.Spawn(pawn, player.gameObject);

                PawnMessage msg = new PawnMessage()
                {
                    player = player.gameObject.GetComponent<NetworkIdentity>(),
                    pawn = pawn.gameObject.GetComponent<NetworkIdentity>()
                };
                NetworkServer.SendToAll(msg);
            }

            players[numPlayers].PossesPawn(pawn.GetComponent<Pawn>());
        }
    }
    
    public void AssignPawn(PawnMessage msg)
    {
        StartCoroutine(WaitToAssignPlayer(msg));
    }

    IEnumerator WaitToAssignPlayer(PawnMessage msg)
    {
        while (!isConnectionReady) yield return null;

        msg.player.gameObject.GetComponent<PlayerController>().PossesPawn(msg.pawn.gameObject.GetComponent<Pawn>());
    }

    // Remove players
    public void RemovePlayer(PlayerInput player)
    {
        Destroy(player.gameObject);
        players[player.playerIndex] = null;
        --numPlayers;
    }

    private bool isPaused;
    public void TogglePause(PlayerController caller)
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;

        PlayerManager playerManager = this;

        // Disable all pawns
        PawnController[] controllers = Object.FindObjectsOfType<PawnController>();
        foreach(PawnController script in controllers)
        {
            script.TogglePause(isPaused);
        }

        // Disable all bullets
        Bullet[] bullets = Object.FindObjectsOfType<Bullet>();
        foreach(Bullet script in bullets)
        {
            script.enabled = !isPaused;
        }
    }

    // ----------
    // STORY MODE
    // ----------

    // Load players into story mode
    public void StoryMode()
    {
        currentMode = GameMode.StoryMode;
        StartCoroutine(LoadStoryLevel());
    }

    // Load first story level
    IEnumerator LoadStoryLevel()
    {
        // Load level
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Level 1", LoadSceneMode.Single);

        // Wait for level to load
        while (!asyncLoadLevel.isDone)
            yield return null;

        // Wait for HUD to be awake
        while (GameObject.Find("HUD") == null)
            yield return null;

        GameObject.Find("P1 Stats").SetActive(false);
        GameObject.Find("P3 Stats").SetActive(false);


        // Spawn princess
        if (numPlayers >= 1)
        {
            Vector3 spawnPoint = new Vector3(-3.5f, 0.0f, -3.5f);
            GameObject hud = GameObject.Find("P2 Stats");

            // p1Pawn = Instantiate(characterPrefabs[0], spawnPoint, Quaternion.identity);
            // players[0].transform.GetComponent<PlayerController>().PossesPawn(p1Pawn.GetComponent<Pawn>());
        }
    }

    //Switch characters
    public void SwitchCharacters()
    {
        if (numPlayers == 1)
        {
            Pawn tempPawn = p1Pawn;
            p1Pawn = p2Pawn;
            p2Pawn = tempPawn;

            // players[0].transform.GetComponent<PlayerController>().AssignPawn(p1Pawn);
            // if (numPlayers >= 2) players[1].transform.GetComponent<PlayerController>().AssignPawn(p2Pawn);
            // else p2AI.GetComponent<CompanionController>().AssignPawn(p2Pawn);
        }
    }

    // -----------
    // ARCADE MODE
    // -----------

    public void ArcadeMode()
    {
        currentMode = GameMode.ArcadeMode;
        // StartCoroutine(LoadArcadeLevel());

        ServerChangeScene("Level 1");
        // NetworkIdentity[] players = GameObject.Find("LobbyController").GetComponent<LobbyManager>().GetPlayers();

        // Spawn players
        for (int i = 0; i < 4; ++i)
        {
            if (players[i])
            {
                AddPlayer(players[i].gameObject.GetComponent<PlayerInput>());
            }
        }
    }

    // Load first story level
    IEnumerator LoadArcadeLevel()
    {
        // Load level
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Level 1", LoadSceneMode.Single);

        // Wait for HUD to be awake
        while (GameObject.Find("HUD") == null)
            yield return null;

        // NetworkIdentity[] players = GameObject.Find("LobbyController").GetComponent<LobbyManager>().GetPlayers();

        // Spawn princess
        if (numPlayers >= 1)
        {
            Vector3 spawnPoint = new Vector3(-3.5f, 0.0f, -3.5f);
            GameObject hud = GameObject.Find("P2 Stats");
        }

        for (int i = 0; i < 4; ++i)
        {
            if (players[i])
            {
                GameObject pawn = Instantiate(characterPrefabs[0], new Vector3(-3.0f + (1.5f * i), 0.0f, -3.5f), Quaternion.identity);
                NetworkServer.Spawn(pawn, players[i].gameObject);
            }
        }
    }
}
