using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public bool active { get { return GetComponent<PolygonCollider2D>().enabled; } set { GetComponent<PolygonCollider2D>().enabled = false; } }
    public int damage;
}
