using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    Rigidbody2D playerRigidbody;
    Vector2 movementVector;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;

    void Awake() 
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void OnEnable() 
    {
        jumpAction.performed += Jump;
    }

    void OnDisable() 
    {
        jumpAction.performed -= Jump;
    }

    void Update() 
    {
        Move();
    }

    void Move()
    {
        movementVector = moveAction.ReadValue<Vector2>();
        playerRigidbody.velocity = movementVector;
    }

    void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("You jumped!");
    }

}
