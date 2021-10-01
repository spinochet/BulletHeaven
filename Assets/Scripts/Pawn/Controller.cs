using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected bool isPaused;

    // Toggle pause on and off
    public void TogglePause(bool _isPaused)
    {
        isPaused = _isPaused;
    }

    // Get if script is currently paused
    protected bool IsPaused() { return isPaused; }
}
