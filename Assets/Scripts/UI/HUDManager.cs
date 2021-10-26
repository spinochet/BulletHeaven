﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private HUDController p1;
    [SerializeField] private HUDController p2;
    [SerializeField] private HUDController p3;
    [SerializeField] private HUDController p4;

    public void AssignHUD(Pawn pawn, int player)
    {
        switch (player)
        {
            case 0:
                p1.gameObject.SetActive(true);
                pawn.AssignHUD(p1);
                break;
            case 1:
                p2.gameObject.SetActive(true);
                pawn.AssignHUD(p2);
                break;
            case 2:
                p3.gameObject.SetActive(true);
                pawn.AssignHUD(p3);
                break;
            case 3:
                p4.gameObject.SetActive(true);
                pawn.AssignHUD(p4);
                break;
            default:
                break;
        }
    }
}
