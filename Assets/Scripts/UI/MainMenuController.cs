using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private float transitionTime = 0.0f;

    // ---------
    // MAIN MENU
    // ---------

    public void StoryMode()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void ArcadeMode()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void SettingsMode()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
