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

    public GameObject projectile, player;

    public float projectileSpeed = 5,
    projectileMaxDistance = 5;
    private float projectileDistance;
    private Rigidbody2D rb2d, rbProjectile;
    private Transform t, projectileT, playerT;


    private Vector2 origin;

    static public int atkStage = 0;
    void Start()
    {
        t = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>();
        projectileT = GetComponent<Transform>();
        projectileDistance = projectileMaxDistance;
        playerT = player.GetComponent<Transform>();
        rbProjectile = projectile.GetComponent<Rigidbody2D>();
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
        if (atkStage != 0 || GameManager.isPaused) return;

        AudioManager.SfxPlayerAttack();
        atkStage++;
        //rb2d.AddForce(transform.right * projectileSpeed);
    }

    private void PreformAttack()
    {
        Vector2 playerDir = playerT.position - transform.position,
        originDir = origin - (Vector2)transform.position;

        switch (atkStage)
        {

            case 0:
                origin = transform.position;
                break;
            case 1:
                projectileT.position += transform.right * projectileSpeed * Time.deltaTime;
                if (originDir.magnitude >= projectileDistance)
                {
                    atkStage++;
                }
                break;
            case 2:
                rbProjectile.rotation = Mathf.Atan2(playerDir.y, playerDir.x);
                projectileT.position -= transform.right * projectileSpeed * Time.deltaTime;
                if (playerDir.magnitude <= 0.5)
                {
                    projectileT.position = playerT.position;
                    atkStage = 0;
                }
                break;
        }
    }
        
    

    }
