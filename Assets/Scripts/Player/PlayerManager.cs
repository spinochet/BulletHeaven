using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Manager
    public static PlayerManager instance;
    private int numPlayers;
    private PlayerInput[] players;
    [SerializeField] private string currentMode;
    [SerializeField] private GameObject AIPlayer;

    // Story mode stuff
    [Header ("Story mode stuff")]
    [SerializeField] private Pawn princessPrefab;
    [SerializeField] private Pawn robotPrefab;

    // Character select stuff
    [SerializeField] private Color[] playerColors;

    private GameObject p1AI;
    private GameObject p2AI;
    private GameObject p3AI;
    private GameObject p4AI;

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
        players = new PlayerInput[4];
    }

    // -----------------
    // PLAYER MANAGEMENT
    // -----------------

    // Add player to manager
    public void AddPlayer(PlayerInput player)
    {
        string playerName = "Player " + (player.playerIndex + 1).ToString();
        player.gameObject.name = playerName;
        DontDestroyOnLoad(player.gameObject);

        players[player.playerIndex] = player;
        players[player.playerIndex].transform.GetComponent<PlayerController>().manager = this;
        ++numPlayers;

        if (currentMode == "MainMenu")
        {
            GameObject pressStart = GameObject.Find("PressStart");
            if (pressStart)
                pressStart.GetComponent<PressStart>().OnSubmit();
        }
        else if (currentMode == "StoryMode")
        {
            if (p2Pawn)
            {
                Destroy(p2AI);

                GameObject hud = GameObject.Find("P4 Stats");
                // player.transform.GetComponent<PlayerController>().AssignPawn(p2Pawn, hud.GetComponent<HUDController>());
            }
        }
        // if (currentScene == "CharacterSelect")
        // {
            // PlayerCharacterSelect characterSelect = GameObject.Find("P" + (player.playerIndex + 1).ToString() + " Selected Character").GetComponent<PlayerCharacterSelect>();
            // player.transform.GetComponentInChildren<CharacterSelectPlayerController>().Setup(characterSelect, playerColors[player.playerIndex], GameObject.Find("CharacterButton"));
        // }

        // CharacterSelect();
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
        currentMode = "MainMenu";
    }

    // ----------
    // STORY MODE
    // ----------

    // Load players into story mode
    public void StoryMode()
    {
        currentMode = "StoryMode";
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

        // Instantiate(princessPrefab);
        // Instantiate(robotPrefab);

        // Spawn princess
        if (numPlayers >= 1)
        {
            Vector3 spawnPoint = new Vector3(-3.5f, 0.0f, -3.5f);
            GameObject hud = GameObject.Find("P2 Stats");

            p1Pawn = Instantiate(princessPrefab, spawnPoint, Quaternion.identity);
            players[0].transform.GetComponent<PlayerController>().AssignPawn(p1Pawn.GetComponent<Pawn>());
        }

        if (numPlayers >= 2)
        {
            Vector3 spawnPoint = new Vector3(3.5f, 0.0f, -3.5f);
            GameObject hud = GameObject.Find("P4 Stats");

            p2Pawn = Instantiate(robotPrefab, spawnPoint, Quaternion.identity);
            // players[1].transform.GetComponent<PlayerController>().AssignPawn(p2Pawn, hud.GetComponent<HUDController>());
        }
        else
        {
            p2AI = Instantiate(AIPlayer);

            Vector3 spawnPoint = new Vector3(3.5f, 0.0f, -3.5f);
            GameObject hud = GameObject.Find("P4 Stats");
            
            p2Pawn = Instantiate(robotPrefab, spawnPoint, Quaternion.identity);
            // p2AI.GetComponent<CompanionController>().AssignPawn(p2Pawn, hud.GetComponent<HUDController>());
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
        currentMode = "CharacterSelect";

        foreach (PlayerInput player in players)
        {
            // PlayerCharacterSelect characterSelect = GameObject.Find("P" + (player.playerIndex + 1).ToString() + " Selected Character").GetComponent<PlayerCharacterSelect>();
            // player.transform.GetComponentInChildren<CharacterSelectPlayerController>().Setup(characterSelect, playerColors[player.playerIndex], GameObject.Find("CharacterButton"));
        }
    }
}
