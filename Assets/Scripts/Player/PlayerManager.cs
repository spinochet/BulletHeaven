using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Manager
    private int numPlayers;
    private PlayerInput[] players;
    private string currentScene;

    // Character select stuff
    [SerializeField] private Color[] playerColors;

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

        // if (currentScene == "CharacterSelect")
        // {
            PlayerCharacterSelect characterSelect = GameObject.Find("P" + (player.playerIndex + 1).ToString() + " Selected Character").GetComponent<PlayerCharacterSelect>();
            player.transform.GetComponentInChildren<CharacterSelectPlayerController>().Setup(characterSelect, playerColors[player.playerIndex], GameObject.Find("CharacterButton"));
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

    // ----------------
    // CHARACTER SELECT
    // ----------------

    // Give players character select
    public void CharacterSelect()
    {
        currentScene = "CharacterSelect";

        foreach (PlayerInput player in players)
        {
            PlayerCharacterSelect characterSelect = GameObject.Find("P" + (player.playerIndex + 1).ToString() + " Selected Character").GetComponent<PlayerCharacterSelect>();
            player.transform.GetComponentInChildren<CharacterSelectPlayerController>().Setup(characterSelect, playerColors[player.playerIndex], GameObject.Find("CharacterButton"));
        }
    }
}
