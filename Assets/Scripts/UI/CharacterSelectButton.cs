using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectButton : Button
{
    private bool isSelected;
    private bool isReady;

    // Select character
    public override void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
    }

    // Set players color
    public void SetColor(Color color)
    {
        Image frame = transform.Find("Frame").GetComponent<Image>();
        if (frame)
            frame.color = color;
    }

    // Deselect character
    public override void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;

        Image frame = transform.Find("Frame").GetComponent<Image>();
        if (frame)
            frame.color = Color.white;
    }

    // Is button currently selected by anyone
    public bool IsSelected()
    {
        return isSelected;
    }

    // Custom UI navigation
    public CharacterSelectButton Find(Vector3 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) dir.y = 0.0f;
        else dir.x = 0.0f;
        dir.Normalize();

        CharacterSelectButton selectable = (CharacterSelectButton) FindSelectable(dir);
        while (selectable && selectable.IsSelected())
        {
            selectable = (CharacterSelectButton) selectable.FindSelectable(dir);
        }
        return selectable;
    }

    public void Press(Color color)
    {
        isReady = true;
        image.color = color;
    }

    public void Cancel()
    {
        isReady = false;
        image.color = Color.white;
    }
}
