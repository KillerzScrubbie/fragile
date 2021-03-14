using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private int numExtraJumpTotal = 1;

    private Collider2D coll;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Vector3 currentPosition;

    int numCurrentJumps;

    float movementInput;

    Animator anim;

    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
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
        bool isTouchingGround = coll.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (isTouchingGround)
        {
            numCurrentJumps = numExtraJumpTotal;
            
        }
        if (numCurrentJumps > 0)
        {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            numCurrentJumps--;
        }
    }

    /*private bool isGrounded()
    {
        Vector2 topLeftPoint = transform.position;
        topLeftPoint.x -= coll.bounds.extents.x;
        topLeftPoint.y += coll.bounds.extents.y;
        Vector2 bottomRightPoint = transform.position;
        bottomRightPoint.x += coll.bounds.extents.x;
        bottomRightPoint.y -= coll.bounds.extents.y;
        return Physics2D.OverlapArea(topLeftPoint, bottomRightPoint, ground);
    }*/

    private void Update()
    {
        movementInput = playerInput.PlayerMain.Move.ReadValue<float>();
        Flip(movementInput);
        Move();
    }

    private void Move()
    {
        currentPosition = transform.position;
        currentPosition.x += movementInput * speed * Time.deltaTime;
        transform.position = currentPosition;

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
}