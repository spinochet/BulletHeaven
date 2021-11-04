// using System.Collections;
// using System.Collections.Generic;

// using UnityEngine;
// using Mirror;
// using Steamworks;

// public class SteamLobby : MonoBehaviour
// {
//     public bool isSteam;

//     protected Callback<LobbyCreated_t> lobbyCreated;
//     protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
//     protected Callback<LobbyEnter_t> lobbyEntered;

//     private PlayerNetworkManager manager;
//     private const string HostAdressKey = "HostAdress";

//     // Start is called before the first frame update
//     void Start()
//     {
//         manager = GetComponent<PlayerNetworkManager>();

//         if (!SteamManager.Initialized) { return; }

//         lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
//         gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
//         lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
//     }

//     public void HostLobby()
//     {
//         if (isSteam)
//         {
//             SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
//         }
//         else
//         {
//             manager.StartHost();
//             manager.LoadLobby();
//         }

//     }

//     private void OnLobbyCreated(LobbyCreated_t callback)
//     {
//         if (callback.m_eResult != EResult.k_EResultOK)
//         {
//             return;
//         }

//         manager.StartHost();
//         manager.LoadLobby();

//         SteamMatchmaking.SetLobbyData(
//             new CSteamID(callback.m_ulSteamIDLobby),
//             HostAdressKey,
//             SteamUser.GetSteamID().ToString());
//     }

//     private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
//     {
//         SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
//     }

//     private void OnLobbyEntered(LobbyEnter_t callback)
//     {
//         if (NetworkServer.active) { return; }

//         string hostAdress = SteamMatchmaking.GetLobbyData(
//             new CSteamID(callback.m_ulSteamIDLobby),
//             HostAdressKey);

//         manager.networkAddress = hostAdress;
//         manager.StartClient();
//     }
// }
