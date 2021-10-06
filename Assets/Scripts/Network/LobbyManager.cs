using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private PlayerManager manager;
    private string code;

    [SerializeField] private NetworkIdentity p1;
    [SerializeField] private NetworkIdentity p2;
    [SerializeField] private NetworkIdentity p3;
    [SerializeField] private NetworkIdentity p4;

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