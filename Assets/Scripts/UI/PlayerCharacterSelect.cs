using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterSelect : MonoBehaviour
{
    [SerializeField] private PlayerController.CharacterPreset presets;
    [SerializeField] private Text name;
    [SerializeField] private GameObject readyUI;
    private bool ready;

    private GameObject model;
    private Vector3 position = new Vector3(-122.5f, 0.0f, -250.0f);
    private Quaternion rotation = Quaternion.Euler(0.0f, 15.0f, 0.0f);
    private Vector3 scale = new Vector3(65.0f, 65.0f, 65.0f);

    public CharacterSelection character;

    void Start()
    {
        presets = null;
        character = null;
    }

    public PlayerController.CharacterPreset GetPreset()
    {
        return presets;
    }

    public void UpdateSelection(PlayerController.CharacterPreset _presets, CharacterSelection _character)
    {
        if (model)
            Destroy(model);

        presets = _presets;
        character = _character;

        if (presets != null)
        {
            model = Instantiate(presets.model, transform);
            model.transform.localPosition = position;
            model.transform.localRotation = rotation;
            model.transform.localScale = scale;
            name.text = presets.name;
        }
        else
        {
            name.text = "";
        }
    }

    public bool SetReady(bool _ready)
    {
        if (character != null && !character.IsSelected() && presets != null)
        {
            ready = _ready;
            readyUI.SetActive(ready);
            character.SetSelected(true);
        }
        else if (!_ready)
        {
            if (character != null && !_ready)
            {
                character.SetSelected(false);
            }

            ready = false;
            readyUI.SetActive(false);
        }

        return ready;
    }

    public bool IsReady()
    {
        return ready;
    }
}
