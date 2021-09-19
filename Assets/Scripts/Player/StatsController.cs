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
    private float hpRegenCooldown;
    private float staminaRegenCooldown;

    private float hpTimer;
    private float staminaTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Initiate variables
    public void Setup(float _maxHP, float _maxStamina, float _hpRegenRate, float _staminaRegenRate, float _hpRegenCooldown, float _staminaRegenCooldown)
    {
        maxHP = _maxHP;
        maxStamina = _maxStamina;
        hp = maxHP;
        stamina = maxStamina;
        hpRegenRate = _hpRegenRate;
        staminaRegenRate = _staminaRegenRate;
        hpRegenCooldown = _hpRegenCooldown;
        staminaRegenCooldown = _staminaRegenCooldown;
    }

    // Assign player's HUD
    public void AssignHUD(HUDController _hud)
    {
        hud = _hud;
    }

    // Update is called once every frame
    void Update()
    {
        hpTimer += Time.unscaledDeltaTime;
        staminaTimer += Time.unscaledDeltaTime;

        // Recover health and stamina
        if (hp < maxHP && hpTimer > hpRegenCooldown)
            hp += hpRegenRate * Time.unscaledDeltaTime;
        if (stamina < maxStamina && staminaTimer > staminaRegenCooldown) 
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
        hpTimer = 0.0f;
    }

    // Consume player stamina
    public void ConsumeStamina(float value)
    {
        stamina -= value;
        staminaTimer = 0.0f;
    }
}
