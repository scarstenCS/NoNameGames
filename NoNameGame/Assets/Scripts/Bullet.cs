using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float  launchForce;
    public int damage;

    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d.GetComponent<Rigidbody2D>();
        rb2d.AddForce(transform.right * launchForce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
