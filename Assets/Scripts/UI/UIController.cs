using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject p1;
    [SerializeField] private Slider p1Health;
    [SerializeField] private Slider p1Stamina;

    [SerializeField] private GameObject p2;
    [SerializeField] private Slider p2Health;
    [SerializeField] private Slider p2Stamina;

    // Start is called before the first frame update
    void Start()
    {
        NewPlayerController player1 = GameObject.Find("Princess").GetComponent<NewPlayerController>();
        if (player1 != null)
        {
            player1.SetUI(p1Health, p1Stamina);
        }

        NewPlayerController player2 = GameObject.Find("Player 2").GetComponent<NewPlayerController>();
    }
}
