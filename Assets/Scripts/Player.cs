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

    private BoxCollider2D coll;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Vector3 currentPosition;

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
        playerInput.PlayerMain.Jump.performed += _ => Jump();
    }

    private void Jump()
    {
        if (numCurrentJumps > 0)
        {
            // rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            rb.velocity = Vector2.up * jumpVelocity;
            numCurrentJumps--;
        }
    }

    private void Update()
    {
        movementInput = playerInput.PlayerMain.Move.ReadValue<float>();

        Flip(movementInput);
        Move();
    }

    private void GroundCheck()
    {
        // bool isTouchingGround = coll.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (IsGrounded())
        {
            numCurrentJumps = numExtraJumpTotal;
            anim.SetBool("Jumping", false);
        }
        else
        {
            anim.SetBool("Jumping", true);
        }
    }

    private void FixedUpdate()
    {
        // Method to mess with the Physics engine, avoid doing Physics in Update method.

        GroundCheck();
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);
        return raycastHit.collider != null;
    }

    private void Move()
    {
        /*currentPosition = transform.position;
        currentPosition.x += movementInput * speed * Time.deltaTime;
        transform.position = currentPosition;*/
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
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        } else if (direction > 0f)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Footstep()
    {
        footstep.Play();
    }
}