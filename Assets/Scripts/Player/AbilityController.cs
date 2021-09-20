using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    private StatsController statsController;
    private Ability abilityL;
    private Ability abilityR;

    private bool isAbilityL;
    private bool isAbilityR;

    // Initiate variables
    public void Setup(StatsController _statsController, Ability _abilityL, Ability _abilityR)
    {
        statsController = _statsController;
        abilityL = _abilityL;
        abilityR = _abilityR;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAbilityL)
        {
            statsController.ConsumeStamina(abilityL.GetCost() * Time.unscaledDeltaTime);

            if (statsController.GetStamina() <= 0.0f)
                abilityL.Deactivate();
        }

        if (isAbilityR)
        {
            statsController.ConsumeStamina(abilityR.GetCost() * Time.unscaledDeltaTime);

            if (statsController.GetStamina() <= 0.0f)
                abilityR.Deactivate();
        }
    }

    // Activate chosen ability
    public void Activate(int i)
    {
        if (i == 0 && statsController.GetStamina() > abilityL.GetCost())
        {
            isAbilityL = abilityL.Activate();

            if (!isAbilityL)
            {
                statsController.ConsumeStamina(abilityL.GetCost());
            }
        }
        else if (i == 1 && statsController.GetStamina() > abilityR.GetCost())
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
