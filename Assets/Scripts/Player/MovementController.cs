using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private CharacterController controller;
    private float speed;

    private Vector3 movement;
    
    // Awake is called when instantiated
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Initiate variables
    public void Setup(float _speed)
    {
        speed = _speed;
    }

    // Update is called once every frame
    void Update()
    {
        controller.Move(movement * speed * Time.unscaledDeltaTime);
    }

    // Move character in XY plane
    public void Move(Vector2 moveVector)
    {
        movement.x = moveVector.x;
        movement.z = moveVector.y;
    }
}
