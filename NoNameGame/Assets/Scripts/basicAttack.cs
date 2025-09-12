using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
// https://www.youtube.com/watch?v=LNLVOjbrQj4&t=998s
public class BasicAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 mousePosition;

    public GameObject projectile;

    public float projectileSpeed = 5,
    projectileMaxDistance =5;
    private float projectileDistance;
    private Rigidbody2D rb2d;
    private Transform t, projectileT;

    static private int atkStage = 0;
    void Start()
    {
        t = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>();
        projectileT = GetComponent<Transform>();
        projectileDistance = projectileMaxDistance;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookDir;
        if (atkStage == 0)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // from https://www.reddit.com/r/Unity2D/comments/swum6c/how_to_make_object_follow_mouse_in_unity_with_new/
            lookDir = mousePosition - (Vector2)t.position;
            rb2d.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        }
        PreformAttack();


    }

    /// <summary>
    /// Attempt to start an attack
    /// </summary>
    public void Attack()
    {
        if (atkStage != 0) return;

        atkStage++;
        //rb2d.AddForce(transform.right * projectileSpeed);
    }

    private void PreformAttack()
    {


        switch (atkStage)
            {
                case 0:
                    return;
                case 1:
                    projectileT.position += transform.right*projectileSpeed*Time.deltaTime;
                    if (projectileT.position.x >= projectileDistance)
                    {
                        atkStage++;
                    }
                    break;
                case 2:
                    projectileT.position -= new Vector3(projectileSpeed * Time.deltaTime,0f,0f);
                    if (projectileT.position.x <= 0.5)
                    {
                        projectileT.position = new Vector3(0.5f, 0f, 0f);
                        atkStage = 0;
                    }
                    break;
            }
        }

    }
