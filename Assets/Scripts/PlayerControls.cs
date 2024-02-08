using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] float movementSpeed = 4.5f;
    [SerializeField] float jumpHeight = 10f;

    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    Vector2 movementVector;
    bool playerHasHorizontalVelocity;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;

    void Awake() 
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Update() 
    {
        movementVector = moveAction.ReadValue<Vector2>();

        Move();
        FlipPlayerSprite();
        Jump();
    }

    void Move()
    {
        Vector2 horizontalMovement = new Vector2(movementVector.x * movementSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = horizontalMovement;

        playerHasHorizontalVelocity = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerHasHorizontalVelocity);
    }

    void FlipPlayerSprite()
    {
        playerHasHorizontalVelocity = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalVelocity)
        {
            gameObject.transform.localScale = new Vector2 (Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }
    }

    void Jump()
    {
        if (jumpAction.triggered)
        {
            playerRigidbody.velocity += new Vector2 (playerRigidbody.velocity.x, jumpHeight);
            playerAnimator.SetBool("isJumping", true);
        }
        else
        {
            playerAnimator.SetBool("isJumping", false);
        }
    }

}
