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

        GameObject.Find("Player Manager").GetComponent<PlayerManager>().TogglePause(isPaused);
        gameObject.SetActive(isPaused);
    }

    public void MainMenu()
    {
        GameObject.Find("Player Manager").GetComponent<PlayerManager>().DeletePlayers();
        Destroy(GameObject.Find("Player Manager"));
        
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
