using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 1;
    public float speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Destroy(this);
        }
        transform.position += Vector3.Normalize(new Vector3(-transform.position.x, -transform.position.y)) * Time.deltaTime*speed; 
    }
}