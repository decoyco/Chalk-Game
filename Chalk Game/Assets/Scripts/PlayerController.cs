﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLAYER CONTROLLER
//Handles controls for player, requires CharacterController 2D and Player
//Gathers data from Player, outputs to CharacterController and Animator to act according to input
public class PlayerController : MonoBehaviour
{

    //Action conditions
    private bool moveable;
    private bool jumping;
    private bool running;
    private bool clinging;
    private bool attacking;
    private bool hurt;
    private float attackCooldown;

    private Player player;
    private CharacterController2D controller;
    private Animator anim;

    public bool canMove
    {
        get { return moveable; }
        set { moveable = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController2D>();
        controller.rb = GetComponent<Rigidbody2D>();
        controller.speed = player.speed;
        moveable = true;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMove = 0f;
        if (moveable)
        {
            #region Movement
            //Vertical
            Vector2 wallJump = Vector2.one;
            float jump = 0;
            if (Input.GetButtonDown("Jump"))
            {

                if (player.wallCling != 0 && !player.isGrounded)
                {
                    controller.velocity = new Vector2(0, 0);
                    wallJump = player.wallJumpForce;
                    jump = player.jumpForce;
                    jumping = true;
                }
                else if (player.isGrounded)
                {
                    controller.velocity = new Vector2(controller.velocity.x, 0);
                    jump = player.jumpForce;
                    jumping = true;
                }
            }
            //Horizontal
            horizontalMove = player.isGrounded ? player.speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime : (player.speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime) / 3;
            //Move
            controller.move(new Vector2(horizontalMove + (wallJump.x * player.wallCling), jump * wallJump.y), player.maxSpeed);
            #endregion

            #region Attack
            if (Input.GetButtonDown("Fire1") && !attacking)
            {
                attacking = true;
            }
            #endregion
        }
        #region Animation
        anim.SetBool("Attack", attacking);
        if (horizontalMove != 0)
            player.facingRight = horizontalMove > 0 ? true : false;
        anim.SetFloat("Speed", Mathf.Abs(horizontalMove));
        anim.SetBool("Hurt", hurt);
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Take Damage
        if (collision.gameObject.GetComponent<HitBox>())
        {
            player.HP -= collision.gameObject.GetComponent<HitBox>().damage;
            //knockback
            Vector2 knockBack = transform.position - collision.transform.position;
            knockBack.Normalize();
            knockBack *= player.knockBackForce;
            controller.move(knockBack, 100f);
            hurt = true;
            canMove = false;
        }
    }

    public void attackEnd()
    {
        attacking = false;
    }
    
    public void hurtEnd()
    {
        moveable = true;
        hurt = false;
    }
}
