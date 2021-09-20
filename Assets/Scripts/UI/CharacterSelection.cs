using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private PlayerController.CharacterPreset presets;
    [SerializeField] private string name;
    private bool isSelected;

    // When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
    void OnTriggerEnter(Collider other)
    {
        PlayerToken player = GameObject.Find(other.gameObject.name + " Token").GetComponent<PlayerToken>();

        if (player)
            player.characterSelect.UpdateSelection(presets, this);
    }

    // When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
    void OnTriggerExit(Collider other)
    {
        PlayerToken player = GameObject.Find(other.gameObject.name + " Token").GetComponent<PlayerToken>();

        if (player)
            player.characterSelect.UpdateSelection(null, null);
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void SetSelected(bool _isSelected)
    {
        isSelected = _isSelected;
    }
}
