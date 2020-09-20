using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public float speed;
    private float startingSpeed;
    private float crouchSpeed;
    public float jumpForce;
    public float maxVelocity;
    private float startingMaxVelocity;
    public float groundCheckRadius;
    public bool canJump = true;
    public bool isCrouching = false;
    public bool canDash = true;
    public string direction;

    [SerializeField] private LayerMask platformsLayerMask;
    private Rigidbody2D rb;
    public Transform groundCheck;
    public Transform startingPosition;
    public Transform checkPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingSpeed = speed;
        maxVelocity = startingMaxVelocity;
        crouchSpeed = startingSpeed / 1.4f;
    }

    private void Update()
    {
        Movement();
        Jumping();
        Crouch();
        Dash();
    }

    void Movement()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        rb.velocity += Vector2.right * move;

        if (move.x == 0 && rb.velocity != Vector2.zero)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        // Velocity can't exceed max Velocity
        if (rb.velocity.sqrMagnitude > maxVelocity)
        {
            rb.velocity *= 0.99f;
        }

        // Check player input to determine current direction for Dash
        if (move.x < 0)
        {
            direction = "Left";
        }
        else if (move.x > 0)
        {
            direction = "Right";
        }
    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            if (canJump)
            {
                canJump = false;
                rb.velocity += Vector2.up * jumpForce;
            }
        }

        canJump = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformsLayerMask);
    }

    void Crouch()
    {
        isCrouching = Input.GetKey(KeyCode.S) && canJump != false || Input.GetAxisRaw("Vertical") < 0 && canJump != false;

        if (isCrouching)
        {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            speed = crouchSpeed;
        }
        else
        {
            speed = startingSpeed;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1")) // Xbox "B"
        {
            if (canDash /*&& canJump*/)
            {
                if (direction == "Left")
                {
                    rb.AddForce(new Vector2(5.5f * -1, 0f), ForceMode2D.Impulse);
                    canDash = false;
                    Invoke("ResetDash", 0.6f);
                }
                else
                if (direction == "Right")
                {
                    rb.AddForce(new Vector2(5.5f * 1, 0f), ForceMode2D.Impulse);
                    canDash = false;
                    Invoke("ResetDash", 0.6f);
                }
            }
        }
    }

    // Reset Dash ability Cooldown
    void ResetDash(){
        speed = startingSpeed;
        maxVelocity = startingMaxVelocity;
        canDash = true;
    }

    // Collision check with Out of Bounds 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "ResetBounds"){
            transform.position = startingPosition.transform.position;
            rb.velocity = Vector2.zero;
        }
        else if (collision.gameObject.name == "ResetBoundsCheckpoint")
        {
            transform.position = checkPoint.transform.position;
            rb.velocity = Vector2.zero;
        }
    }
}
