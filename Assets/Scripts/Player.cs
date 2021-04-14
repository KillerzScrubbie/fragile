using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpVelocity = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int numExtraJumpTotal = 1;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private float jumpTime = 0.35f;

    // Particles effect
    [SerializeField] private ParticleSystem dustEffect = null;
    [SerializeField] private ParticleSystem dustJumpEffect = null;
    [SerializeField] private Transform feet = null;

    // Wall Sliding and wall jumping
    [SerializeField] private Transform frontCheck = null;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private float xWallForce;
    [SerializeField] private float yWallForce;
    [SerializeField] private float wallJumpTime; // Time the x and y wall force is applied
    [SerializeField] private float timeDisabledAfterWallJump = 0.1f;

    private float jumpTimeCounter;
    
    // private BoxCollider2D coll;
    private PlayerInput playerInput;
    private Rigidbody2D rb;

    // Booleans
    private bool isJumping = false;
    private bool spawnDust = false;
    private bool isTouchingFront = false;
    private bool wallSliding = false;
    private bool isGrounded = false;
    private bool wallJumping = false;
    private bool disabledMovement = false;

    private int numCurrentJumps;

    private float movementInput;

    private Animator anim;

    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        // coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        footstep = GetComponent<AudioSource>();
    }

    private void OnEnable() => playerInput.Enable();

    private void OnDisable() => playerInput.Disable();

    private void Start()
    {
        ResetJumpCount();
        playerInput.PlayerMain.Jump.started += _ => HoldJumpButton();
        playerInput.PlayerMain.Jump.canceled += _ => ReleaseJumpButton();
    }

    private void Update()
    {
        movementInput = playerInput.PlayerMain.Move.ReadValue<float>();

        if (!disabledMovement)
        {
            Flip(movementInput);
        }

        Jump();

        anim.SetFloat("Air", rb.velocity.y);
    }

    private void FixedUpdate()
    {
        // Method to mess with the Physics engine, avoid doing Physics in Update method.
        Move();
        GroundCheck();
        WallCheck();
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(feet.position, checkRadius, groundLayer);

        if (isGrounded)
        {
            ResetJumpCount();
            anim.SetBool("InAir", false);
            if (spawnDust)
            {
                SpawnJumpDust();
                spawnDust = false;
                Footstep();
            }
        }
        else
        {
            anim.SetBool("InAir", true);
            spawnDust = true;
        }
    }

    private void WallCheck()
    {
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, groundLayer);

        wallSliding = (isTouchingFront && !isGrounded && movementInput != 0);

        if (wallSliding)
        {
            anim.SetBool("WallCling", true);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            ResetJumpCountOnWall();

            Jump();
        } else
        {
            anim.SetBool("WallCling", false);
        }
        
    }

    private void ResetJumpCount()
    {
        numCurrentJumps = numExtraJumpTotal;
    }

    private void ResetJumpCountOnWall()
    {
        numCurrentJumps = numExtraJumpTotal + 1;
    }

    public void SpawnDust()
    {
        Instantiate(dustEffect, feet.position, Quaternion.identity);
    }

    private void SpawnJumpDust()
    {
        Instantiate(dustJumpEffect, feet.position, Quaternion.identity);
    }

    private void Jump()
    {
        if (isJumping && !disabledMovement) // If the player is holding jump button
        {
            WallJumpCheck();
            if (wallJumping)
            {
                rb.velocity = new Vector2(xWallForce * -movementInput, yWallForce);
                Flip(rb.velocity.x);
                OnDisableMovement();
                return;
            }
            
            if (jumpTimeCounter > 0 && !wallSliding)
            {
                rb.velocity = Vector2.up * jumpVelocity;
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }
        }
    }

    private void OnDisableMovement()
    {
        disabledMovement = true;
        StartCoroutine(StartDisableCountDown());
    }

    private IEnumerator StartDisableCountDown()
    {
        yield return new WaitForSeconds(timeDisabledAfterWallJump);
        disabledMovement = false;
        isJumping = false;
    }

    private void WallJumpCheck()
    {
        if (wallSliding)
        {
            wallJumping = true;
            Invoke(nameof(DisableWallJump), wallJumpTime);
        }
    }

    private void DisableWallJump()
    {
        wallJumping = false;
    }

    private void HoldJumpButton()
    {
        if (numCurrentJumps > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime; // Add jump time counter for holding
            numCurrentJumps--;
            SpawnJumpDust();
        }
    }

    private void ReleaseJumpButton()
    {
        isJumping = false;
    }

    private void Move()
    {
        if (disabledMovement) { return; }

        rb.velocity = new Vector2(movementInput * speed, rb.velocity.y);

        if(Mathf.Abs(movementInput) < Mathf.Epsilon) 
        {
            anim.SetBool("Running", false);
        } else
        {
            anim.SetBool("Running", true);
        }
    }

    private void Flip(float direction)
    {
        if (direction < 0f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        } else if (direction > 0f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void Footstep()
    {
        footstep.Play();
    }
}