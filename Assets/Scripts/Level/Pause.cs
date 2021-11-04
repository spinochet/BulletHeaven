using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private bool isPaused;

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;

        if (isPaused)
        {
            Time.timeScale = 0.0f;

            // Pause all players
            PlayerController[] players = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
            foreach (PlayerController player in players)
            {
                player.TogglePawnAnimation(false);
                player.enabled = false;
            }
        }
        else
        {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);

            // Pause all players
            PlayerController[] players = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
            foreach (PlayerController player in players)
            {
                player.enabled = true;
                player.TogglePawnAnimation(true);
            }
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu Offline");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
