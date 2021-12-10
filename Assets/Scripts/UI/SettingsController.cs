using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class SettingsController : MonoBehaviour
{
    public AudioMixer audio;
    public float threshold = -40.0f;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject button;

    // Start is called before the first frame update
    void Start()
    {
        // audio = PlayerManager.Instance.transform.GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;
    }

    // Close settings menu
    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }

    // Set master volume
    public void SetMasterVolume(float value)
    {
        if (value <= threshold) value = -80.0f;
        audio.SetFloat("Master", value);
    }

    // Set music volume
    public void SetMusicVolume(float value)
    {
        if (value <= threshold) value = -80.0f;
        audio.SetFloat("Music", value);
    }

    // Set sound effects volume
    public void SetSFXValue(float value)
    {
        if (value <= threshold) value = -80.0f;
        audio.SetFloat("SFX", value);
    }

    // Turn the game into windowed mode
    public void ToggleWindowed()
    {
        
    }

    // Turn the game into fullscreen mode
    public void ToggleFullscreen()
    {
        
    }
}
