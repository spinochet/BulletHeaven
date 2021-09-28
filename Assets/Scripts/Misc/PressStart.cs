using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class PressStart : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject thing;
    [SerializeField] private GameObject button;

    public void OnSubmit()
    {
        eventSystem.SetSelectedGameObject(button);
        Destroy(thing);
    }
}
