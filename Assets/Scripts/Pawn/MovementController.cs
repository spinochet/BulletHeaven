using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private float speed;
    [SerializeField] private float dashDist;

    private Vector3 movement;
    private bool isDashing;
    
    // Awake is called when instantiated
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Initiate variables
    public void Setup(float _speed, float _dashDist)
    {
        speed = _speed;
        dashDist = _dashDist;
    }

    // Update is called once every frame
    void Update()
    {
        if (!isDashing)
            controller.Move(movement * speed * Time.unscaledDeltaTime);
    }

    // Move character in XY plane
    public void Move(Vector2 moveVector)
    {
        movement.x = moveVector.x;
        movement.z = moveVector.y;
    }

    public void Dash()
    {
        if (movement.sqrMagnitude > Mathf.Epsilon)
        {
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        float timer = 0.0f;

        while (timer < 0.2f)
        {
            timer += Time.unscaledDeltaTime;

            Vector3 dir = (movement.normalized * dashDist) / 0.2f;
            controller.Move(dir * Time.unscaledDeltaTime);
            yield return null;
        }

        isDashing = false;
    }
}
