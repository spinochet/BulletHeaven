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
            characterSelect.transform.Find("Cover/Text").GetComponent<Text>().enabled = false;

            players[player.playerIndex].Init(null, characterSelect);
            token.Init(player.transform, characterSelect);

            IsReady();
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

    // If all players are ready load level
    public void LoadLevel(string level)
    {
        if (isReady) SceneManager.LoadScene(level);
    }

    // Set up all players for level
    public void SetupPlayers(HUDController[] hud)
    {
        for (int i = 0; i < numPlayers; ++i)
        {
            // Initialize player
            // players[i].Init();

            // Assign hud
            if (numPlayers <= 2)
                players[i].Init(hud[1 + (i * 2)]);
                // players[i].AssignHUD(hud[1 + (i * 2)]);
            else
                players[i].Init(hud[i]);
                // players[i].AssignHUD(hud[i]);

            // Spawn at correct location
            players[i].DisableMovement();
            GameObject spawn = GameObject.Find("P" + i.ToString() + " Spawn");

            if (spawn) players[i].transform.position = spawn.transform.position;
            else players[i].transform.position = new Vector3(-5.0f + (i * 3.33f), 0.0f, -4.0f);
        }

        // Assign huds
        if (numPlayers == 1)
        {
            hud[0].gameObject.SetActive(false);
            hud[2].gameObject.SetActive(false);
            hud[3].gameObject.SetActive(false);
        }
        else if (numPlayers == 2)
        {
            hud[0].gameObject.SetActive(false);
            hud[2].gameObject.SetActive(false);
        }
        else if (numPlayers == 3)
        {
            hud[3].gameObject.SetActive(false);
        }

        StartCoroutine(LevelWait());
    }

    // Delete players
    public void DeletePlayers()
    {
        for (int i = 0; i < numPlayers; ++i)
        {
            Destroy(players[i].gameObject);
        }
    }

    IEnumerator LevelWait()
    {
        float timer = 0.0f;

        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < numPlayers; ++i)
            players[i].EnableMovement();
    }

    public void TogglePause(bool isPaused)
    {
        for (int i = 0; i < numPlayers; ++i)
        {
            if (isPaused)
                players[i].DisableMovement();
            else
                players[i].EnableMovement();
        }
    }
}
