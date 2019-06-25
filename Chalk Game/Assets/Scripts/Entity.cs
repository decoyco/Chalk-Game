using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class defines all entities in the game involved in combat
public class Entity : MonoBehaviour
{
    public float knockBackForce = 3f;
    public int maxHP = 1;
    public int HP;
    public float speed = 1f;
    private bool face;

    protected Rigidbody2D rb;
    //Initialization function, run in Start()
    private void Start()
    {
        init();
    }
    protected void init()
    {
        rb = GetComponent<Rigidbody2D>();
        HP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Take Damage
        if (collision.gameObject.GetComponent<HitBox>())
        {
            HP -= collision.gameObject.GetComponent<HitBox>().damage;
            //knockback
            Vector2 knockBack = transform.position - collision.transform.position;
            knockBack.Normalize();
            knockBack *= knockBackForce;
            Debug.Log(rb);
            rb.AddForce(knockBack);
        }
    }

    protected bool facingRight
    {
        get { return face; }
        set
        {
            face = value;
            Vector3 theScale = transform.localScale;
            theScale.x = face ? 1 : -1;
            transform.localScale = theScale;
        }
    }
}
