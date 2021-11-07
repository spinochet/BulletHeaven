using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseObject;
    private bool isPaused;

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseObject.SetActive(isPaused);

        if (isPaused) ToggleOn();
        else ToggleOff();
    }

    void ToggleOn()
    {
        pauseObject.SetActive(true);
        Time.timeScale = 0.0f;

        // Pause all players
        PlayerController[] players = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
        foreach (PlayerController player in players)
        {
            player.TogglePawnAnimation(false);
            player.enabled = false;
        }
    }

    void ToggleOff()
    {
        pauseObject.SetActive(false);
        Time.timeScale = 1.0f;

        // Pause all players
        PlayerController[] players = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
        foreach (PlayerController player in players)
        {
            player.enabled = true;
            player.TogglePawnAnimation(true);
        }
    }

    public void MainMenu()
    {
        ToggleOff();
        SceneManager.LoadScene("MainMenu Offline");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
