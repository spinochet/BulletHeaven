using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private bool isCharacterSelect;
    [SerializeField] private GameObject tokenPrefab;

    private int numPlayers;
    private PlayerController[] players;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        players = new PlayerController[4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add player to manager
    public void AddPlayer(PlayerInput player)
    {
        string playerName = "Player " + (player.playerIndex + 1).ToString();
        player.gameObject.name = playerName;
        DontDestroyOnLoad(player.gameObject);

        players[player.playerIndex] = player.transform.GetComponent<PlayerController>();
        players[player.playerIndex].Init();
        ++numPlayers;

        if (isCharacterSelect)
        {
            PlayerToken token = Instantiate(tokenPrefab).GetComponent<PlayerToken>();
            token.gameObject.name = playerName + " Token";

            PlayerCharacterSelect characterSelect = GameObject.Find("P" + (player.playerIndex + 1).ToString() + " Character Select").GetComponent<PlayerCharacterSelect>();
            characterSelect.transform.Find("Cover").GetComponent<RawImage>().enabled = false;

            token.Init(player.transform, characterSelect);
        }
    }

    // Remove players
    public void RemovePlayer(PlayerInput player)
    {
        players[player.playerIndex] = null;
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
