using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class CharacterSelectPlayerController : MonoBehaviour
{
    private MultiplayerEventSystem eventSystem;
    private PlayerCharacterSelect characterSelection;
    private Color playerColor;
    
    private bool cooldown;
    private bool isReady;

    public void Setup(PlayerCharacterSelect _characterSelection, Color color, GameObject firstSelected)
    {
        characterSelection = _characterSelection;
        playerColor = color;
        Debug.Log(color);

        eventSystem = GetComponentInChildren<MultiplayerEventSystem>();
        eventSystem.SetSelectedGameObject(firstSelected);

        firstSelected.GetComponent<CharacterSelectButton>().SetColor(playerColor);
        characterSelection.UpdateSelection(eventSystem.currentSelectedGameObject.GetComponent<PlayerData>());
    }

    private void OnMove(InputValue input)
    {
        Vector2 dir = input.Get<Vector2>();

        if (!isReady && !cooldown)
        {
            if (dir.magnitude > 0.0f) cooldown = true;

            CharacterSelectButton current = eventSystem.currentSelectedGameObject.GetComponent<CharacterSelectButton>();
            if (current != null)
            {
                CharacterSelectButton next = current.Find((Vector3) dir);

                if (next && next != current)
                {
                    eventSystem.SetSelectedGameObject(next.gameObject);
                    next.SetColor(playerColor);
                }
            }
        }
        else if (dir.magnitude == 0.0f)
        {
            cooldown = false;
        }

        characterSelection.UpdateSelection(eventSystem.currentSelectedGameObject.GetComponent<PlayerData>());
    }

    private void OnSubmit()
    {
        if (!isReady)
        {
            eventSystem.currentSelectedGameObject.GetComponent<CharacterSelectButton>().SetColor(playerColor);
            eventSystem.currentSelectedGameObject.GetComponent<CharacterSelectButton>().Press(playerColor);
        }

        isReady = true;
    }

    private void OnCancel()
    {
        if (isReady)
        {
            eventSystem.currentSelectedGameObject.GetComponent<CharacterSelectButton>().SetColor(playerColor);
            eventSystem.currentSelectedGameObject.GetComponent<CharacterSelectButton>().Cancel();
        }

        isReady = false;
    }
}
