using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PawnController : NetworkBehaviour 
{
    [SyncVar] public Pawn pawn;

    // [Command(requiresAuthority = false)]
    // public void SpawnPawn(GameObject _pawn)
    // {
    //     pawn = Instantiate(_pawn).GetComponent<Pawn>();
    //     NetworkServer.Spawn(pawn.gameObject);
    // }
}
