using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private PlayerNetworkManager manager;

    [Header ("Player Sprites")]
    [SerializeField] private SpriteRenderer defaultBackgound;
    [SerializeField] private SpriteRenderer p1Portrait;
    [SerializeField] private SpriteRenderer p2Portrait;
    [SerializeField] private SpriteRenderer p3Portrait;
    [SerializeField] private SpriteRenderer p4Portrait;

    private string code = "6969";

    [SyncVar] private NetworkIdentity p1;
    [SyncVar] private NetworkIdentity p2;
    [SyncVar] private NetworkIdentity p3;
    [SyncVar] private NetworkIdentity p4;

    void Awake()
    {
        if (!manager)
            manager = GameObject.Find("PlayerManager").GetComponent<PlayerNetworkManager>();
    }

    void Update()
    {
        p1Portrait.enabled = p1 != null;
        p2Portrait.enabled = p2 != null;
        p3Portrait.enabled = p3 != null;
        p4Portrait.enabled = p4 != null;
    }

    public void JoinLobby(NetworkIdentity client)
    {
        if (!p1)
        {
            p1Portrait.enabled = true;
            p1 = client;
        }
        else if (!p2)
        {
            p2Portrait.enabled = true;
            p2 = client;
        }
        else if (!p3)
        {
            p3Portrait.enabled = true;
            p3 = client;
        }
        else if (!p4)
        {
            p2Portrait.enabled = true;
            p4 = client;
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Level 1");
    }

    public NetworkIdentity[] GetPlayers()
    {
        NetworkIdentity[] players = new NetworkIdentity[4];
        players[0] = p1;
        players[1] = p2;
        players[2] = p3;
        players[3] = p4;
        return players;
    }

    [Command(requiresAuthority = false)]
    public void StartGame()
    {
        manager.LoadArcadeLevel(1);
    }
}