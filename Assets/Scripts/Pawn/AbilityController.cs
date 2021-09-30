using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [SerializeField] private Ability abilityL;
    [SerializeField] private Ability abilityR;

    private StatsController statsController;
    private bool isAbilityL;
    private bool isAbilityR;

    void Awake()
    {
        // bullet = bulletPrefab.GetComponent<Bullet>();
        // burst = burstPrefab.GetComponent<BurstBullet>();
        statsController = GetComponent<StatsController>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (isAbilityL && isAbilityR)
        // {
        //     Debug.Log("Waiting for team attack");
        // }
        if (isAbilityL)
        {
            statsController.ConsumeStamina(abilityL.GetCost() * Time.unscaledDeltaTime);

            if (statsController.GetStamina() <= 0.0f)
                abilityL.Deactivate();
        }
        else if (isAbilityR && abilityR != null)
        {
            statsController.ConsumeStamina(abilityR.GetCost() * Time.unscaledDeltaTime);

            if (statsController.GetStamina() <= 0.0f)
                abilityR.Deactivate();
        }
    }

    // Activate chosen ability
    public void Activate(int i)
    {
        if (i == 0 && abilityL != null && statsController.GetStamina() > abilityL.GetCost())
        {
            isAbilityL = abilityL.Activate();

            if (!isAbilityL)
            {
                statsController.ConsumeStamina(abilityL.GetCost());
            }
        }
        else if (i == 1 && abilityR != null && statsController.GetStamina() > abilityR.GetCost())
        {
            isAbilityR = abilityR.Activate();

            if (!isAbilityR)
            {
                statsController.ConsumeStamina(abilityR.GetCost());
            }
        }
    }

    // Deactivate chosen ability
    public void Deactivate(int i)
    {
        if (i == 0 && isAbilityL)
        {
            isAbilityL = false;
            abilityL.Deactivate();
        }
        else if (i == 1 && isAbilityR)
        {
            isAbilityR = false;
            abilityR.Deactivate();
        }
    }
}
