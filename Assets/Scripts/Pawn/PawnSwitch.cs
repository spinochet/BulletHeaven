using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSwitch : MonoBehaviour
{
    [SerializeField] private List<GameObject> pawns;
    private int active = 0;

    public GameObject Switch(int newActive)
    {
        pawns[active].SetActive(false);
        pawns[newActive].SetActive(true);
        active = newActive;

        return pawns[active];
    }
}
