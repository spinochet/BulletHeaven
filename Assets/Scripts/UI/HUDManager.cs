using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private HUDController p1 = null;
    private HUDController p2 = null;
    public RawImage vingette;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        p1 = transform.Find("P1 Stats").GetComponent<HUDController>();
        p2 = transform.Find("P2 Stats").GetComponent<HUDController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerManager.Instance)
        {
            // Assign player 1 HUD
            if (PlayerManager.Instance.P1)
            {
                p1.gameObject.SetActive(true);
                PlayerManager.Instance.P1.AssignHUD(p1);
            }
            else
            {
                p1.gameObject.SetActive(false);
            }

            // Assign player 2 HUD
            if (PlayerManager.Instance.P2)
            {
                p2.gameObject.SetActive(true);
                PlayerManager.Instance.P2.AssignHUD(p1);
            }
            else
            {
                p2.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Color c = vingette.color;
        // c.a = 0.75f;
        // vingette.color = c;


    }

    // Assign HUD depending on player number
    public void AssignHUD(PlayerController player, int playerNum)
    {
        switch (playerNum)
        {
            case 0:
                p1.gameObject.SetActive(true);
                player.AssignHUD(p1);
                break;
            case 1:
                p2.gameObject.SetActive(true);
                player.AssignHUD(p2);
                break;
            default:
                break;
        }
    }

    // Toggle off hud on and off
    public void ToggleOffHUD(bool toggle)
    {
        p1.ToggleOffHUD(toggle);
        p2.ToggleOffHUD(toggle);
    }
}
