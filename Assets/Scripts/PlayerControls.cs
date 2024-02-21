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
    public bool isGrounded;
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
        if (!isGrounded || isClimbing) { return; }
        
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
            playerRigidbody.gravityScale = defaultGravity;
            playerAnimator.SetBool("isClimbing", false);

            return; 
        }

        playerRigidbody.gravityScale = 0f;
        Vector2 verticalMovement = new Vector2(playerRigidbody.velocity.x, movementVector.y * climbSpeed);
        playerRigidbody.velocity = verticalMovement;

        isClimbing = Mathf.Abs(verticalMovement.y) > Mathf.Epsilon && playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        playerAnimator.SetBool("isClimbing", isClimbing);

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
