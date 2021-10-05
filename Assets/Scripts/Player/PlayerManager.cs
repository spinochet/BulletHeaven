using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Mirror;

public class PlayerManager : NetworkManager
{
    public enum GameMode {MainMenu, StoryMode};

    public struct PawnMessage : NetworkMessage
    {
        public NetworkIdentity player;
        public NetworkIdentity pawn;
    }

    // Manager
    public static PlayerManager instance;
    private int numPlayers;
    private PlayerController[] players;

    public bool isOffline;
    [SerializeField] private GameMode currentMode;

    // Characters
    [Space (10)]
    [SerializeField] private List<GameObject> characterPrefabs;
    [SerializeField] private Color[] playerColors;

    private GameObject localPlayer;
    private bool isConnectionReady;

    private Pawn p1Pawn;
    private Pawn p2Pawn;
    private Pawn p3Pawn;
    private Pawn p4Pawn;

    void Awake()
    {
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
        
        if (!isOffline)
            GetComponent<PlayerInputManager>().enabled = false;
    }

    // ----------
    // NETWORKING
    // ----------

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (!isOffline)
        {
            base.OnServerConnect(conn);
            StartCoroutine(WaitForServerConnect(conn));
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        if (!isOffline)
        {
            base.OnClientConnect(conn);
            NetworkClient.RegisterHandler<PawnMessage>(AssignPawn);
            StartCoroutine(WaitForClientConnect(conn));
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

        AddPlayer(identity.gameObject.GetComponent<PlayerInput>());
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
        players[player.playerIndex] = player.transform.GetComponent<PlayerController>();
        players[player.playerIndex].manager = this;
        ++numPlayers;

        player.transform.GetComponent<PlayerController>().playerNum = numPlayers;

        if (currentMode == GameMode.MainMenu)
        {
            GameObject pressStart = GameObject.Find("PressStart");
            if (pressStart)
                pressStart.GetComponent<PressStart>().OnSubmit();
        }
        else if (currentMode == GameMode.StoryMode)
        {
            GameObject pawn = Instantiate(characterPrefabs[numPlayers - 1], Vector3.zero, Quaternion.identity);
            pawn.GetComponent<Pawn>().playerNum = numPlayers;

            if (!isOffline)
            {
                NetworkServer.Spawn(pawn, player.gameObject);

                PawnMessage msg = new PawnMessage()
                {
                    player = player.gameObject.GetComponent<NetworkIdentity>(),
                    pawn = pawn.gameObject.GetComponent<NetworkIdentity>()
                };
                NetworkServer.SendToAll(msg);
            }

            players[player.playerIndex].PossesPawn(pawn.GetComponent<Pawn>());
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

    // ---------
    // MAIN MENU
    // ---------

    public void MainMenu()
    {
        currentMode = GameMode.MainMenu;
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

    // ----------------
    // CHARACTER SELECT
    // ----------------

    // Give players character select
    public void CharacterSelect()
    {
        // currentMode = "CharacterSelect";

        // foreach (PlayerInput player in players)
        // {
            // PlayerCharacterSelect characterSelect = GameObject.Find("P" + (player.playerIndex + 1).ToString() + " Selected Character").GetComponent<PlayerCharacterSelect>();
            // player.transform.GetComponentInChildren<CharacterSelectPlayerController>().Setup(characterSelect, playerColors[player.playerIndex], GameObject.Find("CharacterButton"));
        // }
    }
}
