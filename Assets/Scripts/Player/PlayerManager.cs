using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private bool isCharacterSelect;

    // Character select variables
    [SerializeField] private GameObject tokenPrefab;
    [SerializeField] private GameObject readyBanner;
    private bool isReady;

    private int numPlayers;
    private PlayerController[] players;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        players = new PlayerController[4];
    }

    // Add player to manager
    public void AddPlayer(PlayerInput player)
    {
        string playerName = "Player " + (player.playerIndex + 1).ToString();
        player.gameObject.name = playerName;
        DontDestroyOnLoad(player.gameObject);

        players[player.playerIndex] = player.transform.GetComponent<PlayerController>();
        players[player.playerIndex].AssignManager(this);
        ++numPlayers;

        if (isCharacterSelect)
        {
            PlayerToken token = Instantiate(tokenPrefab).GetComponent<PlayerToken>();
            token.gameObject.name = playerName + " Token";

            PlayerCharacterSelect characterSelect = GameObject.Find("P" + (player.playerIndex + 1).ToString() + " Character Select").GetComponent<PlayerCharacterSelect>();
            characterSelect.transform.Find("Cover").GetComponent<RawImage>().enabled = false;

            players[player.playerIndex].Init(null, characterSelect);
            token.Init(player.transform, characterSelect);
        }
    }

    // Remove players
    public void RemovePlayer(PlayerInput player)
    {
        players[player.playerIndex] = null;
        --numPlayers;
    }

    // Check if all players have selected their character
    public void IsReady()
    {
        isReady = true;

        for (int i = 0; i < numPlayers; ++i)
        {
            isReady = isReady && players[i].IsReady();
        }

        readyBanner.SetActive(isReady);
    }

    // If all players are ready load first level
    public void StartGame()
    {
        if (isReady)
        {
            SceneManager.LoadScene("CharacterController");
            for (int i = 0; i < numPlayers; ++i)
            {
                players[i].Init();
            }
        }
    }

    // Load HUD
    public void LoadHUD()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (players[i])
            {
                GameObject hud = GameObject.Find("P" + (i + 1).ToString() + " Stats");
                players[i].AssignHUD(hud.GetComponent<HUDController>());
            }
        }
    }
}
