using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : Controller
{
    private CharacterController controller;

    [SerializeField] private float speed;
    [SerializeField] private float dashDist;

    private Vector3 movement;
    
    // Awake is called when instantiated
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once every frame
    void Update()
    {
        if (!IsPaused())
            controller.Move(movement * speed * Time.unscaledDeltaTime);
    }

    // Move character in XY plane
    public void Move(Vector2 moveVector)
    {
        movement.x = moveVector.x;
        movement.z = moveVector.y;
    }
}
