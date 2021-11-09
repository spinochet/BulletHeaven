using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : PawnController
{
    // Character and player data
    private Color playerColor;
    private Pawn partner;
    private int currentLevel = 0;
    public int CurrentLevel { get { return currentLevel; } }

    public bool alive = false;
    public bool IsAlive { get { return alive; }}
    private int score = 0;

    // Movement
    private Vector3 movement = Vector3.zero;
    private float speed = 10.0f;
    private bool canMove = true;

    // Stats
    private HUDController hud;

    // Combat
    private bool isShooting = false;
    private float fireTimer = 0.0f;

    // Abilities
    private bool isAbilityL;
    private bool isAbilityR;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (pawn)
        {
            // Movement
            pawn.Move(movement);

            // Combat
            fireTimer += Time.unscaledDeltaTime;
            if (isShooting && fireTimer > 1.0f / pawn.FireRate)
            {
                fireTimer = 0.0f;
                pawn.Shoot(transform.rotation);
            }

            // Stats
            if (hud != null)
            {
                hud.UpdateHealth(pawn.GetHP());
                hud.UpdateStamina(pawn.GetStamina());
                hud.UpdateScore(score);

                if (partner)
                {
                    hud.UpdateOffHealth(partner.GetHP());
                    hud.UpdateOffStamina(partner.GetStamina());
                }
            }
        }
    }

    public override void Destroy(Pawn _pawn)
    {
        if (_pawn.partner)
        {
            // Deactivate abilities
            pawn.DeactivateAbility(0);
            pawn.DeactivateAbility(1);

            Pawn newPawn = _pawn.partner.GetComponent<Pawn>();
            newPawn.partner = null;
            newPawn.SetVisibility(true);

            Destroy(_pawn.gameObject);
            PossesPawn(newPawn);

            hud.ToggleOffHUD(false);
        }
        else
        {
            Destroy(_pawn.gameObject);
            alive = false;
            
            PlayerManager.Instance.CheckPlayersAlive();

            // LevelManager level = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
            // level.RestoreCheckpoint();

            // hud.ToggleLose(true);
            // hud.ToggleOffHUD(true);
        }
    }

    // -----
    // SETUP
    // -----

    // Assign pawn to controller over the network
    public void PossesPawn(Pawn _pawn)
    {
        if (pawn)
        {
            pawn.DeactivateAbility(0);
            pawn.DeactivateAbility(1);
        }

        pawn = _pawn;
        pawn.pawnController = this;

        if (pawn.partner)
            partner = pawn.partner.GetComponent<Pawn>();

        if (hud)
        {
            hud.UpdatePortrait(pawn.Portrait.texture);
            if (partner != null)
                hud.UpdateOffPortrait(partner.Portrait.texture);
            else
                hud.ToggleOffHUD(false);
        }

        // Switch input map
        PlayerInput input = GetComponent<PlayerInput>();
        if (input) input.SwitchCurrentActionMap("Gameplay");
    }

    // Assign player's HUD
    public void AssignHUD(HUDController _hud)
    {
        hud = _hud;
    }

    // ---------------
    // EVENT CALLBACKS
    // ---------------

    // Move action callback
    void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        movement.x = inputVec.x;
        movement.z = inputVec.y;
    }

    // -------------
    // COMBAT EVENTS
    // -------------

    // Shoot action callback
    void OnShoot(InputValue input)
    {
        isShooting = input.Get<float>() > 0.0f ? true : false;
    }

    // --------------
    // ABILITY EVENTS
    // --------------

    // AbilityL action callback function
    void OnAbilityL(InputValue input)
    {
        if (pawn)
        {
            if (input.Get<float>() > 0.0f) pawn.ActivateAbility(0);
            else pawn.DeactivateAbility(0);
        }
    }

    // AbilityR action callback function
    void OnAbilityR(InputValue input)
    {
        if (pawn)
        {
            if (input.Get<float>() > 0.0f) pawn.ActivateAbility(1);
            else pawn.DeactivateAbility(1);
        }
    }

    // ------
    // EVENTS
    // ------

    // Pause action callback function
    void OnPause()
    {
        if (pawn != null)
            hud.TogglePause();
    }

    public void TogglePawnAnimation(bool playing)
    {
        pawn.ToggleAnimation(playing);
    }

    // Switch action callback
    void OnSwitch()
    {
        if (pawn.partner != null)
        {
            Pawn newPawn = pawn.partner.GetComponent<Pawn>();
            pawn.pawnController = null;
            pawn.SetVisibility(false);

            PossesPawn(newPawn);
            newPawn.SetVisibility(true);
        }
    }

    // -----
    // SCORE
    // -----

    public void AddPoints(int points)
    {
        score += points;
    }

    public void ResetPoints()
    {
        // score = 0;
        // if (hudController)
        //     hudController.UpdateScore(score);
    }
}
