using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseObject;
    [SerializeField] private GameObject settings;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject settingsButton;
    private bool isPaused;

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseObject.SetActive(isPaused);

        if (isPaused) ToggleOn();
        else ToggleOff();

        SoundManager.Instance.ToggleHighPass(isPaused);
    }

    void ToggleOn()
    {
        eventSystem.SetSelectedGameObject(button);
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

    public void Restart()
    {
        LevelManager level = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
        if (level)
            level.RestoreCheckpoint();

        Time.timeScale = 1.0f;

        PlayerController[] players = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
        foreach (PlayerController player in players)
        {
            player.LosePoints();
        }

        TogglePause();
    }

    public void Settings()
    {
        settings.SetActive(true);
        eventSystem.SetSelectedGameObject(settingsButton);
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
