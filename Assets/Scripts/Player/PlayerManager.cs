using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // Singleton
    public static PlayerManager _instance;
    public static PlayerManager Instance { get { return _instance; } }

    // Players
    private PlayerController p1 = null;
    public PlayerController P1 { get { return p1; } }
    private PlayerController p2 = null;
    public PlayerController P2 { get { return p2; } }

    private Pawn player1Pawn = null;
    private Pawn player2Pawn = null;

    // Player prefabs
    public List<GameObject> playablePrefabs;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add players to game
    public void AddPlayer(PlayerInput input)
    {
        PlayerController newPlayer = input.transform.GetComponent<PlayerController>();
        if (input.playerIndex == 0)
            p1 = newPlayer;
        else
            p2 = newPlayer;

        // Drop-in Co-op
        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
            // Assign hud
            GameObject hud = GameObject.Find("HUD");
            if (hud)
                hud.GetComponent<HUDManager>().AssignHUD(newPlayer, input.playerIndex);
            // Spawn players
            if (player1Pawn && !player1Pawn.IsPossesed)
                newPlayer.PossesPawn(player1Pawn);
            else if (player2Pawn && !player2Pawn.IsPossesed)
                newPlayer.PossesPawn(player2Pawn);
        }
    }

    // Spawn pawns into level
    public void SpawnPawns()
    {
        GameObject hud = GameObject.Find("HUD");

        // Spawn player 1
        player1Pawn = GameObject.Instantiate(playablePrefabs[0]).GetComponent<Pawn>();
        if (p1 != null)
        {
            if (hud) hud.GetComponent<HUDManager>().AssignHUD(p1, 0);
            p1.PossesPawn(player1Pawn);
        }

        // Spawn player 2
        player2Pawn = GameObject.Instantiate(playablePrefabs[1]).GetComponent<Pawn>();
        if (p2 != null)
        {
            if (hud) hud.GetComponent<HUDManager>().AssignHUD(p2, 1);
            p2.PossesPawn(player2Pawn);
        }
        else
        {
            player1Pawn.AssignPartner(player2Pawn);
            player2Pawn.AssignPartner(player1Pawn);

            player2Pawn.SetVisibility(false);
        }
    }
}
