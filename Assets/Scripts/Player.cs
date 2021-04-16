using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpVelocity = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int numExtraJumpTotal = 0;
    [SerializeField] private float jumpTime = 0.35f;
    [SerializeField] private Animator anim = null;
    [SerializeField] private float GRAVITY_SCALE = 1.5f;
    [SerializeField] private float maxTerminalVelocity = 4f;
    [SerializeField] private BoxCollider2D boxCollider = null;

    [Header("Sounds")]
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioClip[] footstepClip;
    [SerializeField] private AudioSource jumpSound;

    [Header("Particles")] // Particles effect
    [SerializeField] private ParticleSystem dustEffect = null;
    [SerializeField] private ParticleSystem dustJumpEffect = null;
    [SerializeField] private Transform feet = null;

    [Header("Wall Jump")] // Wall Sliding and wall jumping
    [SerializeField] private Transform frontCheck = null;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private float xWallForce;
    [SerializeField] private float yWallForce;
    [SerializeField] private float wallJumpTime; // Time the x and y wall force is applied
    [SerializeField] private float timeDisabledAfterWallJump = 0.1f;

    [Header("Dash")] // Dashing
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float timeDisabledAfterDash = 0.1f;
    [SerializeField] private float dashCooldown = 0.5f;

    [Header("Abilities Unlocked")]
    [SerializeField] private bool dashUnlocked = false;
    [SerializeField] private bool doubleJumpUnlocked = false;
    [SerializeField] private bool wallJumpUnlocked = false;

    private float jumpTimeCounter;

    public PlayerInput playerInput;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private readonly float defaultCameraSize = 4f;
    private readonly float zoomOutSize = 6f;
    // private SoundFX soundFX;

    // Booleans
    private bool isJumping = false;
    private bool spawnDust = false;
    private bool isTouchingFront = false;
    private bool wallSliding = false;
    private bool isGrounded = false;
    private bool wallJumping = false;
    private bool disabledMovement = false;
    private bool dashing = false;
    private bool canDash = true;
    private bool freeCamPressed = false;

    private int numCurrentJumps;

    private float movementInput;
    private float facingDirection;

    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() => playerInput.Enable();

    private void OnDisable() => playerInput.Disable();

    private void Start()
    {
        ResetJumpCount();
        playerInput.PlayerMain.Jump.started += _ => HoldJumpButton();
        playerInput.PlayerMain.Jump.canceled += _ => ReleaseJumpButton();
        playerInput.PlayerMain.Dash.performed += _ => Dash();
        playerInput.PlayerMain.FreeCam.started += _ => OnPressFreeCam();
        playerInput.PlayerMain.FreeCam.canceled += _ => OnReleaseFreeCam();

        mainCamera = GetComponent<Camera>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!disabledMovement)
        {
            Jump();
            Flip(movementInput);
            Move();
        }

        movementInput = playerInput.PlayerMain.Move.ReadValue<float>();

        facingDirection = transform.right.x;

        anim.SetFloat("Air", rb.velocity.y);
    }

    private void FixedUpdate()
    {
        // Method to mess with the Physics engine, avoid doing Physics in Update method.

        GroundCheck();
        FreeCam();

        if (rb.velocity.y < -maxTerminalVelocity)
        {
            ClampVertVelocity();
        }

        if (wallJumpUnlocked)
        {
            WallCheck();
        }
    }

    private void FreeCam()
    {
        var brain = (mainCamera == null) ? null : mainCamera.GetComponent<CinemachineBrain>();
        var virtualCam = (brain == null) ? null : brain.ActiveVirtualCamera as CinemachineVirtualCamera;

        virtualCam.m_Lens.OrthographicSize = (freeCamPressed && virtualCam != null) ? zoomOutSize : defaultCameraSize;
    }

    private void OnPressFreeCam()
    {
        freeCamPressed = true;
        //StartCoroutine(StopFreeCam());
    }

    /*private IEnumerator StopFreeCam()
    {
        if (freeCamPressed)
        {
            yield return new WaitForSeconds(freeCamTimer);
            
        } else
        {
            yield return new WaitForSeconds(0.001f);
        }
        
        freeCamPressed = false;
    }*/

    private void OnReleaseFreeCam()
    {
        freeCamPressed = false;
    }

    private void GroundCheck()
    {
        isGrounded = /*(Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, checkRadius, groundLayer).collider != null); */Physics2D.OverlapCircle(feet.position, checkRadius, groundLayer);

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

        if ((wallSliding && !dashing) || (!wallSliding && dashing))
        {
            wallSliding = (isTouchingFront && !isGrounded);
        } else if (!wallSliding && !dashing)
        {
            wallSliding = (isTouchingFront && !isGrounded && movementInput != 0); 
        }

        if (wallSliding)
        {
            anim.SetBool("WallCling", true);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            ResetJumpCountOnWall();
            isJumping = false;
            Jump();
        } else
        {
            anim.SetBool("WallCling", false);
        }
        
    }

    private void ClampVertVelocity()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxTerminalVelocity, float.MaxValue));
    }

    private void ResetJumpCount()
    {
        numCurrentJumps = numExtraJumpTotal;
        dashing = false;
    }

    private void ResetJumpCountOnWall()
    {
        numCurrentJumps = numExtraJumpTotal + 1;
        dashing = false;
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
        if (isJumping) // If the player is holding jump button
        {
            if (HeadCheck())
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, float.MinValue, 0));
                isJumping = false;
            }

            if (wallJumpUnlocked) {
                WallJumpCheck();
                if (wallJumping)
                {
                    rb.velocity = new Vector2(xWallForce * -facingDirection, yWallForce);
                    Flip(rb.velocity.x);
                    OnDisableMovement(timeDisabledAfterWallJump);
                    return;
                } 
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

    private void OnDisableMovement(float time)
    {
        disabledMovement = true;
        StartCoroutine(StartDisableCountDown(time));
    }

    private IEnumerator StartDisableCountDown(float timeDisabled)
    {
        yield return new WaitForSeconds(timeDisabled);
        disabledMovement = false;
        isJumping = false;
        anim.SetBool("Dashing", false);
        rb.gravityScale = GRAVITY_SCALE;
    }

    private void WallJumpCheck()
    {
        if (wallSliding)
        {
            wallJumping = true;
            Invoke(nameof(DisableWallJump), wallJumpTime);
        }
    }

    private bool HeadCheck()
    {
        //return Physics2D.OverlapCircle(headCheck.position, checkRadius, groundLayer);
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size / 1.2f, 0f, Vector2.up, checkRadius, groundLayer).collider != null;
    }

    private void DisableWallJump()
    {
        wallJumping = false;
    }

    private void HoldJumpButton()
    {
        if (doubleJumpUnlocked)
        {
            if (numCurrentJumps > 0)
            {
                isJumping = true;
                jumpTimeCounter = jumpTime; // Add jump time counter for holding
                numCurrentJumps--;
                SpawnJumpDust();
                PlayJumpSound();
            }
        } else
        {
            if (isGrounded || wallSliding)
            {
                isJumping = true;
                jumpTimeCounter = jumpTime; // Add jump time counter for holding
                SpawnJumpDust();
                PlayJumpSound();
            }
        }

        
    }

    private void ReleaseJumpButton()
    {
        isJumping = false;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, float.MinValue, 2));
    }

    private void Dash()
    {
        if (disabledMovement || !dashUnlocked) { return; }

        if (!dashing && canDash)
        {
            if (wallSliding)
            {
                rb.velocity = Vector2.left * dashForce * facingDirection;
            }
            else
            {
                rb.velocity = Vector2.right * dashForce * facingDirection;
            }

            Flip(rb.velocity.x);
            rb.gravityScale = 0;
            dashing = true;
            canDash = false;
            anim.SetBool("Dashing", dashing);
            OnDisableMovement(timeDisabledAfterDash);
            StartCoroutine(WaitForDashCooldown(dashCooldown));
        }
    }

    private IEnumerator WaitForDashCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canDash = true;
    }

    private void Move()
    {
        rb.velocity = new Vector2(movementInput * speed, rb.velocity.y);

        if(Mathf.Abs(movementInput) < Mathf.Epsilon) 
        {
            anim.SetBool("Running", false);
        } else
        {
            anim.SetBool("Running", true);
        }
    }

    private void UnlockWallJump()
    {
        wallJumpUnlocked = true;
    }

    private void UnlockDash()
    {
        dashUnlocked = true;
    }

    private void UnlockDoubleJump()
    {
        doubleJumpUnlocked = true;
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
        footstep.clip = footstepClip[UnityEngine.Random.Range(0, footstepClip.Length)];
        footstep.Play();
    }

    private void PlayJumpSound()
    {   
        jumpSound.time = 0.08f;
        jumpSound.Play();
    }
}