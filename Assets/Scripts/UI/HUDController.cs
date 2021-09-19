﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private Slider hpBar;
    private Slider staminaBar;
    private RawImage portrait;

    // Start is called before the first frame update
    void Start()
    {
        hpBar = transform.Find("HP Bar").GetComponent<Slider>();
        staminaBar = transform.Find("Stamina Bar").GetComponent<Slider>();
        portrait = transform.Find("Portrait/Character").GetComponent<RawImage>();
    }

    // Update current health
    public void UpdateHealth(float value)
    {
        hpBar.value = value;
    }

    // Update current stamina
    public void UpdateStamina(float value)
    {
        staminaBar.value = value;
    }

    // Update character portrait
    public void UpdatePortrait(Texture texture)
    {
        portrait.texture = texture;
    }
}