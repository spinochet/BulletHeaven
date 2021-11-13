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
            {
                newPlayer.PossesPawn(player1Pawn);
                player1Pawn.SetVisibility(true);
            }
            else if (player2Pawn && !player2Pawn.IsPossesed)
            {
                newPlayer.PossesPawn(player2Pawn);
                player2Pawn.SetVisibility(true);
            }

            // Check if both players have spawned
            if (player1Pawn && player1Pawn.IsPossesed && player2Pawn && player2Pawn.IsPossesed)
            {
                player1Pawn.AssignPartner(null);
                player2Pawn.AssignPartner(null);

                if (hud)
                    hud.GetComponent<HUDManager>().ToggleOffHUD(false);
            }
        }
    }

    // Spawn pawns into level
    public void SpawnPawns(Vector3 spawn1, Vector3 spawn2)
    {
        GameObject hud = GameObject.Find("HUD");

        // Spawn pawns
        player1Pawn = GameObject.Instantiate(playablePrefabs[0], spawn1, Quaternion.identity).GetComponent<Pawn>();
        player2Pawn = GameObject.Instantiate(playablePrefabs[1], spawn2, Quaternion.identity).GetComponent<Pawn>();

        // Assign player 1
        if (p1 != null && p2 == null)
        {
            player1Pawn.AssignPartner(player2Pawn);
            player2Pawn.AssignPartner(player1Pawn);
            player2Pawn.SetVisibility(false);

            if (hud)
                hud.GetComponent<HUDManager>().ToggleOffHUD(true);
        }

        // Assign player 2
        else if (p2 != null && p1 == null)
        {
            player1Pawn.AssignPartner(player2Pawn);
            player2Pawn.AssignPartner(player1Pawn);
            player1Pawn.SetVisibility(false);

            if (hud)
                hud.GetComponent<HUDManager>().ToggleOffHUD(true);
        }

        // No players
        else if (p1 == null && p2 == null)
        {
            player1Pawn.AssignPartner(player2Pawn);
            player2Pawn.AssignPartner(player1Pawn);

            player1Pawn.SetVisibility(false);
            player2Pawn.SetVisibility(false);

            if (hud)
                hud.GetComponent<HUDManager>().ToggleOffHUD(true);
        }

        // Both players
        else if (hud)
        {
            hud.GetComponent<HUDManager>().ToggleOffHUD(false);
        }

        // Assign player 1 HUD
        if (p1 != null)
        {
            if (hud) hud.GetComponent<HUDManager>().AssignHUD(p1, 0);
            p1.PossesPawn(player1Pawn);
        }

        // Assign player 2 HUD
        if (p2 != null)
        {
            if (hud) hud.GetComponent<HUDManager>().AssignHUD(p2, 1);
            p2.PossesPawn(player2Pawn);
        }
    }

    // --------
    // GAMEPLAY
    // --------

    public void LockPlayers()
    {
        if (p1)
            p1.enabled = false;
        if (p2)
            p1.enabled = false;
    }

    public void UnlockPlayers()
    {
        if (p1)
            p1.enabled = true;
        if (p2)
            p1.enabled = true;
    }

    public void CheckPlayersAlive()
    {
        bool playersExist = p1 || p2;
        bool playersDead = ((p1 && !p1.IsAlive) || !p1) && ((p2 && !p2.IsAlive) || !p2);

        if (playersExist && playersDead)
        {
            GameObject lose = GameObject.Find("LoseMenu");
            if (lose) lose.GetComponent<LoseController>().ToggleLose(true);

            GameObject level = GameObject.Find("LoseMenu");
            if (lose) lose.GetComponent<LoseController>().ToggleLose(true);
        }
    }
}
