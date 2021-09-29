using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    private HUDController hud;

    [SerializeField] private float maxHP;
    [SerializeField] private float hpRegenRate;
    [SerializeField] private float hpRegenCooldown;

    [Space (10)]
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRegenRate;
    [SerializeField] private float staminaRegenCooldown;

    public bool destroy;

    private float hp;
    private float stamina;
    private float hpTimer;
    private float staminaTimer;

    // Start is called before the first frame update
    void Awake()
    {
        hp = maxHP;
        stamina = maxStamina;
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
        Debug.Log("Here");
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

        if (hp <= 0 && destroy) Destroy(gameObject);
    }

    // Consume player stamina
    public void ConsumeStamina(float value)
    {
        stamina -= value;
        staminaTimer = 0.0f;
    }
}
