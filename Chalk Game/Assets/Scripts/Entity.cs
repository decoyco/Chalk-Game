using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class defines all entities in the game involved in combat
public class Entity : MonoBehaviour
{
    public int maxHP = 1;
    public int HP;
    public float speed = 1f;
    protected Sprite spr;

    //Initialization function, run in Start()
    protected void init()
    {
        spr = GetComponent<SpriteRenderer>().sprite;
        HP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Take Damage
        if(collision.gameObject.GetComponent<HitBox>())
        {
            HP -= collision.gameObject.GetComponent<HitBox>().damage;
        }
    }
}
