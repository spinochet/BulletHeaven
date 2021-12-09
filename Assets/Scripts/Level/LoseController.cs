using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoseController : MonoBehaviour
{
    [SerializeField] private GameObject lose;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject button;

    public void ToggleLose(bool on)
    {
        lose.SetActive(on);
        if (on)
        {
            LevelManager level = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
            if (level)
                level.ToggleScrolling(false);

            Time.timeScale = 0.0f;

            eventSystem.SetSelectedGameObject(button);
        }
    }

    // Restore previous checkpoint
    public void TryAgain()
    {
        LevelManager level = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
        if (level)
            level.RestoreCheckpoint();

        lose.SetActive(false);
        Time.timeScale = 1.0f;

        PlayerController[] players = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
        foreach (PlayerController player in players)
        {
            player.LosePoints();
        }

        SoundManager.Instance.ToggleHighPass(false);
    }

    // Go back to main menu
    public void GiveUp()
    {
        SceneManager.LoadScene("MainMenu Offline");
        lose.SetActive(false);
        Time.timeScale = 1.0f;

        PlayerController[] players = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
        foreach (PlayerController player in players)
        {
            player.ResetPoints();
        }
    }
}
