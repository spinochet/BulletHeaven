using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private HUDController[] hud;
    [SerializeField] private GameObject debug;

    // Start is called before the first frame update
    void Start()
    {
        GameObject manager = GameObject.Find("Player Manager");
        if (manager)
        {
            manager.GetComponent<PlayerManager>().SetupPlayers(hud);
        }
        else
        {
            Instantiate(debug);
            hud[0].gameObject.SetActive(false);
            hud[2].gameObject.SetActive(false);
            hud[3].gameObject.SetActive(false);
        }

        Destroy(gameObject);
    }
}
