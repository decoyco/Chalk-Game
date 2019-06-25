using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class defines all entities in the game involved in combat
public class Entity : MonoBehaviour
{
    public int maxHP = 1;
    public int HP;
    public float speed = 1f;
    private bool face;
    //Initialization function, run in Start()
    protected void init()
    {
        HP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Take Damage
        if (collision.gameObject.GetComponent<HitBox>())
        {
            HP -= collision.gameObject.GetComponent<HitBox>().damage;
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
