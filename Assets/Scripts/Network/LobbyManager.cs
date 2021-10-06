using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private PlayerManager manager;

    void Awake()
    {
        // if (!manager)
        //     manager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        // manager.StartClient();
    }

    public void CreateLobby()
    {
        
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Level 1");
    }
}