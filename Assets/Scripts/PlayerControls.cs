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

    [SerializeField] float deathKnockback = -8f;
    [SerializeField] float deathHeight = 10f;

    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowSpawnPoint;

    float defaultGravity;

    Rigidbody2D playerRigidbody;
    BoxCollider2D playerFeetCollider;
    CapsuleCollider2D playerCollider;
    Animator playerAnimator;
    Vector2 movementVector;
    bool playerHasHorizontalVelocity;
    bool isGrounded;
    bool isClimbing = false;
    bool isAlive = true;
    bool isFiring = false;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction fireAction;

    void Awake() 
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerAnimator = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        fireAction = playerInput.actions["Fire"];

        defaultGravity = playerRigidbody.gravityScale;
    }

    void OnEnable() 
    {
        fireAction.performed += Fire;
    }

    void OnDisable() 
    {
        fireAction.performed -= Fire;
    }

    void Update() 
    {
        movementVector = moveAction.ReadValue<Vector2>();

        if (!isAlive) { return; }

        Move();
        FlipPlayerSprite();
        Jump();
        JumpAnimation();
        ClimbLadder();
        Die();
    }

    void Move()
    {
        if (!isAlive) { return; }

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
        if (!isAlive) { return; }

        if (!isGrounded || playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) { return; }
        
        if (jumpAction.triggered)
        {
            playerRigidbody.velocity += new Vector2(playerRigidbody.velocity.x, jumpStrength);
            isGrounded = false;
        }
    }

    void JumpAnimation()
    {
        playerAnimator.SetBool("isJumping", !isGrounded);
    }

    void ClimbLadder()
    {
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            playerRigidbody.gravityScale = defaultGravity;
            playerAnimator.SetBool("isClimbing", false);
            isClimbing = false;

            return; 
        }

        playerRigidbody.gravityScale = 0f;
        Vector2 verticalMovement = new Vector2(playerRigidbody.velocity.x, movementVector.y * climbSpeed);
        playerRigidbody.velocity = verticalMovement;

        isClimbing = Mathf.Abs(verticalMovement.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", isClimbing);

    }

    void Fire(InputAction.CallbackContext context)
    {
        if (!isAlive) { return; }

        if (isFiring) { return; }

        isFiring = true;

        playerAnimator.SetTrigger("Fire");

        isFiring = false;
    }

    void CreateArrow()
    {
        Instantiate(arrow, arrowSpawnPoint.position, transform.rotation);
    }

    void Die()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard", "Water")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Death");

            playerCollider.enabled = false;
            playerFeetCollider.enabled = false;

            playerRigidbody.velocity = new Vector2(0,0);

            playerRigidbody.velocity += new Vector2(deathKnockback, deathHeight);
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
