using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header ("Movement")]
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float dashForce = 10.0f;
    private Vector2 inputVec;

    [Header ("Combat")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 30.0f;
    [SerializeField] private Ability ability1;
    private bool isShooting;
    private float shootTimer;

    [Header ("Stats")]
    [SerializeField] private int maxHP;
    [SerializeField] private int maxStamina;
    private int hp;
    private int stamina;
    private Slider hpBar;
    private Slider staminaBar;

    [Header ("Animations")]
    [SerializeField] private Sprite portrait;
    [SerializeField] private RuntimeAnimatorController animController;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        hp = maxHP;
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle shooting
        if (isShooting)
        {
            shootTimer += Time.unscaledDeltaTime;
            if (shootTimer >= 1.0f / fireRate)
            {
                shootTimer = 0.0f;
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
        }
    }

    public void SetUp(CharacterPresets.CharacterPreset preset)
    {
        portrait = preset.portrait;
        animController = preset.anim;
        bulletPrefab = preset.bulletPrefab;
        fireRate = preset.fireRate;

        GetComponent<Animator>().runtimeAnimatorController = animController;

        ability1 = preset.ability.GetComponent<Ability>();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetUI(Slider _hpBar, Slider _staminaBar)
    {
        hpBar = _hpBar;
        staminaBar = _staminaBar;
    }

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        // Handle movement
        rb.MovePosition(transform.position + (Vector3) inputVec * movementSpeed * Time.unscaledDeltaTime);
    }

    // Move action callback function
    public void OnMove(InputValue input)
    {
        inputVec = input.Get<Vector2>();
    }

    // Shoot action callback function
    public void OnShoot(InputValue input)
    {
        isShooting = input.Get<float>() > 0.0f;
        shootTimer = fireRate / 60.0f;
    }

    // Ability 1 action callback function
    public void OnAbility1(InputValue input)
    {
        if (input.Get<float>() > 0.0f)
        {
            ability1.Activate();
        }
        else
        {
            ability1.Deactivate();
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        hpBar.value = (float) hp / (float) maxHP;
    }
}
