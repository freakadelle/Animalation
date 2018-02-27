using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovements : MonoBehaviour
{

    public bool debugging = false;

    public float speed = 3;
    public float jumpForce = 3;
    public bool isFacingRight;
    public bool isGrounded;
    public float maxAmplifyJumpTime;
    public float jumpAfterBurner;
    public int gravitationAfterBurner;
    public int maxAirJumps;
    public float bounceFactor;

    public Vector2 maxVelocity;

    private float jumpTime;
    private SpriteRenderer renderer;
    private Rigidbody2D rig;
    private Collider2D collider;
    private Vector2 moveVel;
    private float distToGround;
    //private float gravityScale;

    private bool isJumping;
    private int jumpingCount;
    private bool jumpingDown;
    private bool isJumpInterrupted;

    private Animator anim;

    // Use this for initialization
    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        distToGround = collider.bounds.extents.y;
    }

    private void Update()
    {
        
        if (Input.GetButtonDown("Jump"))
        {
            jumpingCount++;
            jumpingDown = true;
            anim.SetTrigger("jumping");
        } else if (Input.GetButtonUp("Jump"))
        {
            rig.gravityScale = gravitationAfterBurner;
            isJumping = false;
            jumpingDown = false;
        }
    }

    private void FixedUpdate()
    {

        float xMoveVel = Input.GetAxisRaw("Horizontal");

        if (xMoveVel > 0)
            isFacingRight = false;
            
        else if(xMoveVel < 0)
            isFacingRight = true;

        if (xMoveVel != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        isGrounded = IsGrounded();

        if (isGrounded)
        {
            jumpingCount = 0;
            anim.SetBool("grounded",true);
        }
        else
        {
            anim.SetBool("grounded", false);
        }

        //Process Jump
        if (jumpingDown && !isJumping && jumpingCount < maxAirJumps)
        {
            Debug.Log("IMPULSE JUMPING");
            rig.gravityScale = 1;
            rig.velocity = Vector2.zero;
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpTime = Time.time + maxAmplifyJumpTime;
            isJumping = true;
            isJumpInterrupted = false;
        } 

        if (!isJumpInterrupted && isJumping && Time.time <= jumpTime)
        {
            rig.AddForce(Vector2.up * jumpForce * jumpAfterBurner);
        }
        else
        {
            rig.gravityScale = gravitationAfterBurner;
        }


        //Process VElocity
        float xVel = xMoveVel * speed;
        xVel = Mathf.Min(maxVelocity.x, xVel);
        float yVel = Mathf.Min(maxVelocity.y, rig.velocity.y);

        moveVel = new Vector2(xVel, yVel);
        rig.velocity = moveVel;

        ProcessFlip(isFacingRight);
    }

    private void ProcessFlip(bool _isRight)
    {
        renderer.flipX = _isRight;
    }

    private bool IsGrounded()
    {
        Ray2D ray = new Ray2D(transform.position, -Vector2.up);
        if (debugging)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
        }

        return Physics2D.Raycast(ray.origin, ray.direction, distToGround + 0.2f, GameStatics.Instance.groundLayerMask);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !isGrounded)
        {
            isJumpInterrupted = true;
            rig.velocity = new Vector2(rig.velocity.x, -bounceFactor);
            anim.SetBool("grounded", true);
        }
    }
}
