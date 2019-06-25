using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player defining class, inherits Entity
public class Player : Entity
{
    #region Properties
    public Vector2 velocity
    {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }

    public BoxCollider2D groundBox;
    private bool isGrounded
    {
        get
        {
            //if (Physics2D.BoxCast(transform.position, new Vector2(spr.rect.width - .5f ,1), 0, Vector2.down, spr.rect.height / 2, mask) && velocity.y == 0)
            if (groundBox.IsTouchingLayers(mask))
                return true;
            else
                return false;
        }
    }

    //Checks if player is next to wall and running against wall.
    //Returns 1 if clinging to right, -1 if left, 0 if not at all
    public BoxCollider2D wallBox;
    private int wallCling()
    {

        if (wallBox.IsTouchingLayers(mask) && Input.GetAxisRaw("Horizontal") > 0.75f)
        {
            clinging = true;
            return -1;
        }
        else if (wallBox.IsTouchingLayers(mask) && Input.GetAxisRaw("Horizontal") < -0.75f)
        {
            clinging = true;
            return 1;
        }
        else
        {
            clinging = false;
            return 0;
        }
    }
    #endregion

    #region Public Variables
    public float jumpForce = 1f;
    public float maxSpeed = 10f;
    public float attackLength = 1f;
    public Vector2 wallJumpForce = new Vector2(-1, 0.5f);
    public Animator anim;
    #endregion
    #region Private Variables
    //Animation conditions
    private bool jumping;
    private bool running;
    private bool clinging;
    private bool attacking;

    private LayerMask mask;
    private float attackCooldown;
    #endregion

    #region Initialization
    private void Start()
    {
        init();
        mask = LayerMask.GetMask("Ground");
    }
    #endregion

    private void Update()
    {
        #region Movement
        //Vertical
        Vector2 wallJump = Vector2.one;
        float jump = 0;
        if(Input.GetButtonDown("Jump"))
        {
            
            if (wallCling() != 0 && !isGrounded)
            {
                velocity = new Vector2(0, 0);
                wallJump = wallJumpForce;
                jump = jumpForce;
                jumping = true;
            }
            else if(isGrounded)
            {
                velocity = new Vector2(velocity.x, 0);
                jump = jumpForce;
                jumping = true;
            }
        }
        //Horizontal
        float horizontalMove = isGrounded ? speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime : (speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime)/3;
        //Move
        rb.AddForce(new Vector2(horizontalMove + (wallJump.x * wallCling()), jump * wallJump.y));
        //Clamp speed to maxSpeed
        velocity = new Vector2(Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed), velocity.y);
        #endregion
        #region Attack
        if (Input.GetButtonDown("Fire1") && !attacking && attackCooldown <= Time.time)
        {
            attackCooldown = Time.time + attackLength;
            attacking = true;
        }
        else if(attackCooldown <= Time.time)
        {
            attacking = false;
        }

        #endregion
        #region Animation
        anim.SetBool("attacking", attacking);
        if (horizontalMove != 0)
            facingRight = horizontalMove > 0 ? true : false;
        anim.SetFloat("running", Mathf.Abs(horizontalMove));

        #endregion
    }
}