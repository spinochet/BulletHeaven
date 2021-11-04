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
            Pawn newPawn = _pawn.partner.GetComponent<Pawn>();
            newPawn.SetVisibility(true);
            PossesPawn(newPawn);
        }
        else
        {
            // Something something...
            // Game over...
            // Something something...
        }

        Destroy(_pawn.gameObject);
    }

    // -----
    // SETUP
    // -----

    // Assign pawn to controller over the network
    public void PossesPawn(Pawn _pawn)
    {
        pawn = _pawn;
        pawn.pawnController = this;

        if (pawn.partner)
            partner = pawn.partner.GetComponent<Pawn>();

        if (hud)
        {
            hud.UpdatePortrait(pawn.Portrait.texture);
            if (partner != null)
                hud.UpdateOffPortrait(partner.Portrait.texture);
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

    // Pause action callback function
    void OnPause()
    {
        // if ((manager.mode == NetworkManagerMode.Offline || this.isLocalPlayer) && manager)
        //     manager.TogglePause(this);
        // LevelManager.Instance.TogglePause();
    }

    // Switch action callback
    void OnSwitch()
    {
        Pawn newPawn = pawn.partner.GetComponent<Pawn>();
        pawn.pawnController = null;
        pawn.SetVisibility(false);

        // if (pawn.partner)
        //     partner = pawn.partner.GetComponent<Pawn>();

        PossesPawn(newPawn);
        newPawn.SetVisibility(true);

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
