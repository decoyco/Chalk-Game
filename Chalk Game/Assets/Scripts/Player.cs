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
            if (Physics2D.BoxCast(transform.position + Vector3.down * .1f, new Vector2(1,1), 0, Vector2.down, spr.rect.height / 2) && velocity.y == 0)
                return true;
            else
                return false;
        }
    }

    //Checks if player is next to wall and running against wall.
    //Returns 1 if clinging to right, -1 if left, 0 if not at all
    private bool wallCling()
    {
        Debug.DrawRay(transform.position, Vector2.right, Color.blue);
        Debug.DrawRay(transform.position, Vector2.left, Color.yellow);
        if (Physics2D.Raycast(transform.position, Vector2.right, spr.rect.width / 2 + 0.1f, 8) && Input.GetAxisRaw("Horizontal") > 0.75f)
        {
            Debug.Log(Physics2D.Raycast(transform.position, Vector2.right, spr.rect.width / 2 + 0.1f, 8));
            return true;
        }
        else if (Physics2D.Raycast(transform.position, Vector2.left, spr.rect.width / 2 + 0.1f, 8) && Input.GetAxisRaw("Horizontal") < -0.75f)
            return true;
        else return false;
    }
    #endregion

    #region Public Variables
    public float jumpForce = 1f;
    public float maxSpeed = 10f;
    public Vector2 wallJumpForce = new Vector2(-1, 0.5f);
    #endregion
    #region Private Variables
    private Rigidbody2D rb = new Rigidbody2D();
    #endregion

    #region Initialization
    private void Start()
    {
        init();
        rb = GetComponent<Rigidbody2D>();
    }
    #endregion

    private void Update()
    {
        #region Movement
        //Vertical
        float jump = (Input.GetButtonDown("Jump") && isGrounded) ? jumpForce : 0;
        //Horizontal
        float horizontalMove = isGrounded ? speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime : (speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime)/2;
        //WallJump check
        Vector2 wallJump = wallCling() ? wallJumpForce : Vector2.one;
        //Move
        rb.AddForce(new Vector2(horizontalMove, jump)*wallJump);
        //Clamp speed to maxSpeed
        velocity = new Vector2(Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed), velocity.y);
        #endregion

    }


}