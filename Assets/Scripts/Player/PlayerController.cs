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
    private int maxLevel = 4;
    public int CurrentLevel { get { return currentLevel; } }

    private int experience = 0;
    [SerializeField] private List<int> experienceNeeded;

    private bool alive = false;
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

    // Touchscreen
    public bool isTouch;
    private bool isTouching;
    private Vector2 touchPosition;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        DontDestroyOnLoad(this);
        maxLevel = experienceNeeded.Count;
        alive = true;
    }

    void Update()
    {

        if (pawn)
        {
            // Timers
            fireTimer += Time.unscaledDeltaTime;

            if (!isTouch)
            {
                // Movement
                pawn.Move(movement);

                // Combat
                if (isShooting && fireTimer > 1.0f / pawn.FireRate)
                {
                    fireTimer = 0.0f;
                    pawn.Shoot(transform.rotation);
                }
            }
            else if (isTouching)
            {
                // Movement
                Vector3 pawnPosition = Camera.main.WorldToScreenPoint(pawn.transform.position);
                movement.x = touchPosition.x - pawnPosition.x;
                movement.z = touchPosition.y - pawnPosition.y;

                if (movement.magnitude > 5.0f)
                    movement.Normalize();
                else
                    movement = Vector3.zero;

                pawn.Move(movement);

                // Combat
                if (fireTimer > 1.0f / pawn.FireRate)
                {
                    fireTimer = 0.0f;
                    pawn.Shoot(transform.rotation);
                }
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
            hud.UpdateHealth(0.0f);

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

            hud.UpdateHealth(0.0f);
            hud.UpdateStamina(0.0f);

            PlayerManager.Instance.CheckPlayersAlive();
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

    // MoveTouch action callback
    void OnMoveTouch(InputValue input)
    {
        Debug.Log("MoveTouch");
        touchPosition = input.Get<Vector2>();
    }

    // Touch action callback
    void OnTouch(InputValue input)
    {
        Debug.Log("Touch");
        isTouching = input.Get<float>() > 0.0f ? true : false;
    }

    // Tap action callback
    void OnTap(InputValue input)
    {
        // if (pawn)
        //     OnSwitch();
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
        if (pawn && enabled)
        {
            if (input.Get<float>() > 0.0f) pawn.ActivateAbility(0);
            else pawn.DeactivateAbility(0);
        }
    }

    // AbilityR action callback function
    void OnAbilityR(InputValue input)
    {
        if (pawn && enabled)
        {
            if (input.Get<float>() > 0.0f) pawn.ActivateAbility(1);
            else pawn.DeactivateAbility(1);
        }
    }

    // AbilityL action callback function
    void SecondTap(InputValue input)
    {
        if (pawn && enabled)
        {
            if (input.Get<float>() > 0.0f) pawn.ActivateAbility(0);
            else pawn.DeactivateAbility(0);
        }
    }

    public bool AbilityButton()
    {
        if (pawn && enabled)
        {
            if (!pawn.isAbilityL)
            {
                pawn.ActivateAbility(0);
                Debug.Log("Activate");
            }
            else
            {
                pawn.DeactivateAbility(0);
                Debug.Log("Deactivate");
            }

            return pawn.isAbilityL;
        }

        return false;
    }

    // ------
    // EVENTS
    // ------

    // Pause action callback function
    void OnPause()
    {
        if (pawn != null && enabled)
            hud.TogglePause();
    }

    public void TogglePawnAnimation(bool playing)
    {
        pawn.ToggleAnimation(playing);
    }

    public void Switch()
    {
        OnSwitch();
    }

    // Switch action callback
    void OnSwitch()
    {
        if (pawn.partner != null && enabled)
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
        experience += points;

        pawn.ConsumeStamina(-5.0f);

        if (currentLevel < maxLevel && experienceNeeded[currentLevel] < experience)
        {

            experience = 0;
            currentLevel++;
            Debug.Log("Current Level: " + currentLevel);
        }
    }

    public void LosePoints()
    {
        score /= 2;
        alive = true;
    }

    public void ResetPoints()
    {
        score = 0;

        experience = 0;
        currentLevel = 0;
    }

    public Pawn GetPawn() {
        return pawn;
    }
}
