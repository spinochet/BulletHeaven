using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Text text;

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        text.text = "";
    }

    //Do this when the selectable UI object is deselected.
    public void OnDeselect(BaseEventData eventData)
    {
        text.text = "Press start to join.";
    }
}
