using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // [SerializeField] private EventSystem eventSystem;
    [SerializeField] private string firstLevel;
    [SerializeField] private GameObject settings;

    // ---------
    // MAIN MENU
    // ---------

    public void StoryMode()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void ArcadeMode()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void SettingsMode()
    {
        settings.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
