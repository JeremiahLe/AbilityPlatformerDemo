using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float maxVelocity;
    public float groundCheckRadius;
    public bool canJump = true;

    [SerializeField] private LayerMask platformsLayerMask;
    private Rigidbody2D rb;
    public Transform groundCheck;
    public Transform startingPosition;
    public Transform checkPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
        Jumping();
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

        if (rb.velocity.sqrMagnitude > maxVelocity)
        {
            rb.velocity *= 0.99f;
        }
    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump)
            {
                canJump = false;
                rb.velocity += Vector2.up * jumpForce;
            }
        }

        canJump = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformsLayerMask);
    }

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
