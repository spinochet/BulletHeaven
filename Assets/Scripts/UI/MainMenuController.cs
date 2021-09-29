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
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject playButton;

    [SerializeField] private GameObject modeSelect;
    [SerializeField] private GameObject storyButton;

    [SerializeField] private float transitionTime = 0.0f;

    // ---------
    // MAIN MENU
    // ---------

    public void StartGame()
    {
        StartCoroutine(SlideSelectMode());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator SlideSelectMode()
    {
        modeSelect.SetActive(true);
        float timer = 0.0f;

        Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 targetPosition = new Vector3(-700.0f, 0.0f, 0.0f);

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            main.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, (timer / transitionTime));
            modeSelect.transform.localPosition = main.transform.localPosition - targetPosition;
            yield return null;
        }

        eventSystem.SetSelectedGameObject(storyButton);
    }

    // -----------
    // MODE SELECT
    // -----------

    public void StoryMode()
    {
        GameObject.Find("Player Manager").GetComponent<PlayerManager>().StoryMode();

        // StartCoroutine(LoadStoryLevel());
    }

    IEnumerator LoadStoryLevel()
    {
        // Load level
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Level 1", LoadSceneMode.Single);

        // Wait for level to load
        while (!asyncLoadLevel.isDone)
        {
            Debug.Log("!");
            yield return null;
        }
    }
}
