using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterSelect : MonoBehaviour
{
    [SerializeField] private PlayerData presets;
    [SerializeField] private Image portrait;
    [SerializeField] private Text name;
    [SerializeField] private GameObject readyUI;

    private bool ready;

    void Start()
    {
        presets = null;
    }

    public PlayerData GetPreset()
    {
        return presets;
    }

    public void UpdateSelection(PlayerData _presets)
    {
        presets = _presets;

        if (presets != null)
        {
            if (presets.portrait != null)
            {
                portrait.color = Color.white;
                portrait.enabled = true;
                portrait.sprite = _presets.portrait;
            }
            else
            {
                portrait.enabled = false;
            }
        }
        else
        {
            // name.text = "";
        }
    }

    public bool SetReady(bool _ready)
    {
        if (presets != null)
        {
            // ready = _ready;
            // readyUI.SetActive(ready);
        }
        else if (!_ready)
        {
            // ready = false;
            // readyUI.SetActive(false);
        }

        return ready;
    }

    public bool IsReady()
    {
        return ready;
    }
}
