using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    private HUDController hud;

    private float maxHP;
    private float maxStamina;
    private float hp;
    private float stamina;
    private float hpRegenRate;
    private float staminaRegenRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Initiate variables
    public void Setup(float _maxHP, float _maxStamina, float _hpRegenRate, float _staminaRegenRate)
    {
        maxHP = _maxHP;
        maxStamina = _maxStamina;
        hp = maxHP;
        stamina = maxStamina;
        hpRegenRate = _hpRegenRate;
        staminaRegenRate = _staminaRegenRate;
    }

    // Assign player's HUD
    public void AssignHUD(HUDController _hud)
    {
        hud = _hud;
    }

    // Update is called once every frame
    void Update()
    {
        // Recover health and stamina
        hp += hpRegenRate * Time.unscaledDeltaTime;
        stamina += staminaRegenRate * Time.unscaledDeltaTime;

        // Update HUD
        if (hud)
        {
            hud.UpdateHealth(hp / maxHP);
            hud.UpdateStamina(stamina / maxStamina);
        }
    }

    // Return current health
    public float GetHealth()
    {
        return hp;
    }

    // Return current health
    public float GetStamina()
    {
        return stamina;
    }

    // Add health to player
    public void ModifyHealth(float value)
    {
        hp += value;
    }

    // Consume player stamina
    public void ConsumeStamina(float value)
    {
        stamina += value;
    }
}
