using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private PlayerController.CharacterPreset presets;
    [SerializeField] private GameObject model;
    [SerializeField] private string name;

    // When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
    void OnTriggerEnter(Collider other)
    {
        PlayerToken player = GameObject.Find(other.gameObject.name + " Token").GetComponent<PlayerToken>();

        if (player)
            player.characterSelect.UpdateSelection(model, name);
    }
}
