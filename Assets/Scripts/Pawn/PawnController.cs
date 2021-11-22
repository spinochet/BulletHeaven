using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

abstract public class PawnController : MonoBehaviour 
{
    [SerializeField] public Pawn pawn;

    abstract public void Destroy(Pawn _pawn);

    // [Command(requiresAuthority = false)]
    // public void SpawnPawn(GameObject _pawn)
    // {
    //     pawn = Instantiate(_pawn).GetComponent<Pawn>();
    //     NetworkServer.Spawn(pawn.gameObject);
    // }
}
