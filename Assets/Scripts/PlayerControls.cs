using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] float movementSpeed = 4.5f;
    [SerializeField] float jumpStrength = 11f;
    [SerializeField] float climbSpeed = 5f;

    float defaultGravity;

    Rigidbody2D playerRigidbody;
    BoxCollider2D playerFeetCollider;
    CapsuleCollider2D playerCollider;
    Animator playerAnimator;
    Vector2 movementVector;
    bool playerHasHorizontalVelocity;
    bool isGrounded;
    public bool isClimbing = false;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;

    void Awake() 
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerAnimator = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        defaultGravity = playerRigidbody.gravityScale;
    }

    void Update() 
    {
        movementVector = moveAction.ReadValue<Vector2>();

        Move();
        FlipPlayerSprite();
        Jump();
        ClimbLadder();
        SetGravity();
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
        if (!isGrounded) { return; }
        
        if (jumpAction.triggered)
        {
            playerRigidbody.velocity += new Vector2(playerRigidbody.velocity.x, jumpStrength);
            isGrounded = false;
        }
    }

    void ClimbLadder()
    {
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            isClimbing = false;

            return; 
        }

        isClimbing = true;
        Vector2 verticalMovement = new Vector2(playerRigidbody.velocity.x, movementVector.y * climbSpeed);
        playerRigidbody.velocity = verticalMovement;
    }

    void SetGravity()
    {
        if (isClimbing)
        {
            playerRigidbody.gravityScale = 0;
        }
        else
        {
            playerRigidbody.gravityScale = defaultGravity;
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }   
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.GetMask("Ground"))
        {
            isGrounded = false;
        }
    }

}
