using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private RawImage background;
    [SerializeField] private Text text;

    private bool isPaused;

    public bool TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;

        if (background) background.enabled = isPaused;
        if (text) text.enabled = isPaused;

        return isPaused;
    }
}
