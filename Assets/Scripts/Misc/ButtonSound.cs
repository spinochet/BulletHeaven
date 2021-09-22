using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, ISelectHandler
{
    public AudioSource audio;

    public void OnSelect(BaseEventData eventData)
    {
        audio.Play();
    }

    public void Select()
    {
        audio.Play();
    }
}
