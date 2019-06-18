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

    private bool isGrounded
    {
        get
        {
            if (Physics2D.BoxCast(transform.position + Vector3.down * .1f, new Vector2(1,1), 0, Vector2.down, spr.rect.height / 2, mask) && velocity.y == 0)
                return true;
            else
                return false;
        }
    }

    //Checks if player is next to wall and running against wall.
    //Returns 1 if clinging to right, -1 if left, 0 if not at all
    private int wallCling()
    {
        
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 1f, mask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 1f, mask);
        if (hitRight.collider != null && Input.GetAxisRaw("Horizontal") > 0.75f)
            return -1;
        else if (hitLeft.collider != null && Input.GetAxisRaw("Horizontal") < -0.75f)
            return 1;
        else return 0;
    }
    #endregion

    #region Public Variables
    public float jumpForce = 1f;
    public float maxSpeed = 10f;
    public Vector2 wallJumpForce = new Vector2(-1, 0.5f);
    #endregion
    #region Private Variables
    private Rigidbody2D rb = new Rigidbody2D();
    LayerMask mask;
    #endregion

    #region Initialization
    private void Start()
    {
        init();
        rb = GetComponent<Rigidbody2D>();
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
                wallJump = wallJumpForce;
                jump = jumpForce;
            }
            else if(isGrounded)
            {
                jump = jumpForce;
            }

        }
        //Horizontal
        float horizontalMove = isGrounded ? speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime : (speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime)/3;
        //Move
        rb.AddForce(new Vector2(horizontalMove + (wallJump.x * wallCling()), jump * wallJump.y));
        //Clamp speed to maxSpeed
        velocity = new Vector2(Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed), velocity.y);
        #endregion

    }


}