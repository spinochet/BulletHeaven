using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject p1 = GameObject.Find("Player 1");
        if (p1 != null)
        {
            p1.GetComponent<SpriteRenderer>().enabled = true;
            p1.GetComponent<PlayerInput>().currentActionMap = p1.GetComponent<PlayerInput>().actions.FindActionMap("Gameplay");
        }

        GameObject p2 = GameObject.Find("Player 2");
        if (p2 != null)
        {
            p2.GetComponent<SpriteRenderer>().enabled = true;
            p2.GetComponent<PlayerInput>().currentActionMap = p2.GetComponent<PlayerInput>().actions.FindActionMap("Gameplay");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
