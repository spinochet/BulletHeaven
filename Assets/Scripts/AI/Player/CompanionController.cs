using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : MonoBehaviour
{
    // Controller scripts for player mechanics
    private MovementController movementController;
    private StatsController statsController;
    private BulletController bulletController;
    private AbilityController abilityController;
    private HUDController hudController;

    // TEMP BEHAVIOR
    float timer;
    int score;
    Vector2 movement;

    // Spawn players in level
    public void AssignPawn(GameObject pawn, HUDController hud = null)
    {
        // Set up player HUD
        if (hud != null)
            hudController = hud;
        if (hudController != null)
            hudController.UpdatePortrait(pawn.GetComponent<CharacterData>().portrait.texture);

        // Set up player movement
        movementController = pawn.GetComponent<MovementController>();

        // Set up player stats
        statsController = pawn.GetComponent<StatsController>();
        statsController.AssignHUD(hudController);

        // Set up player bullets
        bulletController = pawn.GetComponent<BulletController>();
        bulletController.AssignOwner(this);
        bulletController.StopShooting();

        // Set up player abilities
        abilityController = pawn.GetComponent<AbilityController>();
        abilityController.Deactivate(0);
        abilityController.Deactivate(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.0f)
        {
            timer += Time.unscaledDeltaTime;

            if (timer > 0.5f)
            {
                timer = 0.0f;

                // Random movement
                float dir = Random.Range(0.0f, 1.0f);
                if (dir < 0.25f)
                    movement = Vector2.left;
                else if (dir < 0.5f)
                    movement = Vector2.right;
                else
                    movement = Vector2.zero;

                // Random shooting
                float shoot = Random.Range(0.0f, 1.0f);
                if (shoot < 0.6f)
                    bulletController.StartShooting();
                else
                    bulletController.StopShooting();
            }

            movementController.Move(movement);
        }
        else
        {
            movementController.Move(Vector3.zero);
            bulletController.StopShooting();
        }
    }

    public void AddPoints()
    {
        score += 100;
        hudController.UpdateScore(score);
    }

    // ----
    // TEMP
    // ----

    [SerializeField] private int health = 10;

    public void Damage()
    {
        statsController.ModifyHealth(-10);
    }
}
