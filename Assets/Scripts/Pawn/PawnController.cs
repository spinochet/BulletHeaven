using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PawnController : NetworkBehaviour 
{
    [SerializeField] protected Pawn pawn;
    [SyncVar] public int playerNum = -1;

    public void TogglePause(bool isPaused)
    {
        
    }
}
