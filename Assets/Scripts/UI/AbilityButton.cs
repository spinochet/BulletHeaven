using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }

    void Awake()
    {
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }

    public void Ability()
    {
        player.AbilityButton();
    }

    public void Switch()
    {
        player.Switch();
    }
}
