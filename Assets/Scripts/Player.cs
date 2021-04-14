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

    [SerializeField] private ParticleSystem dustEffect = null;
    [SerializeField] private ParticleSystem dustJumpEffect = null;
    [SerializeField] private Transform feet = null;

    private float jumpTimeCounter;
    private BoxCollider2D coll;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool spawnDust = false;

    private int numCurrentJumps;

    float movementInput;

    Animator anim;

    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        footstep = GetComponent<AudioSource>();
    }

    private void OnEnable() => playerInput.Enable();

    private void OnDisable() => playerInput.Disable();

    private void Start()
    {
        numCurrentJumps = numExtraJumpTotal;
        playerInput.PlayerMain.Jump.started += _ => HoldJumpButton();
        playerInput.PlayerMain.Jump.canceled += _ => ReleaseJumpButton();
    }

    private void Update()
    {
        movementInput = playerInput.PlayerMain.Move.ReadValue<float>();

        Flip(movementInput);
        Jump();

        anim.SetFloat("Air", rb.velocity.y);
    }

    private void FixedUpdate()
    {
        // Method to mess with the Physics engine, avoid doing Physics in Update method.
        Move();
        GroundCheck();
    }

    private void GroundCheck()
    {
        if (IsGrounded())
        {
            numCurrentJumps = numExtraJumpTotal;
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
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpVelocity;
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }
        }
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

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);
        return raycastHit.collider != null;
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

    private void Flip(float direction)
    {
        if (direction < 0f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            //transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        } else if (direction > 0f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            //transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Footstep()
    {
        footstep.Play();
    }
}