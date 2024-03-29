﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player defining class, contains player traits, inherits Entity
public class Player : Entity
{
    public float jumpForce = 1f;
    public float maxSpeed = 10f;
    public float attackLength = 1f;
    public Vector2 wallJumpForce = new Vector2(-1, 0.5f);
    private LayerMask mask;
    private float attackCooldown;

    #region Properties
        public BoxCollider2D groundBox;
        public bool isGrounded
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
        public int wallCling
        {
            get
            {
                if (wallBox.IsTouchingLayers(mask) && Input.GetAxisRaw("Horizontal") > 0.75f)
                {
                    return -1;
                }
                else if (wallBox.IsTouchingLayers(mask) && Input.GetAxisRaw("Horizontal") < -0.75f)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    #endregion


    #region Initialization
        private void Start()
        {
            init();
            mask = LayerMask.GetMask("Ground");
        }
    #endregion
}